using back_end_for_TMS.Business.Types;
using back_end_for_TMS.Common;
using back_end_for_TMS.Common.Error;
using back_end_for_TMS.Models;
using back_end_for_TMS.Models.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace back_end_for_TMS.Business;

public class AccountService(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    TokenService tokenService,
    TenantRepo tenantRepo,
    ILogger<AccountService> logger)
{
  public async Task<AppResult<AuthDTO>> Login(LoginDto dto)
  {
    var user = await userManager.FindByEmailAsync(dto.Email);
    if (user == null)
    {
      return AppResult<AuthDTO>.FromError(AuthError.LoginFailed);
    }

    var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
    if (!result.Succeeded)
    {
      return AppResult<AuthDTO>.FromError(AuthError.LoginFailed);
    }

    var roles = await userManager.GetRolesAsync(user);
    var accessToken = tokenService.CreateToken(user, roles);
    var refreshToken = tokenService.GenerateRefreshToken();

    user.RefreshToken = refreshToken;
    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
    await userManager.UpdateAsync(user);

    return AppResult<AuthDTO>.FromResult(new AuthDTO
    {
      Success = true,
      AccessToken = accessToken,
      RefreshToken = refreshToken
    });
  }
  public async Task<AuthDTO> RefreshToken(TokenDto dto)
  {
    var principal = tokenService.GetPrincipalFromExpiredToken(dto.Token);
    var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(userId))
      throw new InvalidOperationException("NameIdentifier is empty in ClaimsPrincipal");

    var user = await userManager.FindByIdAsync(userId);
    if (user == null)
      throw new KeyNotFoundException("User not found by ID");

    if (user.RefreshToken != dto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
      throw new SecurityTokenException("Invalid refresh token");

    var roles = await userManager.GetRolesAsync(user);
    var newAccessToken = tokenService.CreateToken(user, roles);
    var newRefreshToken = tokenService.GenerateRefreshToken();

    user.RefreshToken = newRefreshToken;
    await userManager.UpdateAsync(user);

    return new AuthDTO
    {
      Success = true,
      AccessToken = newAccessToken,
      RefreshToken = newRefreshToken
    };
  }

  public async Task<AuthDTO> Register(RegisterDto dto)
  {
    var user = new AppUser { UserName = dto.Email, Email = dto.Email };

    var result = await userManager.CreateAsync(user, dto.Password);
    if (!result.Succeeded)
      return new AuthDTO { Success = false, Errors = [.. result.Errors.Select(e => e.Description)] };

    await userManager.AddToRoleAsync(user, "User");

    // Tạo Tenant cho user mới
    var tenant = new Tenant
    {
      Name = GenerateTenantName(dto.Email),
      OwnerId = user.Id
    };
    tenantRepo.Add(tenant);
    await tenantRepo.SaveChangesAsync();

    user.TenantId = tenant.TenantId;
    await userManager.UpdateAsync(user);

    var roles = await userManager.GetRolesAsync(user);
    var accessToken = tokenService.CreateToken(user, roles);
    var refreshToken = tokenService.GenerateRefreshToken();

    user.RefreshToken = refreshToken;
    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
    await userManager.UpdateAsync(user);

    return new AuthDTO
    {
      Success = true,
      AccessToken = accessToken,
      RefreshToken = refreshToken
    };
  }

  public async Task<UserProfile?> GetProfile(ClaimsPrincipal currentUser)
  {
    var email = currentUser.FindFirstValue(ClaimTypes.Email);
    if (string.IsNullOrEmpty(email))
      throw new InvalidOperationException("Email is empty in ClaimsPrincipal");

    var user = await userManager.FindByEmailAsync(email);
    if (user == null)
      throw new KeyNotFoundException("User not found by email");

    var roles = await userManager.GetRolesAsync(user);

    string? tenantName = null;
    if (user.TenantId is Guid tenantId && tenantId != Guid.Empty)
    {
      var tenant = await tenantRepo.FindAsync(t => t.TenantId == tenantId);
      tenantName = tenant?.Name;
    }

    return new UserProfile
    {
      Email = user.Email ?? string.Empty,
      UserName = user.UserName ?? string.Empty,
      Roles = [.. roles],
      TenantId = user.TenantId,
      TenantName = tenantName
    };
  }

  public async Task<UserProfile> UpdateProfileAsync(ClaimsPrincipal currentUser, UpdateProfileDto dto)
  {
    var email = currentUser.FindFirstValue(ClaimTypes.Email);
    if (string.IsNullOrEmpty(email))
      throw new InvalidOperationException("Email is empty in ClaimsPrincipal");

    var user = await userManager.FindByEmailAsync(email);
    if (user == null)
      throw new KeyNotFoundException("User not found by email");

    var result = await userManager.SetUserNameAsync(user, dto.UserName);
    if (!result.Succeeded)
      throw new ArgumentException(string.Join("; ", result.Errors.Select(e => e.Description)));

    user = await userManager.FindByEmailAsync(email);
    if (user == null)
      throw new KeyNotFoundException("User not found after update");

    var roles = await userManager.GetRolesAsync(user);
    string? tenantName = null;
    if (user.TenantId is Guid tenantId && tenantId != Guid.Empty)
    {
      var tenant = await tenantRepo.FindAsync(t => t.TenantId == tenantId);
      tenantName = tenant?.Name;
    }

    return new UserProfile
    {
      Email = user.Email ?? string.Empty,
      UserName = user.UserName ?? string.Empty,
      Roles = [.. roles],
      TenantId = user.TenantId,
      TenantName = tenantName
    };
  }

  public async Task<AuthDTO> ChangePasswordAsync(ClaimsPrincipal currentUser, ChangePasswordDto dto)
  {
    var email = currentUser.FindFirstValue(ClaimTypes.Email);
    if (string.IsNullOrEmpty(email))
      throw new InvalidOperationException("Email is empty in ClaimsPrincipal");

    var user = await userManager.FindByEmailAsync(email);
    if (user == null)
      throw new KeyNotFoundException("User not found by email");

    var result = await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
    if (!result.Succeeded)
      return new AuthDTO { Success = false, Errors = [.. result.Errors.Select(e => e.Description)] };

    return new AuthDTO { Success = true };
  }

  public async Task<ForgotPasswordResult> ForgotPasswordAsync(ForgotPasswordDto dto)
  {
    var user = await userManager.FindByEmailAsync(dto.Email);
    if (user != null)
    {
      var token = await userManager.GeneratePasswordResetTokenAsync(user);
      logger.LogInformation("Password reset token for {Email}: {Token}", dto.Email, token);
    }

    return new ForgotPasswordResult
    {
      Success = true,
      Message = "If the email exists, a reset token has been sent."
    };
  }

  public async Task<AuthDTO> ResetPasswordAsync(ResetPasswordDto dto)
  {
    var user = await userManager.FindByEmailAsync(dto.Email);
    if (user == null)
      return new AuthDTO { Success = false, Errors = ["Invalid request"] };

    var result = await userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
    if (!result.Succeeded)
      return new AuthDTO { Success = false, Errors = [.. result.Errors.Select(e => e.Description)] };

    return new AuthDTO { Success = true };
  }

  private static string GenerateTenantName(string email)
  {
    var parts = email.Split('@');
    var username = parts[0];
    var domain = parts[1].ToLowerInvariant();

    string[] genericDomains = ["gmail.com", "outlook.com", "yahoo.com", "hotmail.com"];

    return genericDomains.Contains(domain)
        ? $"{username}'s Business"
        : domain;
  }
}
