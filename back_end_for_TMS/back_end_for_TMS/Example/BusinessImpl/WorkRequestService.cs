using back_end_for_TMS.Common;
using back_end_for_TMS.Common.Error;
using back_end_for_TMS.Example.IBusiness;
using System.Collections.Concurrent;

namespace back_end_for_TMS.Example.BusinessImpl
{
  public class WorkRequestService : IWorkRequestService
  {
    // Thread-safe dictionary to store active cancellation tokens by requestId
    private readonly ConcurrentDictionary<string, CancellationTokenSource> _ctsMap = new();

    // Simulate 30 seconds of work.
    // Task.Delay natively listens to cancellationToken and throws TaskCanceledException when aborted.
    public async Task<AppResult<bool>> StartWorkAsync(CancellationToken cancellationToken)
    {
      try
      {
        var quick = true;
        var loop = quick ? 1 : 30;
        for (int i = 0; i < loop; i++)
        {
          await Task.Delay(1000, cancellationToken);
        }

        var success = 2;
        if (success == 1)
        {
          return AppResult<bool>.FromResult(true);
        }
        else if (success == 2)
        {
          return AppResult<bool>.FromError(ExampleError.NormalError);
        }
        else
        {
          throw new ArgumentException("Work done. Test an exception");
        }
      }
      catch (Exception ex) when (ex is not OperationCanceledException)
      {
        throw;
      }
    }
  }
}
