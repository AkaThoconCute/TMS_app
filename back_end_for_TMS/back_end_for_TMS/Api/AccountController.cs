using back_end_for_TMS.Business;
using back_end_for_TMS.Business.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end_for_TMS.Api;

[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController(AccountService accountService) : ControllerBase
{
  [HttpGet]
  [AllowAnonymous]
  public IActionResult GetInfo()
  {
    return Ok(new { message = "This is public data" });
  }

  [HttpPost]
  [AllowAnonymous]
  public async Task<ActionResult<AuthResult>> RefreshToken([FromBody] TokenDto dto)
  {
    var result = await accountService.RefreshToken(dto);
    return Ok(result);
  }

  [HttpPost]
  [AllowAnonymous]
  public async Task<ActionResult<AuthResult>> Register([FromBody] RegisterDto dto)
  {
    var result = await accountService.Register(dto);
    return Ok(result);
  }

  [HttpPost]
  [AllowAnonymous]
  public async Task<ActionResult<AuthResult>> Login([FromBody] LoginDto dto)
  {
    var result = await accountService.Login(dto);
    return Ok(result);
  }

  [HttpPost]
  [Authorize]
  public IActionResult Logout()
  {
    return Ok(new { message = "Logged out successfully" });
  }

  [HttpGet]
  [Authorize]
  public async Task<ActionResult<UserProfile>> GetMe()
  {
    var profile = await accountService.GetProfile(User);
    return Ok(profile);
  }

  [HttpPut]
  [Authorize]
  public async Task<ActionResult<UserProfile>> UpdateProfile([FromBody] UpdateProfileDto dto)
  {
    var profile = await accountService.UpdateProfileAsync(User, dto);
    return Ok(profile);
  }

  [HttpPost]
  [Authorize]
  public async Task<ActionResult<AuthResult>> ChangePassword([FromBody] ChangePasswordDto dto)
  {
    var result = await accountService.ChangePasswordAsync(User, dto);
    return Ok(result);
  }

  [HttpPost]
  [AllowAnonymous]
  public async Task<ActionResult<ForgotPasswordResult>> ForgotPassword([FromBody] ForgotPasswordDto dto)
  {
    var result = await accountService.ForgotPasswordAsync(dto);
    return Ok(result);
  }

  [HttpPost]
  [AllowAnonymous]
  public async Task<ActionResult<AuthResult>> ResetPassword([FromBody] ResetPasswordDto dto)
  {
    var result = await accountService.ResetPasswordAsync(dto);
    return Ok(result);
  }
}
