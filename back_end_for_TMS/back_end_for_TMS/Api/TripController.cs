using back_end_for_TMS.Business;
using back_end_for_TMS.Business.Types;
using back_end_for_TMS.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end_for_TMS.Api;

[ApiController]
[Route("api/[controller]/[action]")]
public class TripController(TripService tripService) : ControllerBase
{
  /// <summary>
  /// Create a new trip (assign truck + driver to an order)
  /// </summary>
  [HttpPost]
  [Authorize]
  public async Task<ActionResult<TripDto>> Create([FromBody] CreateTripDto dto)
  {
    var result = await tripService.CreateAsync(dto);
    return CreatedAtAction(nameof(GetById), new { tripId = result.TripId }, result);
  }

  /// <summary>
  /// Get trip by ID
  /// </summary>
  [HttpGet("{tripId}")]
  [Authorize]
  public async Task<ActionResult<TripDto>> GetById([FromRoute] Guid tripId)
  {
    var result = await tripService.GetByIdAsync(tripId);
    return Ok(result);
  }

  /// <summary>
  /// List trips with pagination and filtering
  /// </summary>
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<PaginatedResult<TripDto>>> List(
      [FromQuery] int pageNumber = 1,
      [FromQuery] int pageSize = 10,
      [FromQuery] Guid? orderId = null,
      [FromQuery] int? status = null)
  {
    var result = await tripService.ListAsync(pageNumber, pageSize, orderId, status);
    return Ok(result);
  }

  /// <summary>
  /// Update trip information (partial update)
  /// </summary>
  [HttpPut("{tripId}")]
  [Authorize]
  public async Task<ActionResult<TripDto>> Update(
      [FromRoute] Guid tripId,
      [FromBody] UpdateTripDto dto)
  {
    var result = await tripService.UpdateAsync(tripId, dto);
    return Ok(result);
  }

  /// <summary>
  /// Start a trip (Planned → InTransit)
  /// </summary>
  [HttpPatch("{tripId}")]
  [Authorize]
  public async Task<ActionResult<TripDto>> Start(
      [FromRoute] Guid tripId,
      [FromBody] StartTripDto dto)
  {
    var result = await tripService.StartAsync(tripId, dto);
    return Ok(result);
  }

  /// <summary>
  /// Complete a trip (InTransit → Completed)
  /// </summary>
  [HttpPatch]
  [Authorize]
  public async Task<ActionResult<TripDto>> Complete([FromBody] CompleteTripDto dto)
  {
    var result = await tripService.CompleteAsync(dto);
    return Ok(result);
  }

  /// <summary>
  /// Cancel a trip (with reason)
  /// </summary>
  [HttpPatch]
  [Authorize]
  public async Task<ActionResult<TripDto>> Cancel([FromBody] CancelTripDto dto)
  {
    var result = await tripService.CancelAsync(dto);
    return Ok(result);
  }
}
