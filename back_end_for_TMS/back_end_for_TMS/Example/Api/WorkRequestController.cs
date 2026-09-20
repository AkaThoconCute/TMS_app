using back_end_for_TMS.Common;
using back_end_for_TMS.Example.IBusiness;
using Microsoft.AspNetCore.Mvc;

namespace back_end_for_TMS.Example.Api
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
    public async Task<AppResult<bool>> StartWork(CancellationToken cancellationToken)
    {
      var result = await _workRequestService.StartWorkAsync(cancellationToken);
      return result;
    }
  }
}
