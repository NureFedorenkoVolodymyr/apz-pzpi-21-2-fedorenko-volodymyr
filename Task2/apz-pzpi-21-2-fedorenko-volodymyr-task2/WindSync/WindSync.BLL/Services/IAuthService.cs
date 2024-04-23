using System.IdentityModel.Tokens.Jwt;
using WindSync.BLL.Dtos;

namespace WindSync.BLL.Services;

public interface IAuthService
{
    Task<JwtSecurityToken?> LoginAsync(LoginDto model);
    Task<bool> RegisterAsync(RegisterDto model);
    Task<bool> RegisterAdminAsync(RegisterDto model);
}
