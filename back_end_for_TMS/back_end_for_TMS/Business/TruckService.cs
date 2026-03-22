using AutoMapper;
using back_end_for_TMS.Business.Types;
using back_end_for_TMS.Models;
using back_end_for_TMS.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Business;

public class TruckService(TruckRepo truckRepository, IMapper mapper)
{
  public async Task<TruckDto> CreateAsync(CreateTruckDto dto)
  {
    if (string.IsNullOrEmpty(dto.LicensePlate))
      throw new ArgumentException("License plate cannot be null or empty", nameof(dto.LicensePlate));

    // Check if license plate already exists
    var existingTruck = await truckRepository.FindAsync(t => t.LicensePlate == dto.LicensePlate);

    if (existingTruck != null)
      throw new InvalidOperationException($"Truck with license plate '{dto.LicensePlate}' already exists");

    var truck = mapper.Map<Truck>(dto);
    truck.CreatedAt = DateTimeOffset.UtcNow;

    truckRepository.Add(truck);
    await truckRepository.SaveChangesAsync();

    return mapper.Map<TruckDto>(truck);
  }

  public async Task<TruckDto> GetByIdAsync(Guid truckId)
  {
    if (truckId == Guid.Empty)
      throw new ArgumentException("Truck ID cannot be empty", nameof(truckId));

    var truck = await truckRepository.FindAsync(t => t.TruckId == truckId);
    if (truck == null)
      throw new KeyNotFoundException($"Truck with ID '{truckId}' not found");

    return mapper.Map<TruckDto>(truck);
  }

  public async Task<TruckDto> GetByLicensePlateAsync(string licensePlate)
  {
    if (string.IsNullOrEmpty(licensePlate))
      throw new ArgumentException("License plate cannot be null or empty", nameof(licensePlate));

    var truck = await truckRepository.FindAsync(t => t.LicensePlate == licensePlate);

    if (truck == null)
      throw new KeyNotFoundException($"Truck with license plate '{licensePlate}' not found");

    return mapper.Map<TruckDto>(truck);
  }

  public async Task<PaginatedTrucksDto> ListAsync(
      int pageNumber = 1,
      int pageSize = 10,
      int? status = null,
      string? searchTerm = null)
  {
    if (pageNumber < 1) pageNumber = 1;
    if (pageSize < 1) pageSize = 10;
    if (pageSize > 100) pageSize = 100; // Max page size limit

    var query = truckRepository.Query();

    // Filter by status if provided
    if (status.HasValue)
      query = query.Where(t => t.CurrentStatus == status.Value);

    // Filter by search term (license plate or brand)
    if (!string.IsNullOrWhiteSpace(searchTerm))
    {
      var lowerSearchTerm = searchTerm.ToLower();
      query = query.Where(t =>
          t.LicensePlate.ToLower().Contains(lowerSearchTerm) ||
          (t.Brand != null && t.Brand.ToLower().Contains(lowerSearchTerm)));
    }

    var totalCount = await query.CountAsync();
    var trucks = await query
        .OrderByDescending(t => t.CreatedAt)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    var truckDtos = mapper.Map<List<TruckDto>>(trucks);

    return new PaginatedTrucksDto
    {
      Data = truckDtos,
      TotalCount = totalCount,
      PageSize = pageSize,
      PageNumber = pageNumber
    };
  }

  public async Task<TruckDto> UpdateAsync(Guid truckId, UpdateTruckDto dto)
  {
    if (truckId == Guid.Empty)
      throw new ArgumentException("Truck ID cannot be empty", nameof(truckId));

    var truck = await truckRepository.FindAsync(t => t.TruckId == truckId);
    if (truck == null)
      throw new KeyNotFoundException($"Truck with ID '{truckId}' not found");

    // Check if new license plate already exists (if being updated)
    if (!string.IsNullOrEmpty(dto.LicensePlate) && dto.LicensePlate != truck.LicensePlate)
    {
      var existingTruck = await truckRepository.FindAsync(t => t.LicensePlate == dto.LicensePlate);

      if (existingTruck != null)
        throw new InvalidOperationException($"Truck with license plate '{dto.LicensePlate}' already exists");
    }

    mapper.Map(dto, truck);
    truck.UpdatedAt = DateTimeOffset.UtcNow;

    truckRepository.Update(truck);
    await truckRepository.SaveChangesAsync();

    return mapper.Map<TruckDto>(truck);
  }

  public async Task<bool> DeleteAsync(Guid truckId)
  {
    if (truckId == Guid.Empty)
      throw new ArgumentException("Truck ID cannot be empty", nameof(truckId));

    var truck = await truckRepository.FindAsync(t => t.TruckId == truckId);
    if (truck == null)
      throw new KeyNotFoundException($"Truck with ID '{truckId}' not found");

    truckRepository.Remove(truck);
    await truckRepository.SaveChangesAsync();

    return true;
  }

  public async Task<bool> UpdateOdometerAsync(Guid truckId, decimal odometerReading)
  {
    if (truckId == Guid.Empty)
      throw new ArgumentException("Truck ID cannot be empty", nameof(truckId));

    var truck = await truckRepository.FindAsync(t => t.TruckId == truckId);
    if (truck == null)
      throw new KeyNotFoundException($"Truck with ID '{truckId}' not found");

    truck.OdometerReading = odometerReading;
    truck.UpdatedAt = DateTimeOffset.UtcNow;

    truckRepository.Update(truck);
    await truckRepository.SaveChangesAsync();

    return true;
  }

  public async Task<bool> UpdateStatusAsync(Guid truckId, int status)
  {
    if (truckId == Guid.Empty)
      throw new ArgumentException("Truck ID cannot be empty", nameof(truckId));

    var truck = await truckRepository.FindAsync(t => t.TruckId == truckId);
    if (truck == null)
      throw new KeyNotFoundException($"Truck with ID '{truckId}' not found");

    truck.CurrentStatus = status;
    truck.UpdatedAt = DateTimeOffset.UtcNow;

    truckRepository.Update(truck);
    await truckRepository.SaveChangesAsync();

    return true;
  }

  public async Task<bool> UpdateMaintenanceDateAsync(Guid truckId, DateTime maintenanceDate)
  {
    if (truckId == Guid.Empty)
      throw new ArgumentException("Truck ID cannot be empty", nameof(truckId));

    var truck = await truckRepository.FindAsync(t => t.TruckId == truckId);
    if (truck == null)
      throw new KeyNotFoundException($"Truck with ID '{truckId}' not found");

    truck.LastMaintenanceDate = maintenanceDate;
    truck.UpdatedAt = DateTimeOffset.UtcNow;

    truckRepository.Update(truck);
    await truckRepository.SaveChangesAsync();

    return true;
  }
}
