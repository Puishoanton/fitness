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

            CreateMap<CreateWorkoutTemplateDto, WorkoutTemplate>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow));
            CreateMap<UpdateWorkoutTemplateDto, WorkoutTemplate>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow));
            CreateMap<WorkoutTemplate, WorkoutTemplateResponseDto>();
            CreateMap<WorkoutTemplate, WorkoutTemplateWithExercisesDto>();

            CreateMap<CreateExerciseDto, Exercise>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow));
            CreateMap<UpdateExerciseDto, Exercise>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow));
            CreateMap<Exercise, ExerciseResponseDto>();
            CreateMap<Exercise, ExerciseLightDto>();
        }
    }
}
