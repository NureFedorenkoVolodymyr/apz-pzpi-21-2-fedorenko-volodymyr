using System.IdentityModel.Tokens.Jwt;
using WindSync.BLL.Dtos;
using WindSync.Core.Models;

namespace WindSync.BLL.Services.Auth;

public interface IAuthService
{
    Task<JwtSecurityToken?> LoginAsync(LoginDto model);
    Task<bool> RegisterAsync(RegisterDto model);
    Task<bool> RegisterAdminAsync(RegisterDto model);
    Task<User> GetUserByUsernameAsync(string username);
    Task<IList<User>> GetAdmins();
    Task<IList<User>> GetUsers();
    Task DeleteUser(string id);
}
