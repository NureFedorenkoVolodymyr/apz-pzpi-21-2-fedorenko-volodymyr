using AutoMapper;
using WindSync.BLL.Dtos;
using WindSync.Core.Models;
using WindSync.PL.ViewModels.WindFarm;

namespace WindSync.PL.Mapping;

public class WindFarmMappingProfile : Profile
{
    public WindFarmMappingProfile()
    {
        CreateMap<WindFarmAddViewModel, WindFarmDto>();
        CreateMap<WindFarmDto, WindFarmReadViewModel>().ReverseMap();
        CreateMap<WindFarm, WindFarmDto>().ReverseMap();
    }
}
