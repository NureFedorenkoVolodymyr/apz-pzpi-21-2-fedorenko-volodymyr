using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WindSync.BLL.Dtos;
using WindSync.BLL.Utils;
using WindSync.Core.Enums;
using WindSync.Core.Models;

namespace WindSync.BLL.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JwtConfiguration _configuration;

    public AuthService(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            JwtConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<JwtSecurityToken?> LoginAsync(LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            return GetToken(authClaims);
        }

        return null;
    }

    public async Task<bool> RegisterAsync(RegisterDto model)
    {
        var userExists = await _userManager.FindByNameAsync(model.Username);
        if (userExists is not null)
            return false;

        User user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return false;

        if (!await _roleManager.RoleExistsAsync(UserRoles.User.ToString()))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.User.ToString()));

        if (await _roleManager.RoleExistsAsync(UserRoles.User.ToString()))
        {
            await _userManager.AddToRoleAsync(user, UserRoles.User.ToString());
        }

        return true;
    }

    public async Task<bool> RegisterAdminAsync(RegisterDto model)
    {
        var userExists = await _userManager.FindByNameAsync(model.Username);
        if (userExists is not null)
            return false;

        User user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return false;

        if (!await _roleManager.RoleExistsAsync(UserRoles.Admin.ToString()))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin.ToString()));
        if (!await _roleManager.RoleExistsAsync(UserRoles.User.ToString()))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.User.ToString()));

        if (await _roleManager.RoleExistsAsync(UserRoles.Admin.ToString()))
        {
            await _userManager.AddToRoleAsync(user, UserRoles.Admin.ToString());
        }
        if (await _roleManager.RoleExistsAsync(UserRoles.User.ToString()))
        {
            await _userManager.AddToRoleAsync(user, UserRoles.User.ToString());
        }

        return true;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }

    public async Task<IList<User>> GetAdmins()
    {
        return await _userManager.GetUsersInRoleAsync("Admin");
    }

    public async Task<IList<User>> GetUsers()
    {
        return await _userManager.GetUsersInRoleAsync("User");
    }

    public async Task DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        await _userManager.DeleteAsync(user);
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));

        var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }
}
