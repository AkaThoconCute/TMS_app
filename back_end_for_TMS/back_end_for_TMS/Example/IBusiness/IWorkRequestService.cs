using back_end_for_TMS.Common;

namespace back_end_for_TMS.Example.IBusiness
{
  public interface IWorkRequestService
  {
    Task<AppResult<bool>> StartWorkAsync(CancellationToken cancellationToken);
  }
}
