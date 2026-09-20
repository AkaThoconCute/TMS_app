namespace back_end_for_TMS.Common
{
  public interface IAppResult
  {
    bool Success { get; }
    AppError? Error { get; }
  }

  public class AppResult<T> : IAppResult
  {
    public bool Success { get; }
    public T? Result { get; }
    public AppError? Error { get; }

    // Private constructor
    private AppResult(bool Success, T? Result, AppError? Error)
    {
      this.Success = Success;
      this.Result = Result;
      this.Error = Error;
    }

    // Factory methods
    public static AppResult<T> FromResult(T value) => new(true, value, default);
    public static AppResult<T> FromError(AppError error) => new(false, default, error);
  }
}
