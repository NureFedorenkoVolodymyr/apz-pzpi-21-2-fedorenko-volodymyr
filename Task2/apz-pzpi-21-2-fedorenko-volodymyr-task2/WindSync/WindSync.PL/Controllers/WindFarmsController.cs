using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WindSync.BLL.Dtos;
using WindSync.BLL.Services;
using WindSync.PL.ViewModels;

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
        [HttpGet]
        public async Task<ActionResult<List<WindFarmViewModel>>> GetFarmsByUserAsync()
        {
            var userId = HttpContext.Items["UserId"]?.ToString();

            var farmsDto = await _windFarmService.GetFarmsByUserAsync(userId);
            var farmsViewModel = _mapper.Map<List<WindFarmViewModel>>(farmsDto);

            return Ok(farmsViewModel);
        }

        [HttpGet("{farmId}")]
        public async Task<ActionResult<WindFarmViewModel>> GetFarmByIdAsync([FromRoute] int farmId)
        {
            var farmDto = await _windFarmService.GetFarmByIdAsync(farmId);
            if (farmDto is null)
                return NotFound();

            var farmViewModel = _mapper.Map<WindFarmViewModel>(farmDto);
            return Ok(farmViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddFarmAsync([FromBody] WindFarmViewModel farmViewModel)
        {
            var userId = HttpContext.Items["UserId"]?.ToString();

            var farmDto = _mapper.Map<WindFarmDto>(farmViewModel);
            farmDto.UserId = userId;

            var result = await _windFarmService.AddFarmAsync(farmDto);
            if (!result)
                return BadRequest();

            return Ok();
        }

        [HttpPut("{farmId}")]
        public async Task<ActionResult> UpdateFarmAsync([FromRoute] int farmId, [FromBody] WindFarmViewModel farmViewModel)
        {
            var farmDto = _mapper.Map<WindFarmDto>(farmViewModel);

            var result = await _windFarmService.UpdateFarmAsync(farmDto);
            if (!result)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{farmId}")]
        public async Task<ActionResult> DeleteFarmAsync([FromRoute] int farmId)
        {
            var result = await _windFarmService.DeleteFarmAsync(farmId);
            if (!result)
                return BadRequest();

            return Ok();
        }

        [HttpGet("{farmId}/turbines")]
        public async Task<ActionResult<List<TurbineViewModel>>> GetTurbinesByFarmAsync(int farmId)
        {
            var turbinesDto = await _windFarmService.GetTurbinesByFarmAsync(farmId);
            var turbinesViewModel = _mapper.Map<List<TurbineViewModel>>(turbinesDto);

            return Ok(turbinesViewModel);
        }
    }
}
