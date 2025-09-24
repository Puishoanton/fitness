using AutoMapper;
using Fitness.Application.DTOs.Auth;
using Fitness.Application.DTOs.User;
using Fitness.Domain.Entities;

namespace Fitness.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow));

            CreateMap<User, UserPayloadDto>();
            CreateMap<User, AuthResponseDto>();
        }
    }
}
