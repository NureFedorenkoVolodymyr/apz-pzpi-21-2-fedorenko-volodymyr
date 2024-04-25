using AutoMapper;
using WindSync.BLL.Dtos;
using WindSync.Core.Models;
using WindSync.PL.ViewModels.Alert;
using WindSync.PL.ViewModels.TurbineData;

namespace WindSync.PL.Mapping;

public class AlertMappingProfile : Profile
{
    public AlertMappingProfile()
    {
        CreateMap<AlertViewModel, AlertDto>().ReverseMap();
        CreateMap<AlertDto, Alert>().ReverseMap();
    }
}
