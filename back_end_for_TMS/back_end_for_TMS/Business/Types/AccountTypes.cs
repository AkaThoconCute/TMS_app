namespace back_end_for_TMS.Business.Types;

public record TokenDto(string Token, string RefreshToken);

public record RegisterDto(string Email, string Password);

public record LoginDto(string Email, string Password);

public class AuthResult
{
  public bool Success { get; set; }
  public string? Token { get; set; }
  public string? RefreshToken { get; set; }
  public List<string>? Errors { get; set; }
}

public class UserProfile
{
  public string Email { get; set; } = string.Empty;
  public string UserName { get; set; } = string.Empty;
  public List<string> Roles { get; set; } = [];
  public Guid? TenantId { get; set; }
  public string? TenantName { get; set; }
}

public record UpdateProfileDto(string UserName);

public record ChangePasswordDto(string CurrentPassword, string NewPassword);

public record ForgotPasswordDto(string Email);

public record ResetPasswordDto(string Email, string Token, string NewPassword);

public class ForgotPasswordResult
{
  public bool Success { get; set; }
  public string Message { get; set; } = string.Empty;
}