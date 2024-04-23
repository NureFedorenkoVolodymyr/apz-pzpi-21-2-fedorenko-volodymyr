using AutoMapper;
using WindSync.BLL.Dtos;
using WindSync.Core.Models;
using WindSync.DAL.Repositories;

namespace WindSync.BLL.Services;

public class TurbineDataService : ITurbineDataService
{
    private readonly ITurbineDataRepository _repository;
    private readonly IMapper _mapper;

    public TurbineDataService(ITurbineDataRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<bool> AddDataAsync(TurbineDataDto data)
    {
        var model = _mapper.Map<TurbineData>(data);
        return await _repository.AddDataAsync(model);
    }

    public async Task<bool> DeleteDataAsync(int dataId)
    {
        return await _repository.DeleteDataAsync(dataId);
    }
}
