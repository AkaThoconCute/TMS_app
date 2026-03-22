using back_end_for_TMS.Business;
using back_end_for_TMS.Business.Types;
using back_end_for_TMS.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end_for_TMS.Api;

[ApiController]
[Route("api/[controller]/[action]")]
public class TruckController(TruckService truckService) : ControllerBase
{
  /// <summary>
  /// Create a new truck
  /// </summary>
  [HttpPost]
  [Authorize]
  public async Task<ActionResult<TruckDto>> Create([FromBody] CreateTruckDto dto)
  {
    var result = await truckService.CreateAsync(dto);
    return CreatedAtAction(nameof(GetById), new { truckId = result.TruckId }, result);
  }

  /// <summary>
  /// Get truck by ID
  /// </summary>
  [HttpGet("{truckId}")]
  [Authorize]
  public async Task<ActionResult<TruckDto>> GetById([FromRoute] Guid truckId)
  {
    var result = await truckService.GetByIdAsync(truckId);
    return Ok(result);
  }

  /// <summary>
  /// Get truck by license plate
  /// </summary>
  [HttpGet("{licensePlate}")]
  [Authorize]
  public async Task<ActionResult<TruckDto>> GetByLicensePlate([FromRoute] string licensePlate)
  {
    var result = await truckService.GetByLicensePlateAsync(licensePlate);
    return Ok(result);
  }

  /// <summary>
  /// List trucks with pagination and filtering
  /// </summary>
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<PaginatedResult<TruckDto>>> List(
      [FromQuery] int pageNumber = 1,
      [FromQuery] int pageSize = 10,
      [FromQuery] int? status = null,
      [FromQuery] string? searchTerm = null)
  {
    var result = await truckService.ListAsync(pageNumber, pageSize, status, searchTerm);
    return Ok(result);
  }

  /// <summary>
  /// Update truck information
  /// </summary>
  [HttpPut("{truckId}")]
  [Authorize]
  public async Task<ActionResult<TruckDto>> Update(
      [FromRoute] Guid truckId,
      [FromBody] UpdateTruckDto dto)
  {
    var result = await truckService.UpdateAsync(truckId, dto);
    return Ok(result);
  }

  /// <summary>
  /// Delete a truck
  /// </summary>
  [HttpDelete("{truckId}")]
  [Authorize]
  public async Task<ActionResult<bool>> Delete([FromRoute] Guid truckId)
  {
    var result = await truckService.DeleteAsync(truckId);
    return Ok(result);
  }

  /// <summary>
  /// Update truck odometer reading
  /// </summary>
  [HttpPatch("{truckId}/odometer")]
  [Authorize]
  public async Task<ActionResult<bool>> UpdateOdometer(
      [FromRoute] Guid truckId,
      [FromBody] UpdateOdometerDto dto)
  {
    var result = await truckService.UpdateOdometerAsync(truckId, dto.OdometerReading);
    return Ok(result);
  }

  /// <summary>
  /// Update truck status
  /// </summary>
  [HttpPatch("{truckId}/status")]
  [Authorize]
  public async Task<ActionResult<bool>> UpdateStatus(
      [FromRoute] Guid truckId,
      [FromBody] UpdateStatusDto dto)
  {
    var result = await truckService.UpdateStatusAsync(truckId, dto.Status);
    return Ok(result);
  }

  /// <summary>
  /// Update truck maintenance date
  /// </summary>
  [HttpPatch("{truckId}/maintenance")]
  [Authorize]
  public async Task<ActionResult<bool>> UpdateMaintenanceDate(
      [FromRoute] Guid truckId,
      [FromBody] UpdateMaintenanceDateDto dto)
  {
    var result = await truckService.UpdateMaintenanceDateAsync(truckId, dto.MaintenanceDate);
    return Ok(result);
  }
}
