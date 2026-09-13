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
        var quick = false;
        var loop = quick ? 1 : 30;
        for (int i = 0; i < loop; i++)
        {
          await Task.Delay(1000, cancellationToken);
        }

        var success = true;
        if (success == true)
        {
          return true;
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
