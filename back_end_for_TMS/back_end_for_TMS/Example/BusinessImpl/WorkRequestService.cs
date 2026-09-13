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
    public async Task<bool> StartWorkAsync(CancellationToken cancellationToken)
    {
      try
      {
        for (int i = 0; i < 30; i++)
        {
          await Task.Delay(1000, cancellationToken);
        }

        return true;
      }
      catch (Exception ex) when (ex is not OperationCanceledException)
      {
        return false;
      }
      catch (OperationCanceledException ex)
      {
        return false;
      }
    }
  }
}
