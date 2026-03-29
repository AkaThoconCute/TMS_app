using AutoMapper;
using back_end_for_TMS.Business.Types;
using back_end_for_TMS.Common;
using back_end_for_TMS.Models;
using back_end_for_TMS.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Business;

public class OrderService(OrderRepo orderRepo, CustomerRepo customerRepo, TripRepo tripRepo, IMapper mapper)
{
  private static readonly string[] StatusLabels = ["Unknown", "Created", "Assigned", "InTransit", "Delivered", "Completed", "Cancelled"];

  private static string GetStatusLabel(int status)
    => status >= 1 && status <= 6 ? StatusLabels[status] : "Unknown";

  public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
  {
    if (dto.CustomerId == Guid.Empty)
      throw new ArgumentException("Customer ID is required.");

    if (string.IsNullOrWhiteSpace(dto.PickupAddress))
      throw new ArgumentException("Pickup address is required.");

    if (string.IsNullOrWhiteSpace(dto.DeliveryAddress))
      throw new ArgumentException("Delivery address is required.");

    if (string.IsNullOrWhiteSpace(dto.CargoDescription))
      throw new ArgumentException("Cargo description is required.");

    if (dto.CargoWeightKg.HasValue && dto.CargoWeightKg.Value <= 0)
      throw new ArgumentException("Cargo weight must be greater than 0.");

    if (dto.QuotedPrice.HasValue && dto.QuotedPrice.Value < 0)
      throw new ArgumentException("Quoted price must be non-negative.");

    if (dto.RequestedPickupDate.HasValue && dto.RequestedDeliveryDate.HasValue
        && dto.RequestedDeliveryDate.Value < dto.RequestedPickupDate.Value)
      throw new ArgumentException("Delivery date must be on or after pickup date.");

    // Validate customer exists and is Active
    var customer = await customerRepo.FindAsync(c => c.CustomerId == dto.CustomerId);
    if (customer == null)
      throw new KeyNotFoundException($"Customer with ID '{dto.CustomerId}' not found.");

    if (customer.Status != 1)
      throw new ArgumentException("Customer must be Active to create an order.");

    // Auto-generate OrderNumber
    var count = await orderRepo.Query().CountAsync();
    var orderNumber = $"ORD-{(count + 1):D6}";

    var order = mapper.Map<Order>(dto);
    order.OrderNumber = orderNumber;
    order.Status = 1; // Created
    order.CreatedAt = DateTimeOffset.UtcNow;

    orderRepo.Add(order);
    await orderRepo.SaveChangesAsync();

    // Reload with Customer navigation for mapping
    var created = await orderRepo.Query()
        .Include(o => o.Customer)
        .Include(o => o.Trips)
        .FirstAsync(o => o.OrderId == order.OrderId);

    return mapper.Map<OrderDto>(created);
  }

  public async Task<OrderDto> GetByIdAsync(Guid orderId)
  {
    if (orderId == Guid.Empty)
      throw new ArgumentException("Order ID cannot be empty.");

    var order = await orderRepo.Query()
        .Include(o => o.Customer)
        .Include(o => o.Trips)
        .FirstOrDefaultAsync(o => o.OrderId == orderId);

    if (order == null)
      throw new KeyNotFoundException($"Order with ID '{orderId}' not found.");

    return mapper.Map<OrderDto>(order);
  }

  public async Task<PaginatedResult<OrderDto>> ListAsync(
      int pageNumber = 1,
      int pageSize = 10,
      int? status = null,
      Guid? customerId = null,
      string? searchTerm = null)
  {
    if (pageNumber < 1) pageNumber = 1;
    if (pageSize < 1) pageSize = 1;
    if (pageSize > 100) pageSize = 100;

    var query = orderRepo.Query()
        .Include(o => o.Customer)
        .Include(o => o.Trips)
        .AsQueryable();

    if (status.HasValue)
      query = query.Where(o => o.Status == status.Value);

    if (customerId.HasValue)
      query = query.Where(o => o.CustomerId == customerId.Value);

    if (!string.IsNullOrWhiteSpace(searchTerm))
    {
      var lowerSearch = searchTerm.ToLower();
      query = query.Where(o =>
          o.OrderNumber.ToLower().Contains(lowerSearch) ||
          o.PickupAddress.ToLower().Contains(lowerSearch) ||
          o.DeliveryAddress.ToLower().Contains(lowerSearch) ||
          o.CargoDescription.ToLower().Contains(lowerSearch));
    }

    var totalCount = await query.CountAsync();
    var orders = await query
        .OrderByDescending(o => o.CreatedAt)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    var orderDtos = mapper.Map<List<OrderDto>>(orders);

    return new PaginatedResult<OrderDto>
    {
      Data = orderDtos,
      TotalCount = totalCount,
      PageSize = pageSize,
      PageNumber = pageNumber
    };
  }

  public async Task<OrderStatusSummaryDto> GetStatusSummaryAsync()
  {
    var counts = await orderRepo.Query()
        .GroupBy(o => o.Status)
        .Select(g => new { Status = g.Key, Count = g.Count() })
        .ToListAsync();

    return new OrderStatusSummaryDto
    {
      Created = counts.FirstOrDefault(c => c.Status == 1)?.Count ?? 0,
      Assigned = counts.FirstOrDefault(c => c.Status == 2)?.Count ?? 0,
      InTransit = counts.FirstOrDefault(c => c.Status == 3)?.Count ?? 0,
      Delivered = counts.FirstOrDefault(c => c.Status == 4)?.Count ?? 0,
      Completed = counts.FirstOrDefault(c => c.Status == 5)?.Count ?? 0,
      Cancelled = counts.FirstOrDefault(c => c.Status == 6)?.Count ?? 0,
    };
  }

  public async Task<OrderDto> UpdateAsync(Guid orderId, UpdateOrderDto dto)
  {
    if (orderId == Guid.Empty)
      throw new ArgumentException("Order ID cannot be empty.");

    var order = await orderRepo.Query()
        .Include(o => o.Customer)
        .Include(o => o.Trips)
        .FirstOrDefaultAsync(o => o.OrderId == orderId);

    if (order == null)
      throw new KeyNotFoundException($"Order with ID '{orderId}' not found.");

    // Cannot update terminal-status orders
    if (order.Status == 5 || order.Status == 6)
      throw new ArgumentException("Cannot update a closed order.");

    if (dto.CargoWeightKg.HasValue && dto.CargoWeightKg.Value <= 0)
      throw new ArgumentException("Cargo weight must be greater than 0.");

    if (dto.QuotedPrice.HasValue && dto.QuotedPrice.Value < 0)
      throw new ArgumentException("Quoted price must be non-negative.");

    // Cross-check dates: use updated values if provided, otherwise use existing values
    var effectivePickup = dto.RequestedPickupDate ?? order.RequestedPickupDate;
    var effectiveDelivery = dto.RequestedDeliveryDate ?? order.RequestedDeliveryDate;
    if (effectivePickup.HasValue && effectiveDelivery.HasValue && effectiveDelivery.Value < effectivePickup.Value)
      throw new ArgumentException("Delivery date must be on or after pickup date.");

    mapper.Map(dto, order);
    order.UpdatedAt = DateTimeOffset.UtcNow;

    orderRepo.Update(order);
    await orderRepo.SaveChangesAsync();

    return mapper.Map<OrderDto>(order);
  }

  public async Task<OrderDto> CompleteAsync(Guid orderId, CompleteOrderDto dto)
  {
    if (orderId == Guid.Empty)
      throw new ArgumentException("Order ID cannot be empty.");

    var order = await orderRepo.Query()
        .Include(o => o.Customer)
        .Include(o => o.Trips)
        .FirstOrDefaultAsync(o => o.OrderId == orderId);

    if (order == null)
      throw new KeyNotFoundException($"Order with ID '{orderId}' not found.");

    if (order.Status != 4) // Must be Delivered
      throw new ArgumentException($"Order can only be completed from Delivered status. Current: {GetStatusLabel(order.Status)}.");

    // All trips must be in terminal state (Completed=3 or Cancelled=4)
    var hasActiveTrips = order.Trips.Any(t => t.Status == 1 || t.Status == 2);
    if (hasActiveTrips)
      throw new ArgumentException("Cannot complete order while trips are still active.");

    order.Status = 5; // Completed
    order.CompletedAt = DateTimeOffset.UtcNow;
    order.UpdatedAt = DateTimeOffset.UtcNow;

    if (!string.IsNullOrWhiteSpace(dto.Notes))
      order.Notes = dto.Notes;

    orderRepo.Update(order);
    await orderRepo.SaveChangesAsync();

    return mapper.Map<OrderDto>(order);
  }

  public async Task<OrderDto> CancelAsync(CancelOrderDto dto)
  {
    if (dto.OrderId == Guid.Empty)
      throw new ArgumentException("Order ID is required.");

    if (string.IsNullOrWhiteSpace(dto.CancellationReason))
      throw new ArgumentException("Cancellation reason is required.");

    var order = await orderRepo.Query()
        .Include(o => o.Customer)
        .Include(o => o.Trips)
        .FirstOrDefaultAsync(o => o.OrderId == dto.OrderId);

    if (order == null)
      throw new KeyNotFoundException($"Order with ID '{dto.OrderId}' not found.");

    if (order.Status == 5)
      throw new ArgumentException("Cannot cancel a completed order.");

    if (order.Status == 6)
      throw new ArgumentException("Order is already cancelled.");

    // Auto-cancel all active trips (Planned=1 or InTransit=2)
    var activeTrips = order.Trips.Where(t => t.Status == 1 || t.Status == 2).ToList();
    foreach (var trip in activeTrips)
    {
      trip.Status = 4; // Cancelled
      trip.CancellationReason = "Parent order cancelled";
      trip.CancelledAt = DateTimeOffset.UtcNow;
      trip.UpdatedAt = DateTimeOffset.UtcNow;
      tripRepo.Update(trip);
    }

    order.Status = 6; // Cancelled
    order.CancellationReason = dto.CancellationReason;
    order.CancelledAt = DateTimeOffset.UtcNow;
    order.UpdatedAt = DateTimeOffset.UtcNow;

    orderRepo.Update(order);
    await orderRepo.SaveChangesAsync();

    return mapper.Map<OrderDto>(order);
  }
}
