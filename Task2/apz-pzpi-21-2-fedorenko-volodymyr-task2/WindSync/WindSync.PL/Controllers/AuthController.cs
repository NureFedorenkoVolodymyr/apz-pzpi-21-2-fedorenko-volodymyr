using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WindSync.BLL.Dtos;
using WindSync.BLL.Services.Auth;
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
        [Route("register-admin")]
        public async Task<ActionResult> RegisterAdmin([FromBody] RegisterViewModel model)
        {
            var registerDto = _mapper.Map<RegisterDto>(model);
            var registerResult = await _authService.RegisterAsync(registerDto);

            if (!registerResult)
                return BadRequest();

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
