using AutoMapper;
using NETCoreAngular.API.DTOs;
using NETCoreAngular.API.Entities;
using NETCoreAngular.API.Extensions;

namespace NETCoreAngular.API.Heplers;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.PhotoUrl,
                opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.Age,
                opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
        CreateMap<Photo, PhotoDto>();
    }
}