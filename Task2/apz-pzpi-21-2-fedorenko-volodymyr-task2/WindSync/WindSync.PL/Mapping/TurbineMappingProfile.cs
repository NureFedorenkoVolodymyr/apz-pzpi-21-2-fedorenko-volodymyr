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
        CreateMap<TurbineDto, Turbine>()
                .ForMember(dest => dest.SweptArea, opt => opt.MapFrom<SweptAreaResolver>());
        CreateMap<Turbine, TurbineDto>();
    }
}

public class SweptAreaResolver : IValueResolver<TurbineDto, Turbine, double>
{
    public double Resolve(TurbineDto source, Turbine destination, double destMember, ResolutionContext context)
    {
        double sweptArea = Math.Pow(source.TurbineRadius, 2) * Math.PI;
        return sweptArea;
    }
}
