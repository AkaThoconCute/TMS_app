namespace back_end_for_TMS.Common
{
  public class AppError(int status, int code, string message)
  {
    public int Status { get; } = status;
    public int Code { get; } = code;
    public string Message { get; } = message;
  }
}
