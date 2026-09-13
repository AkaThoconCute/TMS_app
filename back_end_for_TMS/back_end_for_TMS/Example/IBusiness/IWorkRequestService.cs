namespace back_end_for_TMS.Example.IBusiness
{
  public interface IWorkRequestService
  {
    Task<bool> StartWorkAsync(CancellationToken cancellationToken);
  }
}
