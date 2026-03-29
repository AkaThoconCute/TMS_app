using AutoMapper;
using back_end_for_TMS.Business.Types;
using back_end_for_TMS.Common;
using back_end_for_TMS.Models;
using back_end_for_TMS.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Business;

public class CustomerService(CustomerRepo customerRepository, IMapper mapper)
{
  public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
  {
    if (string.IsNullOrEmpty(dto.Name))
      throw new ArgumentException("Name cannot be null or empty", nameof(dto.Name));

    if (string.IsNullOrEmpty(dto.PhoneNumber))
      throw new ArgumentException("Phone number cannot be null or empty", nameof(dto.PhoneNumber));

    if (dto.CustomerType != 1 && dto.CustomerType != 2)
      throw new ArgumentException("CustomerType must be 1 (Individual) or 2 (Business)", nameof(dto.CustomerType));

    if (dto.Status != 1 && dto.Status != 2)
      throw new ArgumentException("Status must be 1 (Active) or 2 (Inactive)", nameof(dto.Status));

    // Check if phone number already exists for this tenant
    var existingCustomer = await customerRepository.FindAsync(c => c.PhoneNumber == dto.PhoneNumber);
    if (existingCustomer != null)
      throw new InvalidOperationException($"Customer with phone number '{dto.PhoneNumber}' already exists");

    var customer = mapper.Map<Customer>(dto);
    customer.CreatedAt = DateTimeOffset.UtcNow;

    customerRepository.Add(customer);
    await customerRepository.SaveChangesAsync();

    return mapper.Map<CustomerDto>(customer);
  }

  public async Task<CustomerDto> GetByIdAsync(Guid customerId)
  {
    if (customerId == Guid.Empty)
      throw new ArgumentException("Customer ID cannot be empty", nameof(customerId));

    var customer = await customerRepository.FindAsync(c => c.CustomerId == customerId);
    if (customer == null)
      throw new KeyNotFoundException($"Customer with ID '{customerId}' not found");

    return mapper.Map<CustomerDto>(customer);
  }

  public async Task<PaginatedResult<CustomerDto>> ListAsync(
      int pageNumber = 1,
      int pageSize = 10,
      int? status = null,
      int? customerType = null,
      string? searchTerm = null)
  {
    if (pageNumber < 1) pageNumber = 1;
    if (pageSize < 1) pageSize = 1;
    if (pageSize > 100) pageSize = 100;

    var query = customerRepository.Query();

    // Filter by status if provided
    if (status.HasValue)
      query = query.Where(c => c.Status == status.Value);

    // Filter by customerType if provided
    if (customerType.HasValue)
      query = query.Where(c => c.CustomerType == customerType.Value);

    // Filter by search term (name, contactPerson, or phoneNumber)
    if (!string.IsNullOrWhiteSpace(searchTerm))
    {
      var lowerSearchTerm = searchTerm.ToLower();
      query = query.Where(c =>
          c.Name.ToLower().Contains(lowerSearchTerm) ||
          (c.ContactPerson != null && c.ContactPerson.ToLower().Contains(lowerSearchTerm)) ||
          c.PhoneNumber.ToLower().Contains(lowerSearchTerm));
    }

    var totalCount = await query.CountAsync();
    var customers = await query
        .OrderByDescending(c => c.CreatedAt)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    var customerDtos = mapper.Map<List<CustomerDto>>(customers);

    return new PaginatedResult<CustomerDto>
    {
      Data = customerDtos,
      TotalCount = totalCount,
      PageSize = pageSize,
      PageNumber = pageNumber
    };
  }

  public async Task<CustomerDto> UpdateAsync(Guid customerId, UpdateCustomerDto dto)
  {
    if (customerId == Guid.Empty)
      throw new ArgumentException("Customer ID cannot be empty", nameof(customerId));

    var customer = await customerRepository.FindAsync(c => c.CustomerId == customerId);
    if (customer == null)
      throw new KeyNotFoundException($"Customer with ID '{customerId}' not found");

    // Check if new phone number already exists (if being updated)
    if (!string.IsNullOrEmpty(dto.PhoneNumber) && dto.PhoneNumber != customer.PhoneNumber)
    {
      var existingCustomer = await customerRepository.FindAsync(c => c.PhoneNumber == dto.PhoneNumber);
      if (existingCustomer != null)
        throw new InvalidOperationException($"Customer with phone number '{dto.PhoneNumber}' already exists");
    }

    if (dto.CustomerType.HasValue && dto.CustomerType != 1 && dto.CustomerType != 2)
      throw new ArgumentException("CustomerType must be 1 (Individual) or 2 (Business)", nameof(dto.CustomerType));

    if (dto.Status.HasValue && dto.Status != 1 && dto.Status != 2)
      throw new ArgumentException("Status must be 1 (Active) or 2 (Inactive)", nameof(dto.Status));

    mapper.Map(dto, customer);
    customer.UpdatedAt = DateTimeOffset.UtcNow;

    customerRepository.Update(customer);
    await customerRepository.SaveChangesAsync();

    return mapper.Map<CustomerDto>(customer);
  }

  public async Task<bool> DeleteAsync(Guid customerId)
  {
    if (customerId == Guid.Empty)
      throw new ArgumentException("Customer ID cannot be empty", nameof(customerId));

    var customer = await customerRepository.FindAsync(c => c.CustomerId == customerId);
    if (customer == null)
      throw new KeyNotFoundException($"Customer with ID '{customerId}' not found");

    customerRepository.Remove(customer);
    await customerRepository.SaveChangesAsync();

    return true;
  }
}
