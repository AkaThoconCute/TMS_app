using AutoMapper;
using back_end_for_TMS.Business.Types;
using back_end_for_TMS.Common;
using back_end_for_TMS.Models;
using back_end_for_TMS.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Business;

public class TripService(TripRepo tripRepo, OrderRepo orderRepo, TruckRepo truckRepo, DriverRepo driverRepo, IMapper mapper)
{
  private static readonly string[] StatusLabels = ["Unknown", "Planned", "InTransit", "Completed", "Cancelled"];

  private static string GetStatusLabel(int status)
    => status >= 1 && status <= 4 ? StatusLabels[status] : "Unknown";

  private static readonly string[] DriverStatusLabels = ["Unknown", "Active", "OnLeave", "Terminated"];

  private static string GetDriverStatusLabel(int status)
    => status >= 1 && status <= 3 ? DriverStatusLabels[status] : "Unknown";

  public async Task<TripDto> CreateAsync(CreateTripDto dto)
  {
    if (dto.OrderId == Guid.Empty)
      throw new ArgumentException("Order ID is required.");

    if (dto.TruckId == Guid.Empty)
      throw new ArgumentException("Truck ID is required.");

    if (dto.DriverId == Guid.Empty)
      throw new ArgumentException("Driver ID is required.");

    // Validate order exists and is not terminal
    var order = await orderRepo.Query()
        .Include(o => o.Trips)
        .FirstOrDefaultAsync(o => o.OrderId == dto.OrderId);

    if (order == null)
      throw new KeyNotFoundException($"Order with ID '{dto.OrderId}' not found.");

    if (order.Status == 5 || order.Status == 6)
      throw new ArgumentException("Cannot create a trip for a completed or cancelled order.");

    // Validate order does not already have an active trip
    var activeTrip = order.Trips.FirstOrDefault(t => t.Status == 1 || t.Status == 2);
    if (activeTrip != null)
      throw new ArgumentException("This order already has an active trip. Cancel the existing trip first.");

    // Validate truck exists
    var truck = await truckRepo.FindAsync(t => t.TruckId == dto.TruckId);
    if (truck == null)
      throw new KeyNotFoundException($"Truck with ID '{dto.TruckId}' not found.");

    // Validate truck not assigned to another active trip
    var truckActiveTrip = await tripRepo.Query()
        .FirstOrDefaultAsync(t => t.TruckId == dto.TruckId && (t.Status == 1 || t.Status == 2));
    if (truckActiveTrip != null)
      throw new ArgumentException($"This truck is already assigned to active trip {truckActiveTrip.TripNumber}.");

    // Validate driver exists
    var driver = await driverRepo.FindAsync(d => d.DriverId == dto.DriverId);
    if (driver == null)
      throw new KeyNotFoundException($"Driver with ID '{dto.DriverId}' not found.");

    // Validate driver is active
    if (driver.Status != 1)
      throw new ArgumentException($"Driver is not available (status: {GetDriverStatusLabel(driver.Status)}).");

    // Validate driver license not expired
    if (driver.LicenseExpiry.HasValue && driver.LicenseExpiry.Value < DateTime.UtcNow.Date)
      throw new ArgumentException($"Driver's license expired on {driver.LicenseExpiry.Value:yyyy-MM-dd}.");

    // Validate driver not assigned to another active trip
    var driverActiveTrip = await tripRepo.Query()
        .FirstOrDefaultAsync(t => t.DriverId == dto.DriverId && (t.Status == 1 || t.Status == 2));
    if (driverActiveTrip != null)
      throw new ArgumentException($"This driver is already assigned to active trip {driverActiveTrip.TripNumber}.");

    // Validate dates
    if (dto.PlannedPickupDate.HasValue && dto.PlannedDeliveryDate.HasValue
        && dto.PlannedDeliveryDate.Value < dto.PlannedPickupDate.Value)
      throw new ArgumentException("Planned delivery date must be on or after planned pickup date.");

    // Auto-generate TripNumber
    var count = await tripRepo.Query().CountAsync();
    var tripNumber = $"TRP-{(count + 1):D6}";

    var trip = mapper.Map<Trip>(dto);
    trip.TripNumber = tripNumber;
    trip.Status = 1; // Planned
    trip.CreatedAt = DateTimeOffset.UtcNow;

    tripRepo.Add(trip);

    // CASCADE: Set order status to Assigned if currently Created
    if (order.Status == 1)
    {
      order.Status = 2; // Assigned
      order.UpdatedAt = DateTimeOffset.UtcNow;
      orderRepo.Update(order);
    }

    await tripRepo.SaveChangesAsync();

    // Reload with navigation properties for mapping
    var created = await tripRepo.Query()
        .Include(t => t.Order)
        .Include(t => t.Truck)
        .Include(t => t.Driver)
        .FirstAsync(t => t.TripId == trip.TripId);

    return mapper.Map<TripDto>(created);
  }

  public async Task<TripDto> GetByIdAsync(Guid tripId)
  {
    if (tripId == Guid.Empty)
      throw new ArgumentException("Trip ID cannot be empty.");

    var trip = await tripRepo.Query()
        .Include(t => t.Order)
        .Include(t => t.Truck)
        .Include(t => t.Driver)
        .FirstOrDefaultAsync(t => t.TripId == tripId);

    if (trip == null)
      throw new KeyNotFoundException($"Trip with ID '{tripId}' not found.");

    return mapper.Map<TripDto>(trip);
  }

  public async Task<PaginatedResult<TripDto>> ListAsync(
      int pageNumber = 1,
      int pageSize = 10,
      Guid? orderId = null,
      int? status = null)
  {
    if (pageNumber < 1) pageNumber = 1;
    if (pageSize < 1) pageSize = 1;
    if (pageSize > 100) pageSize = 100;

    var query = tripRepo.Query()
        .Include(t => t.Order)
        .Include(t => t.Truck)
        .Include(t => t.Driver)
        .AsQueryable();

    if (orderId.HasValue)
      query = query.Where(t => t.OrderId == orderId.Value);

    if (status.HasValue)
      query = query.Where(t => t.Status == status.Value);

    var totalCount = await query.CountAsync();
    var trips = await query
        .OrderByDescending(t => t.CreatedAt)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    var tripDtos = mapper.Map<List<TripDto>>(trips);

    return new PaginatedResult<TripDto>
    {
      Data = tripDtos,
      TotalCount = totalCount,
      PageSize = pageSize,
      PageNumber = pageNumber
    };
  }

  public async Task<TripDto> UpdateAsync(Guid tripId, UpdateTripDto dto)
  {
    if (tripId == Guid.Empty)
      throw new ArgumentException("Trip ID cannot be empty.");

    var trip = await tripRepo.Query()
        .Include(t => t.Order)
        .Include(t => t.Truck)
        .Include(t => t.Driver)
        .FirstOrDefaultAsync(t => t.TripId == tripId);

    if (trip == null)
      throw new KeyNotFoundException($"Trip with ID '{tripId}' not found.");

    // Cannot update terminal-status trips
    if (trip.Status == 3 || trip.Status == 4)
      throw new ArgumentException("Cannot update a closed trip.");

    // Cannot change truck or driver while in transit
    if (trip.Status == 2)
    {
      if (dto.TruckId.HasValue && dto.TruckId.Value != trip.TruckId)
        throw new ArgumentException("Cannot change truck or driver while trip is in transit.");

      if (dto.DriverId.HasValue && dto.DriverId.Value != trip.DriverId)
        throw new ArgumentException("Cannot change truck or driver while trip is in transit.");
    }

    // Validate new truck if provided
    if (dto.TruckId.HasValue)
    {
      var truck = await truckRepo.FindAsync(t => t.TruckId == dto.TruckId.Value);
      if (truck == null)
        throw new KeyNotFoundException($"Truck with ID '{dto.TruckId.Value}' not found.");

      var truckActiveTrip = await tripRepo.Query()
          .FirstOrDefaultAsync(t => t.TruckId == dto.TruckId.Value
              && (t.Status == 1 || t.Status == 2)
              && t.TripId != tripId);
      if (truckActiveTrip != null)
        throw new ArgumentException($"This truck is already assigned to active trip {truckActiveTrip.TripNumber}.");
    }

    // Validate new driver if provided
    if (dto.DriverId.HasValue)
    {
      var driver = await driverRepo.FindAsync(d => d.DriverId == dto.DriverId.Value);
      if (driver == null)
        throw new KeyNotFoundException($"Driver with ID '{dto.DriverId.Value}' not found.");

      if (driver.Status != 1)
        throw new ArgumentException($"Driver is not available (status: {GetDriverStatusLabel(driver.Status)}).");

      if (driver.LicenseExpiry.HasValue && driver.LicenseExpiry.Value < DateTime.UtcNow.Date)
        throw new ArgumentException($"Driver's license expired on {driver.LicenseExpiry.Value:yyyy-MM-dd}.");

      var driverActiveTrip = await tripRepo.Query()
          .FirstOrDefaultAsync(t => t.DriverId == dto.DriverId.Value
              && (t.Status == 1 || t.Status == 2)
              && t.TripId != tripId);
      if (driverActiveTrip != null)
        throw new ArgumentException($"This driver is already assigned to active trip {driverActiveTrip.TripNumber}.");
    }

    // Cross-check dates using effective values
    var effectivePickup = dto.PlannedPickupDate ?? trip.PlannedPickupDate;
    var effectiveDelivery = dto.PlannedDeliveryDate ?? trip.PlannedDeliveryDate;
    if (effectivePickup.HasValue && effectiveDelivery.HasValue && effectiveDelivery.Value < effectivePickup.Value)
      throw new ArgumentException("Planned delivery date must be on or after planned pickup date.");

    mapper.Map(dto, trip);
    trip.UpdatedAt = DateTimeOffset.UtcNow;

    tripRepo.Update(trip);
    await tripRepo.SaveChangesAsync();

    return mapper.Map<TripDto>(trip);
  }

  public async Task<TripDto> StartAsync(Guid tripId, StartTripDto dto)
  {
    if (tripId == Guid.Empty)
      throw new ArgumentException("Trip ID cannot be empty.");

    var trip = await tripRepo.Query()
        .Include(t => t.Order)
        .Include(t => t.Truck)
        .Include(t => t.Driver)
        .FirstOrDefaultAsync(t => t.TripId == tripId);

    if (trip == null)
      throw new KeyNotFoundException($"Trip with ID '{tripId}' not found.");

    if (trip.Status != 1)
      throw new ArgumentException($"Trip can only be started from Planned status. Current status: {GetStatusLabel(trip.Status)}.");

    trip.Status = 2; // InTransit
    trip.ActualPickupDate = dto.ActualPickupDate ?? DateTime.UtcNow;
    trip.UpdatedAt = DateTimeOffset.UtcNow;

    tripRepo.Update(trip);

    // CASCADE: Set order status to InTransit if currently Assigned
    if (trip.Order.Status == 2)
    {
      trip.Order.Status = 3; // InTransit
      trip.Order.UpdatedAt = DateTimeOffset.UtcNow;
      orderRepo.Update(trip.Order);
    }

    await tripRepo.SaveChangesAsync();

    return mapper.Map<TripDto>(trip);
  }

  public async Task<TripDto> CompleteAsync(CompleteTripDto dto)
  {
    if (dto.TripId == Guid.Empty)
      throw new ArgumentException("Trip ID is required.");

    var trip = await tripRepo.Query()
        .Include(t => t.Order).ThenInclude(o => o.Trips)
        .Include(t => t.Truck)
        .Include(t => t.Driver)
        .FirstOrDefaultAsync(t => t.TripId == dto.TripId);

    if (trip == null)
      throw new KeyNotFoundException($"Trip with ID '{dto.TripId}' not found.");

    if (trip.Status != 2)
      throw new ArgumentException($"Trip can only be completed from InTransit status. Current: {GetStatusLabel(trip.Status)}.");

    if (!trip.ActualPickupDate.HasValue)
      throw new ArgumentException("Trip must have an actual pickup date before completing.");

    if (dto.ActualDeliveryDate < trip.ActualPickupDate.Value)
      throw new ArgumentException("Delivery date cannot be before pickup date.");

    // Validate costs non-negative
    if ((dto.FuelCost.HasValue && dto.FuelCost.Value < 0)
        || (dto.TollCost.HasValue && dto.TollCost.Value < 0)
        || (dto.OtherCost.HasValue && dto.OtherCost.Value < 0))
      throw new ArgumentException("Cost values must be non-negative.");

    trip.Status = 3; // Completed
    trip.ActualDeliveryDate = dto.ActualDeliveryDate;
    trip.FuelCost = dto.FuelCost;
    trip.TollCost = dto.TollCost;
    trip.OtherCost = dto.OtherCost;
    trip.CostNotes = dto.CostNotes;
    if (!string.IsNullOrWhiteSpace(dto.Notes))
      trip.Notes = dto.Notes;
    trip.CompletedAt = DateTimeOffset.UtcNow;
    trip.UpdatedAt = DateTimeOffset.UtcNow;

    tripRepo.Update(trip);

    // CASCADE: If no other active trips on the order, set order to Delivered
    var hasOtherActiveTrips = trip.Order.Trips
        .Any(t => t.TripId != trip.TripId && (t.Status == 1 || t.Status == 2));

    if (!hasOtherActiveTrips)
    {
      trip.Order.Status = 4; // Delivered
      trip.Order.UpdatedAt = DateTimeOffset.UtcNow;
      orderRepo.Update(trip.Order);
    }

    await tripRepo.SaveChangesAsync();

    return mapper.Map<TripDto>(trip);
  }

  public async Task<TripDto> CancelAsync(CancelTripDto dto)
  {
    if (dto.TripId == Guid.Empty)
      throw new ArgumentException("Trip ID is required.");

    if (string.IsNullOrWhiteSpace(dto.CancellationReason))
      throw new ArgumentException("Cancellation reason is required.");

    var trip = await tripRepo.Query()
        .Include(t => t.Order).ThenInclude(o => o.Trips)
        .Include(t => t.Truck)
        .Include(t => t.Driver)
        .FirstOrDefaultAsync(t => t.TripId == dto.TripId);

    if (trip == null)
      throw new KeyNotFoundException($"Trip with ID '{dto.TripId}' not found.");

    if (trip.Status == 3)
      throw new ArgumentException("Cannot cancel a completed trip.");

    if (trip.Status == 4)
      throw new ArgumentException("Trip is already cancelled.");

    trip.Status = 4; // Cancelled
    trip.CancellationReason = dto.CancellationReason;
    trip.CancelledAt = DateTimeOffset.UtcNow;
    trip.UpdatedAt = DateTimeOffset.UtcNow;

    tripRepo.Update(trip);

    // CASCADE: If no other active trips on the order, revert order to Created
    var hasOtherActiveTrips = trip.Order.Trips
        .Any(t => t.TripId != trip.TripId && (t.Status == 1 || t.Status == 2));

    if (!hasOtherActiveTrips)
    {
      trip.Order.Status = 1; // Created
      trip.Order.UpdatedAt = DateTimeOffset.UtcNow;
      orderRepo.Update(trip.Order);
    }

    await tripRepo.SaveChangesAsync();

    return mapper.Map<TripDto>(trip);
  }
}
