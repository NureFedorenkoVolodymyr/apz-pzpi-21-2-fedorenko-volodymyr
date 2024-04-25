using AutoMapper;
using WindSync.BLL.Dtos;
using WindSync.Core.Models;
using WindSync.PL.ViewModels.TurbineData;

namespace WindSync.PL.Mapping;

public class TurbineDataMappingProfile : Profile
{
    public TurbineDataMappingProfile()
    {
        CreateMap<TurbineDataAddViewModel, TurbineDataDto>();
        CreateMap<TurbineDataDto, TurbineDataReadViewModel>();
        CreateMap<TurbineDataDto, TurbineData>().ReverseMap();
    }
}
