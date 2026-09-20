namespace back_end_for_TMS.Common.Error
{
  public class AuthError
  {
    public static readonly AppError LoginFailed = new(
      StatusCodes.Status401Unauthorized,
      22001,
      "Login failed. Please check your username and password"
    );
  }
}
