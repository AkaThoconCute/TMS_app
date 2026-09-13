using back_end_for_TMS.Example.IBusiness;
using Microsoft.AspNetCore.Mvc;

namespace back_end_for_TMS.Example
{
  [ApiController]
  [Route("api/work-requests")]
  public class WorkRequestController : ControllerBase
  {
    private readonly IWorkRequestService _workRequestService;

    public WorkRequestController(IWorkRequestService workRequestService)
    {
      _workRequestService = workRequestService;
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartWork(CancellationToken cancellationToken)
    {
      var result = await _workRequestService.StartWorkAsync(cancellationToken);
      return Ok(new { Message = "Work completed successfully.", Result = result });
    }
  }
}
