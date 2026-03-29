using back_end_for_TMS.Business;
using back_end_for_TMS.Business.Types;
using back_end_for_TMS.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end_for_TMS.Api;

[ApiController]
[Route("api/[controller]/[action]")]
public class CustomerController(CustomerService customerService) : ControllerBase
{
  /// <summary>
  /// Create a new customer
  /// </summary>
  [HttpPost]
  [Authorize]
  public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerDto dto)
  {
    var result = await customerService.CreateAsync(dto);
    return CreatedAtAction(nameof(GetById), new { customerId = result.CustomerId }, result);
  }

  /// <summary>
  /// Get customer by ID
  /// </summary>
  [HttpGet("{customerId}")]
  [Authorize]
  public async Task<ActionResult<CustomerDto>> GetById([FromRoute] Guid customerId)
  {
    var result = await customerService.GetByIdAsync(customerId);
    return Ok(result);
  }

  /// <summary>
  /// List customers with pagination and filtering
  /// </summary>
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<PaginatedResult<CustomerDto>>> List(
      [FromQuery] int pageNumber = 1,
      [FromQuery] int pageSize = 10,
      [FromQuery] int? status = null,
      [FromQuery] int? customerType = null,
      [FromQuery] string? searchTerm = null)
  {
    var result = await customerService.ListAsync(pageNumber, pageSize, status, customerType, searchTerm);
    return Ok(result);
  }

  /// <summary>
  /// Update customer information
  /// </summary>
  [HttpPut("{customerId}")]
  [Authorize]
  public async Task<ActionResult<CustomerDto>> Update(
      [FromRoute] Guid customerId,
      [FromBody] UpdateCustomerDto dto)
  {
    var result = await customerService.UpdateAsync(customerId, dto);
    return Ok(result);
  }

  /// <summary>
  /// Delete a customer
  /// </summary>
  [HttpDelete("{customerId}")]
  [Authorize]
  public async Task<ActionResult<bool>> Delete([FromRoute] Guid customerId)
  {
    var result = await customerService.DeleteAsync(customerId);
    return Ok(result);
  }
}
