using AutoMapper;
using DogHouse.Api.DTOs;
using DogHouse.Api.Models;

namespace DogHouse.Api.Mapping
{
    public class DogProfile : Profile
    {
        public DogProfile()
        {
            CreateMap<Dog, DogDto>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color != null ? src.Color.Replace(" ", "") : null));

            CreateMap<CreateDogDto, Dog>()
                .ForMember(dest => dest.TailLength, opt => opt.MapFrom(src => src.TailLength ?? 0))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight ?? 0));
        }
    }
}