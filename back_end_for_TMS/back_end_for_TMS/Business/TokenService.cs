using back_end_for_TMS.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace back_end_for_TMS.Business;

public class TokenService(IConfiguration config)
{
  private const string TenantIdClaimType = "tenantId";

  public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
  {
    var tokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidIssuer = config["JWT:Issuer"],
      ValidateAudience = true,
      ValidAudience = config["JWT:Audience"],
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["JWT:SigningKey"] ?? throw new InvalidOperationException("JWT:SigningKey is missing"))
        ),
      ValidAlgorithms = [SecurityAlgorithms.HmacSha512],
      ValidateLifetime = false,
      ClockSkew = TimeSpan.Zero
    };

    var tokenHandler = new JwtSecurityTokenHandler();

    try
    {
      var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
      if (securityToken is not JwtSecurityToken jwtSecurityToken)
      {
        throw new SecurityTokenException("Invalid token");
      }

      return principal;
    }
    catch (Exception ex)
    {
      throw new SecurityTokenException("Invalid token", ex);
    }
  }

  public string GenerateRefreshToken()
  {
    var randomNumber = new byte[64];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }

  public string CreateToken(AppUser user, IList<string> roles)
  {
    // 1. Lấy thông tin từ cấu hình
    // Sử dụng GetValue để đảm bảo nếu thiếu key sẽ báo lỗi rõ ràng
    var signingKey = config.GetValue<string>("JWT:SigningKey")
        ?? throw new InvalidOperationException("Missing JWT.SigningKey in configuration");

    // Đảm bảo key đủ độ dài cho HS512 (512 bits = 64 bytes)
    var keyBytes = Encoding.UTF8.GetBytes(signingKey);
    if (keyBytes.Length < 64)
    {
      throw new InvalidOperationException("JWT.SigningKey must be at least 64 characters");
    }

    var key = new SymmetricSecurityKey(keyBytes);

    // 2. Tạo danh sách Claims (thông tin định danh)
    var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new(JwtRegisteredClaimNames.GivenName, user.UserName ?? ""),
            new(JwtRegisteredClaimNames.NameId, user.Id)
        };

    if (user.TenantId is Guid tenantId && tenantId != Guid.Empty)
    {
      claims.Add(new Claim(TenantIdClaimType, tenantId.ToString()));
      claims.Add(new Claim(ClaimTypes.GroupSid, tenantId.ToString()));
    }

    // Thêm các Role vào Claims
    foreach (var role in roles)
    {
      claims.Add(new Claim(ClaimTypes.Role, role));
    }

    // 3. Cấu hình nội dung Token
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.UtcNow.AddDays(7),
      SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature),
      Issuer = config["JWT:Issuer"],
      Audience = config["JWT:Audience"]
    };

    // 4. Tạo và trả về chuỗi Token
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);

    return tokenHandler.WriteToken(token);
  }
}
