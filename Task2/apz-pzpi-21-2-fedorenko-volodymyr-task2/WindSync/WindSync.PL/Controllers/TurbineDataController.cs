using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WindSync.BLL.Dtos;
using WindSync.BLL.Services.TurbineDataService;
using WindSync.Core.Enums;
using WindSync.PL.ViewModels.TurbineData;

namespace WindSync.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurbineDataController : ControllerBase
    {
        private readonly ITurbineDataService _turbineDataService;
        private readonly IMapper _mapper;

        public TurbineDataController(ITurbineDataService turbineDataService, IMapper mapper)
        {
            _turbineDataService = turbineDataService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<TurbineStatus>> AddDataAsync(TurbineDataAddViewModel viewModel)
        {
            var turbineDataDto = _mapper.Map<TurbineDataDto>(viewModel);

            var turbineStatus = await _turbineDataService.AddDataAsync(turbineDataDto);
            if (turbineStatus == TurbineStatus.None)
                return BadRequest();

            return Ok(turbineStatus);
        }

        [HttpDelete("{dataId}")]
        public async Task<ActionResult> DeleteDataAsync(int dataId)
        {
            var result = await _turbineDataService.DeleteDataAsync(dataId);
            if (!result)
                return BadRequest();

            return Ok();
        }
    }
}
