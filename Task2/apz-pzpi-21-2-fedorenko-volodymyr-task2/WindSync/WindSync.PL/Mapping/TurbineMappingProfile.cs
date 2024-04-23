using AutoMapper;
using WindSync.BLL.Dtos;
using WindSync.Core.Models;

namespace WindSync.PL.Mapping;

public class TurbineMappingProfile : Profile
{
    public TurbineMappingProfile()
    {
        CreateMap<Turbine, TurbineDto>().ReverseMap();
    }
}
