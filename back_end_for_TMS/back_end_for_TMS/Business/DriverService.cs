using AutoMapper;
using back_end_for_TMS.Business.Types;
using back_end_for_TMS.Common;
using back_end_for_TMS.Models;
using back_end_for_TMS.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Business;

public class DriverService(DriverRepo driverRepository, IMapper mapper)
{
  public async Task<DriverDto> CreateAsync(CreateDriverDto dto)
  {
    if (string.IsNullOrEmpty(dto.FullName))
      throw new ArgumentException("Full name cannot be null or empty", nameof(dto.FullName));

    if (string.IsNullOrEmpty(dto.PhoneNumber))
      throw new ArgumentException("Phone number cannot be null or empty", nameof(dto.PhoneNumber));

    if (string.IsNullOrEmpty(dto.LicenseNumber))
      throw new ArgumentException("License number cannot be null or empty", nameof(dto.LicenseNumber));

    // Check if phone number already exists
    var existingDriver = await driverRepository.FindAsync(d => d.PhoneNumber == dto.PhoneNumber);

    if (existingDriver != null)
      throw new InvalidOperationException($"Driver with phone number '{dto.PhoneNumber}' already exists");

    var driver = mapper.Map<Driver>(dto);
    driver.CreatedAt = DateTimeOffset.UtcNow;

    driverRepository.Add(driver);
    await driverRepository.SaveChangesAsync();

    return mapper.Map<DriverDto>(driver);
  }

  public async Task<DriverDto> GetByIdAsync(Guid driverId)
  {
    if (driverId == Guid.Empty)
      throw new ArgumentException("Driver ID cannot be empty", nameof(driverId));

    var driver = await driverRepository.FindAsync(d => d.DriverId == driverId);
    if (driver == null)
      throw new KeyNotFoundException($"Driver with ID '{driverId}' not found");

    return mapper.Map<DriverDto>(driver);
  }

  public async Task<PaginatedResult<DriverDto>> ListAsync(
      int pageNumber = 1,
      int pageSize = 10,
      int? status = null,
      string? searchTerm = null)
  {
    if (pageNumber < 1) pageNumber = 1;
    if (pageSize < 1) pageSize = 1;
    if (pageSize > 100) pageSize = 100;

    var query = driverRepository.Query();

    // Filter by status if provided
    if (status.HasValue)
      query = query.Where(d => d.Status == status.Value);

    // Filter by search term (full name or phone number)
    if (!string.IsNullOrWhiteSpace(searchTerm))
    {
      var lowerSearchTerm = searchTerm.ToLower();
      query = query.Where(d =>
          d.FullName.ToLower().Contains(lowerSearchTerm) ||
          d.PhoneNumber.ToLower().Contains(lowerSearchTerm));
    }

    var totalCount = await query.CountAsync();
    var drivers = await query
        .OrderByDescending(d => d.CreatedAt)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    var driverDtos = mapper.Map<List<DriverDto>>(drivers);

    return new PaginatedResult<DriverDto>
    {
      Data = driverDtos,
      TotalCount = totalCount,
      PageSize = pageSize,
      PageNumber = pageNumber
    };
  }

  public async Task<DriverDto> UpdateAsync(Guid driverId, UpdateDriverDto dto)
  {
    if (driverId == Guid.Empty)
      throw new ArgumentException("Driver ID cannot be empty", nameof(driverId));

    var driver = await driverRepository.FindAsync(d => d.DriverId == driverId);
    if (driver == null)
      throw new KeyNotFoundException($"Driver with ID '{driverId}' not found");

    // Check if new phone number already exists (if being updated)
    if (!string.IsNullOrEmpty(dto.PhoneNumber) && dto.PhoneNumber != driver.PhoneNumber)
    {
      var existingDriver = await driverRepository.FindAsync(d => d.PhoneNumber == dto.PhoneNumber);

      if (existingDriver != null)
        throw new InvalidOperationException($"Driver with phone number '{dto.PhoneNumber}' already exists");
    }

    mapper.Map(dto, driver);
    driver.UpdatedAt = DateTimeOffset.UtcNow;

    driverRepository.Update(driver);
    await driverRepository.SaveChangesAsync();

    return mapper.Map<DriverDto>(driver);
  }

  public async Task<bool> DeleteAsync(Guid driverId)
  {
    if (driverId == Guid.Empty)
      throw new ArgumentException("Driver ID cannot be empty", nameof(driverId));

    var driver = await driverRepository.FindAsync(d => d.DriverId == driverId);
    if (driver == null)
      throw new KeyNotFoundException($"Driver with ID '{driverId}' not found");

    driverRepository.Remove(driver);
    await driverRepository.SaveChangesAsync();

    return true;
  }
}
