using back_end_for_TMS.Business;
using back_end_for_TMS.Business.Types;
using back_end_for_TMS.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end_for_TMS.Api;

[ApiController]
[Route("api/[controller]/[action]")]
public class OrderController(OrderService orderService) : ControllerBase
{
  /// <summary>
  /// Create a new order
  /// </summary>
  [HttpPost]
  [Authorize]
  public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderDto dto)
  {
    var result = await orderService.CreateAsync(dto);
    return CreatedAtAction(nameof(GetById), new { orderId = result.OrderId }, result);
  }

  /// <summary>
  /// Get order by ID
  /// </summary>
  [HttpGet("{orderId}")]
  [Authorize]
  public async Task<ActionResult<OrderDto>> GetById([FromRoute] Guid orderId)
  {
    var result = await orderService.GetByIdAsync(orderId);
    return Ok(result);
  }

  /// <summary>
  /// List orders with pagination and filtering
  /// </summary>
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<PaginatedResult<OrderDto>>> List(
      [FromQuery] int pageNumber = 1,
      [FromQuery] int pageSize = 10,
      [FromQuery] int? status = null,
      [FromQuery] Guid? customerId = null,
      [FromQuery] string? searchTerm = null)
  {
    var result = await orderService.ListAsync(pageNumber, pageSize, status, customerId, searchTerm);
    return Ok(result);
  }

  /// <summary>
  /// Get order status summary (count per status for pipeline display)
  /// </summary>
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<OrderStatusSummaryDto>> StatusSummary()
  {
    var result = await orderService.GetStatusSummaryAsync();
    return Ok(result);
  }

  /// <summary>
  /// Update order information (partial update)
  /// </summary>
  [HttpPut("{orderId}")]
  [Authorize]
  public async Task<ActionResult<OrderDto>> Update(
      [FromRoute] Guid orderId,
      [FromBody] UpdateOrderDto dto)
  {
    var result = await orderService.UpdateAsync(orderId, dto);
    return Ok(result);
  }

  /// <summary>
  /// Complete an order (Delivered → Completed)
  /// </summary>
  [HttpPatch("{orderId}")]
  [Authorize]
  public async Task<ActionResult<OrderDto>> Complete(
      [FromRoute] Guid orderId,
      [FromBody] CompleteOrderDto dto)
  {
    var result = await orderService.CompleteAsync(orderId, dto);
    return Ok(result);
  }

  /// <summary>
  /// Cancel an order (with reason, auto-cancels active trips)
  /// </summary>
  [HttpPatch]
  [Authorize]
  public async Task<ActionResult<OrderDto>> Cancel([FromBody] CancelOrderDto dto)
  {
    var result = await orderService.CancelAsync(dto);
    return Ok(result);
  }
}
