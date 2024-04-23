using AutoMapper;
using WindSync.BLL.Dtos;
using WindSync.Core.Models;

namespace WindSync.PL.Mapping;

public class WindFarmMappingProfile : Profile
{
    public WindFarmMappingProfile()
    {
        CreateMap<WindFarm, WindFarmDto>().ReverseMap();
    }
}
