﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WindSync.BLL.Dtos;
using WindSync.BLL.Services;
using WindSync.PL.ViewModels;

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
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var loginDto = _mapper.Map<LoginDto>(model);
            var token = await _authService.Login(loginDto);

            if (token is null)
                return Unauthorized();

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var registerDto = _mapper.Map<RegisterDto>(model);
            var registerResult = await _authService.Register(registerDto);

            if(!registerResult)
                return BadRequest();

            return Ok();
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterViewModel model)
        {
            var registerDto = _mapper.Map<RegisterDto>(model);
            var registerResult = await _authService.Register(registerDto);

            if (!registerResult)
                return BadRequest();

            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}