using AutoMapper;
using WindSync.BLL.Dtos;
using WindSync.Core.Models;
using WindSync.PL.ViewModels;

namespace WindSync.PL.Mapping;

public class WindFarmMappingProfile : Profile
{
    public WindFarmMappingProfile()
    {
        CreateMap<WindFarm, WindFarmDto>().ReverseMap();
        CreateMap<WindFarmDto, WindFarmViewModel>().ReverseMap();
    }
}
