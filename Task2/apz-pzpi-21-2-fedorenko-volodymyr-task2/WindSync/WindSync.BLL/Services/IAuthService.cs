using System.IdentityModel.Tokens.Jwt;
using WindSync.BLL.Dtos;

namespace WindSync.BLL.Services;

public interface IAuthService
{
    Task<JwtSecurityToken?> Login(LoginDto model);
    Task<bool> Register(RegisterDto model);
    Task<bool> RegisterAdmin(RegisterDto model);
}
