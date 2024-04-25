using AutoMapper;
using WindSync.BLL.Dtos;
using WindSync.Core.Models;
using WindSync.DAL.Repositories.AlertRepository;

namespace WindSync.BLL.Services.AlertService;

public class AlertService : IAlertService
{
    private readonly IAlertRepository _repository;
    private readonly IMapper _mapper;

    public AlertService(IAlertRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<AlertDto>> GetAlertsByUserAsync(string userId)
    {
        var alerts = await _repository.GetAlertsByUserAsync(userId);
        return _mapper.Map<List<AlertDto>>(alerts);
    }

    public async Task<bool> AddAlertAsync(AlertDto alert)
    {
        var model = _mapper.Map<Alert>(alert);
        return await _repository.AddAlertAsync(model);
    }

    public async Task<bool> DeleteAlertAsync(int alertId)
    {
        return await _repository.DeleteAlertAsync(alertId);
    }
}
