using AutoMapper;
using WindSync.BLL.Dtos;
using WindSync.Core.Models;
using WindSync.PL.ViewModels.Turbine;

namespace WindSync.PL.Mapping;

public class TurbineMappingProfile : Profile
{
    public TurbineMappingProfile()
    {
        CreateMap<TurbineAddViewModel, TurbineDto>();
        CreateMap<TurbineDto, TurbineReadViewModel>();
        CreateMap<Turbine, TurbineDto>().ReverseMap();
    }
}
