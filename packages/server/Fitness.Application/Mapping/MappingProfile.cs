using AutoMapper;
using Fitness.Application.DTOs.Auth;
using Fitness.Application.DTOs.Exercise;
using Fitness.Application.DTOs.User;
using Fitness.Application.DTOs.WorkoutTemplate;
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

            CreateMap<CreateWorkoutTemplateDto, WorkoutTemplate>();
            CreateMap<UpdateWorkoutTemplateDto, WorkoutTemplate>();
            CreateMap<WorkoutTemplate, WorkoutTemplateResponseDto>();
            CreateMap<WorkoutTemplate, WorkoutTemplateWithExercisesDto>();

            CreateMap<Exercise, ExerciseInWorkoutTemplateDto>();
        }
    }
}
