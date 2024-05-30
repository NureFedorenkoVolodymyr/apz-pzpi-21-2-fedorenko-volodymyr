using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WindSync.BLL.Dtos;
using WindSync.BLL.Services.WindFarmService;
using WindSync.PL.ViewModels.Turbine;
using WindSync.PL.ViewModels.WindFarm;

namespace WindSync.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WindFarmsController : ControllerBase
    {
        private readonly IWindFarmService _windFarmService;
        private readonly IMapper _mapper;

        public WindFarmsController(IWindFarmService windFarmService, IMapper mapper)
        {
            _windFarmService = windFarmService;
            _mapper = mapper;
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<WindFarmReadViewModel>>> GetFarmsAsync()
        {
            var farmsDto = await _windFarmService.GetFarmsAsync();
            var farmsViewModel = _mapper.Map<List<WindFarmReadViewModel>>(farmsDto);

            return Ok(farmsViewModel);
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<ActionResult<List<WindFarmReadViewModel>>> GetFarmsByUserAsync()
        {
            var userId = HttpContext.Items["UserId"]?.ToString();

            var farmsDto = await _windFarmService.GetFarmsByUserAsync(userId);
            var farmsViewModel = _mapper.Map<List<WindFarmReadViewModel>>(farmsDto);

            return Ok(farmsViewModel);
        }

        [Authorize]
        [HttpGet("{farmId}", Name = nameof(GetFarmByIdAsync))]
        public async Task<ActionResult<WindFarmReadViewModel>> GetFarmByIdAsync([FromRoute] int farmId)
        {
            var farmDto = await _windFarmService.GetFarmByIdAsync(farmId);
            if (farmDto is null)
                return NotFound();

            var farmViewModel = _mapper.Map<WindFarmReadViewModel>(farmDto);
            return Ok(farmViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddFarmAsync([FromBody] WindFarmAddViewModel farmViewModel)
        {
            var userId = HttpContext.Items["UserId"]?.ToString();

            var farmDto = _mapper.Map<WindFarmDto>(farmViewModel);
            farmDto.UserId = userId;

            var farmId = await _windFarmService.AddFarmAsync(farmDto);
            if (farmId == 0)
                return BadRequest();

            farmDto = await _windFarmService.GetFarmByIdAsync(farmId);
            var farmReadViewModel = _mapper.Map<WindFarmReadViewModel>(farmDto);

            return CreatedAtRoute(nameof(GetFarmByIdAsync), new { farmId }, farmViewModel);
        }

        [Authorize]
        [HttpPut("{farmId}")]
        public async Task<ActionResult> UpdateFarmAsync([FromRoute] int farmId, [FromBody] WindFarmReadViewModel farmViewModel)
        {
            var userId = HttpContext.Items["UserId"]?.ToString();
            if(userId != farmViewModel.UserId)
                return Forbid();

            var farmDto = _mapper.Map<WindFarmDto>(farmViewModel);
            farmDto.Id = farmId;

            var result = await _windFarmService.UpdateFarmAsync(farmDto);
            if (!result)
                return BadRequest();

            return Ok();
        }

        [Authorize]
        [HttpDelete("{farmId}")]
        public async Task<ActionResult> DeleteFarmAsync([FromRoute] int farmId)
        {
            var userId = HttpContext.Items["UserId"]?.ToString();
            var roles = HttpContext.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);
            var isAdmin = roles.Contains("Admin");

            var farm = await _windFarmService.GetFarmByIdAsync(farmId);
            if (userId != farm.UserId && !isAdmin)
                return Forbid();

            var result = await _windFarmService.DeleteFarmAsync(farmId);
            if (!result)
                return BadRequest();

            return Ok();
        }

        [Authorize]
        [HttpGet("{farmId}/turbines")]
        public async Task<ActionResult<List<TurbineReadViewModel>>> GetTurbinesByFarmAsync(int farmId)
        {
            var turbinesDto = await _windFarmService.GetTurbinesByFarmAsync(farmId);
            var turbinesViewModel = _mapper.Map<List<TurbineReadViewModel>>(turbinesDto);

            return Ok(turbinesViewModel);
        }
    }
}
