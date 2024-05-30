using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WindSync.BLL.Dtos;
using WindSync.BLL.Services.TurbineService;
using WindSync.Core.Enums;
using WindSync.PL.ViewModels.Turbine;
using WindSync.PL.ViewModels.TurbineData;

namespace WindSync.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurbinesController : ControllerBase
    {
        private readonly ITurbineService _turbineService;
        private readonly IMapper _mapper;

        public TurbinesController(ITurbineService turbineService, IMapper mapper)
        {
            _turbineService = turbineService;
            _mapper = mapper;
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<TurbineReadViewModel>>> GetTurbinesAsync()
        {
            var turbinesDto = await _turbineService.GetTurbinesAsync();
            var turbinesViewModel = _mapper.Map<List<TurbineReadViewModel>>(turbinesDto);
            return Ok(turbinesViewModel);
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<ActionResult<List<TurbineReadViewModel>>> GetTurbinesByUserAsync()
        {
            var userId = HttpContext.Items["UserId"]?.ToString();
            var turbinesDto = await _turbineService.GetTurbinesByUserAsync(userId);
            var turbinesViewModel = _mapper.Map<List<TurbineReadViewModel>>(turbinesDto);
            return Ok(turbinesViewModel);
        }

        [Authorize]
        [HttpGet("{turbineId}", Name = nameof(GetTurbineByIdAsync))]
        public async Task<ActionResult<TurbineReadViewModel>> GetTurbineByIdAsync(int turbineId)
        {
            var turbineDto = await _turbineService.GetTurbineByIdAsync(turbineId);
            if (turbineDto is null)
                return NotFound();

            var turbineViewModel = _mapper.Map<TurbineReadViewModel>(turbineDto);
            return Ok(turbineViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddTurbineAsync(TurbineAddViewModel turbineViewModel)
        {
            var turbineDto = _mapper.Map<TurbineDto>(turbineViewModel);
            var turbineId = await _turbineService.AddTurbineAsync(turbineDto);
            if (turbineId == 0)
                return BadRequest();

            turbineDto = await _turbineService.GetTurbineByIdAsync(turbineId);
            var turbineReadViewModel = _mapper.Map<TurbineReadViewModel>(turbineDto);

            return CreatedAtRoute(nameof(GetTurbineByIdAsync), new { turbineId }, turbineReadViewModel);
        }

        [Authorize]
        [HttpPut("{turbineId}")]
        public async Task<ActionResult> UpdateTurbineAsync(int turbineId, TurbineAddViewModel turbineViewModel)
        {
            var turbineDto = _mapper.Map<TurbineDto>(turbineViewModel);
            turbineDto.Id = turbineId;

            var result = await _turbineService.UpdateTurbineAsync(turbineDto);
            if (!result)
                return BadRequest();

            return Ok();
        }

        [Authorize]
        [HttpDelete("{turbineId}")]
        public async Task<ActionResult> DeleteTurbineAsync(int turbineId)
        {
            var result = await _turbineService.DeleteTurbineAsync(turbineId);
            if (!result)
                return BadRequest();

            return Ok();
        }

        [Authorize]
        [HttpPost("{turbineId}/status")]
        public async Task<ActionResult> ChangeTurbineStatusAsync(int turbineId, [FromBody] TurbineStatus status)
        {
            var result = await _turbineService.ChangeTurbineStatusAsync(turbineId, status);
            if (!result)
                return BadRequest();

            return Ok();
        }

        [Authorize]
        [HttpGet("{turbineId}/data")]
        public async Task<ActionResult<List<TurbineDataReadViewModel>>> GetTurbineDataHistoricalAsync(int turbineId, [FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var turbineDataDto = await _turbineService.GetTurbineDataHistoricalAsync(turbineId, start, end);
            var turbineDataViewModel = _mapper.Map<List<TurbineDataReadViewModel>>(turbineDataDto);
            return Ok(turbineDataViewModel);
        }

        [Authorize]
        [HttpGet("{turbineId}/data/recent")]
        public async Task<ActionResult<TurbineDataReadViewModel>> GetMostRecentTurbineDataAsync(int turbineId)
        {
            var turbineDataDto = await _turbineService.GetMostRecentTurbineDataAsync(turbineId);
            if (turbineDataDto is null)
                return NotFound();

            var turbineDataViewModel = _mapper.Map<TurbineDataReadViewModel>(turbineDataDto);

            return Ok(turbineDataViewModel);
        }
    }
}
