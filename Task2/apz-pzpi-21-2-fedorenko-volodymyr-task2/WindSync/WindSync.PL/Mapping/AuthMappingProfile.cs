using AutoMapper;
using WindSync.BLL.Dtos;
using WindSync.PL.ViewModels;

namespace WindSync.PL.Mapping;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<LoginViewModel, LoginDto>();
        CreateMap<RegisterViewModel, RegisterDto>();
    }
}
