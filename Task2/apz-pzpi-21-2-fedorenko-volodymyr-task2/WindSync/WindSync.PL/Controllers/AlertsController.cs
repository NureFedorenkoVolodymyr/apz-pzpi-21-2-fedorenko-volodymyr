using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WindSync.BLL.Services.AlertService;
using WindSync.PL.ViewModels.Alert;

namespace WindSync.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertsController : ControllerBase
    {
        private readonly IAlertService _alertService;
        private readonly IMapper _mapper;

        public AlertsController(IAlertService alertService, IMapper mapper)
        {
            _alertService = alertService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<AlertViewModel>>> GetAlertsByUserAsync()
        {
            var userId = HttpContext.Items["UserId"]?.ToString();
            var alertsDto = await _alertService.GetAlertsByUserAsync(userId);
            var alertsViewModel = _mapper.Map<List<AlertViewModel>>(alertsDto);
            return Ok(alertsViewModel);
        }
    }
}
