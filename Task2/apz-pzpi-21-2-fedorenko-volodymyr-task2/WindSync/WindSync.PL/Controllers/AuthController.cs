using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WindSync.BLL.Dtos;
using WindSync.BLL.Services.Auth;
using WindSync.Core.Models;
using WindSync.PL.ViewModels.Auth;

namespace WindSync.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginViewModel model)
        {
            var loginDto = _mapper.Map<LoginDto>(model);
            var token = await _authService.LoginAsync(loginDto);

            if (token is null)
                return BadRequest();

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] RegisterViewModel model)
        {
            var registerDto = _mapper.Map<RegisterDto>(model);
            var registerResult = await _authService.RegisterAsync(registerDto);

            if(!registerResult)
                return BadRequest();

            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Authorize(Roles = "Admin")]
        [Route("register-admin")]
        public async Task<ActionResult> RegisterAdmin([FromBody] RegisterViewModel model)
        {
            var registerDto = _mapper.Map<RegisterDto>(model);
            var registerResult = await _authService.RegisterAdminAsync(registerDto);

            if (!registerResult)
                return BadRequest();

            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetUserInfo()
        {
            var roles = HttpContext.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            var username = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            return Ok(new {
                username = username,
                roles = roles
            });
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("admins")]
        public async Task<ActionResult<IList<User>>> GetAdmins()
        {
            var admins = await _authService.GetAdmins();
            return Ok(admins);
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("users")]
        public async Task<ActionResult<IList<User>>> GetUsers()
        {
            var admins = await _authService.GetUsers();
            return Ok(admins);
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("users/{userId}")]
        public async Task<ActionResult<IList<User>>> DeleteUser(string userId)
        {
            await _authService.DeleteUser(userId);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("test")]
        public async Task<ActionResult> Test()
        {
            return Ok();
        }
    }
}
