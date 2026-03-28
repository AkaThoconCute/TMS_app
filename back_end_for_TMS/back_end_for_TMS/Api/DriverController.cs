using back_end_for_TMS.Business;
using back_end_for_TMS.Business.Types;
using back_end_for_TMS.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end_for_TMS.Api;

[ApiController]
[Route("api/[controller]/[action]")]
public class DriverController(DriverService driverService) : ControllerBase
{
  /// <summary>
  /// Create a new driver
  /// </summary>
  [HttpPost]
  [Authorize]
  public async Task<ActionResult<DriverDto>> Create([FromBody] CreateDriverDto dto)
  {
    var result = await driverService.CreateAsync(dto);
    return CreatedAtAction(nameof(GetById), new { driverId = result.DriverId }, result);
  }

  /// <summary>
  /// Get driver by ID
  /// </summary>
  [HttpGet("{driverId}")]
  [Authorize]
  public async Task<ActionResult<DriverDto>> GetById([FromRoute] Guid driverId)
  {
    var result = await driverService.GetByIdAsync(driverId);
    return Ok(result);
  }

  /// <summary>
  /// List drivers with pagination and filtering
  /// </summary>
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<PaginatedResult<DriverDto>>> List(
      [FromQuery] int pageNumber = 1,
      [FromQuery] int pageSize = 10,
      [FromQuery] int? status = null,
      [FromQuery] string? searchTerm = null)
  {
    var result = await driverService.ListAsync(pageNumber, pageSize, status, searchTerm);
    return Ok(result);
  }

  /// <summary>
  /// Update driver information
  /// </summary>
  [HttpPut("{driverId}")]
  [Authorize]
  public async Task<ActionResult<DriverDto>> Update(
      [FromRoute] Guid driverId,
      [FromBody] UpdateDriverDto dto)
  {
    var result = await driverService.UpdateAsync(driverId, dto);
    return Ok(result);
  }

  /// <summary>
  /// Delete a driver
  /// </summary>
  [HttpDelete("{driverId}")]
  [Authorize]
  public async Task<ActionResult<bool>> Delete([FromRoute] Guid driverId)
  {
    var result = await driverService.DeleteAsync(driverId);
    return Ok(result);
  }
}
