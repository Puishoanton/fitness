using AutoMapper;
using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.WorkoutTemplate;
using Fitness.Application.Exceptions;
using Fitness.Application.Interfaces.Repositories;
using Fitness.Application.Services;
using Fitness.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Fitness.Tests.Services
{
    public class WorkoutTemplateServiceTests
    {
        [Fact]
        public async Task CreateWorkoutTemplateAsync_WhenCreateWorkoutTemplateDtoIsValid_ShouldCreateWorkoutTemplate()
        {
            const string workoutTemplateName = "workout_template_name";
            Guid userId = Guid.NewGuid();
            CreateWorkoutTemplateDto createWorkoutTemplateDto = new() { Name = workoutTemplateName };
            WorkoutTemplate workoutTemplate = new() { Name = workoutTemplateName };
            WorkoutTemplateResponseDto workoutTemplateResponseDto = new() { Name = workoutTemplateName };

            // Arrange
            Mock<IWorkoutTemplateRepository> workoutTemplateRepositoryMock = new();
            Mock<IMapper> mapperMock = new();


            mapperMock
                .Setup(m => m.Map<WorkoutTemplate>(It.IsAny<CreateWorkoutTemplateDto>()))
                .Returns(workoutTemplate);

            workoutTemplateRepositoryMock
                .Setup(r => r.CreateAsync(workoutTemplate))
                .ReturnsAsync(workoutTemplate);

            mapperMock
                .Setup(m => m.Map<WorkoutTemplateResponseDto>(It.IsAny<WorkoutTemplate>()))
                .Returns(workoutTemplateResponseDto);

            WorkoutTemplateService workoutTemplateService = new(
                workoutTemplateRepositoryMock.Object,
                mapperMock.Object
                );

            //Act
            WorkoutTemplateResponseDto result = await workoutTemplateService.CreateWorkoutTemplateAsync(createWorkoutTemplateDto, userId);

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(workoutTemplateName);

            mapperMock.Verify(m => m.Map<WorkoutTemplate>(It.IsAny<CreateWorkoutTemplateDto>()), Times.Once);
            workoutTemplateRepositoryMock.Verify(r => r.CreateAsync(It.Is<WorkoutTemplate>(wt => wt.UserId == userId)), Times.Once);
            mapperMock.Verify(m => m.Map<WorkoutTemplateResponseDto>(workoutTemplate), Times.Once);
        }

        [Fact]
        public async Task UpdateWorkoutTemplateAsync_WhenWorkoutTemplateIdIsValid_ShouldUpdateWorkoutTemplate()
        {
            const string newWorkoutTemplateName = "new_workout_template_name";
            Guid workoutTemplateId = Guid.NewGuid();
            UpdateWorkoutTemplateDto updateWorkoutTemplateDto = new() { Name = newWorkoutTemplateName };
            WorkoutTemplate workoutTemplate = new() { Id = workoutTemplateId, Name = newWorkoutTemplateName };
            WorkoutTemplateResponseDto workoutTemplateResponseDto = new() { Name = newWorkoutTemplateName };

            // Arrange
            Mock<IWorkoutTemplateRepository> workoutTemplateRepositoryMock = new();
            Mock<IMapper> mapperMock = new();

            workoutTemplateRepositoryMock
                .Setup(r => r.GetByIdAsync(workoutTemplateId))
                .ReturnsAsync(workoutTemplate);

            mapperMock
                .Setup(m => m.Map(updateWorkoutTemplateDto, workoutTemplate))
                .Returns(workoutTemplate);

            workoutTemplateRepositoryMock
                .Setup(r => r.UpdateAsync(workoutTemplate))
                .ReturnsAsync(workoutTemplate);

            mapperMock
                .Setup(m => m.Map<WorkoutTemplateResponseDto>(It.IsAny<WorkoutTemplate>()))
                .Returns(workoutTemplateResponseDto);

            WorkoutTemplateService workoutTemplateService = new(
                workoutTemplateRepositoryMock.Object,
                mapperMock.Object
                );

            //Act
            WorkoutTemplateResponseDto result = await workoutTemplateService.UpdateWorkoutTemplateAsync(updateWorkoutTemplateDto, workoutTemplateId);

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(newWorkoutTemplateName);

            workoutTemplateRepositoryMock.Verify(r => r.GetByIdAsync(workoutTemplateId), Times.Once);
            mapperMock.Verify(m => m.Map(updateWorkoutTemplateDto, workoutTemplate), Times.Once);
            workoutTemplateRepositoryMock.Verify(r => r.UpdateAsync(It.Is<WorkoutTemplate>(wt => wt.Id == workoutTemplateId)), Times.Once);
            mapperMock.Verify(m => m.Map<WorkoutTemplateResponseDto>(workoutTemplate), Times.Once);
        }

        [Fact]
        public async Task UpdateWorkoutTemplateAsync_WhenWorkoutTemplateIdIsNotValid_ShouldThrowWorkoutTemplateNotFoundException()
        {
            const string newWorkoutTemplateName = "new_workout_template_name";
            Guid workoutTemplateId = Guid.NewGuid();
            UpdateWorkoutTemplateDto updateWorkoutTemplateDto = new() { Name = newWorkoutTemplateName };
            WorkoutTemplate workoutTemplate = new() { Id = workoutTemplateId, Name = newWorkoutTemplateName };
            WorkoutTemplateResponseDto workoutTemplateResponseDto = new() { Name = newWorkoutTemplateName };

            // Arrange
            Mock<IWorkoutTemplateRepository> workoutTemplateRepositoryMock = new();
            Mock<IMapper> mapperMock = new();

            workoutTemplateRepositoryMock
                .Setup(r => r.GetByIdAsync(workoutTemplateId))
                .ReturnsAsync((WorkoutTemplate?)null);

            WorkoutTemplateService workoutTemplateService = new(
                workoutTemplateRepositoryMock.Object,
                mapperMock.Object
                );

            //Act
            Func<Task> act = async () => await workoutTemplateService.UpdateWorkoutTemplateAsync(updateWorkoutTemplateDto, workoutTemplateId);

            //Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(WorkoutTemplate)}: {workoutTemplateId} is not found.");

            workoutTemplateRepositoryMock.Verify(r => r.GetByIdAsync(workoutTemplateId), Times.Once);
            mapperMock.Verify(m => m.Map<WorkoutTemplate>(It.IsAny<UpdateWorkoutTemplateDto>()), Times.Never);
            workoutTemplateRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<WorkoutTemplate>()), Times.Never);
            mapperMock.Verify(m => m.Map<WorkoutTemplateResponseDto>(It.IsAny<WorkoutTemplate>()), Times.Never);
        }

        [Fact]
        public async Task DeleteWorkoutTemplateAsync_WhenWorkoutTemplateIdIsValid_ShouldDeleteWorkoutTemplate()
        {
            Guid workoutTemplateId = Guid.NewGuid();

            // Arrange
            Mock<IWorkoutTemplateRepository> workoutTemplateRepositoryMock = new();
            Mock<IMapper> mapperMock = new();

            workoutTemplateRepositoryMock
                .Setup(r => r.DeleteAsync(workoutTemplateId))
                .ReturnsAsync(true);

            WorkoutTemplateService workoutTemplateService = new(
                workoutTemplateRepositoryMock.Object,
                mapperMock.Object
                );

            //Act
            DeleteResponseMessageDto result = await workoutTemplateService.DeleteWorkoutTemplateAsync(workoutTemplateId);

            //Assert
            result.Should().NotBeNull();
            result.Message.Should().Be($"{nameof(WorkoutTemplate)} with id {workoutTemplateId} has been deleted successfully.");

            workoutTemplateRepositoryMock.Verify(r => r.DeleteAsync(workoutTemplateId), Times.Once);
        }

        [Fact]
        public async Task DeleteWorkoutTemplateAsync_WhenWorkoutTemplateIdIsNotValid_ShouldThrowWorkoutTemplateNotFoundException()
        {
            Guid workoutTemplateId = Guid.NewGuid();

            // Arrange
            Mock<IWorkoutTemplateRepository> workoutTemplateRepositoryMock = new();
            Mock<IMapper> mapperMock = new();

            workoutTemplateRepositoryMock
                .Setup(r => r.DeleteAsync(workoutTemplateId))
                .ReturnsAsync(false);

            WorkoutTemplateService workoutTemplateService = new(
                workoutTemplateRepositoryMock.Object,
                mapperMock.Object
                );

            //Act
            Func<Task> act = async () => await workoutTemplateService.DeleteWorkoutTemplateAsync(workoutTemplateId);

            //Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(WorkoutTemplate)}: {workoutTemplateId} is not found.");

            workoutTemplateRepositoryMock.Verify(r => r.DeleteAsync(workoutTemplateId), Times.Once);
        }

        [Fact]
        public async Task GetWorkoutTemplatesAsync_WhenRequestIsValid_ShouldReturnWorkoutTemplates()
        {
            // Arrange
            Mock<IWorkoutTemplateRepository> workoutTemplateRepositoryMock = new();
            Mock<IMapper> mapperMock = new();

            ICollection<WorkoutTemplate> workoutTemplates = [];
            workoutTemplateRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(workoutTemplates);
            ICollection<WorkoutTemplateResponseDto> workoutTemplateResponseDto = [];
            mapperMock
                .Setup(m => m.Map<ICollection<WorkoutTemplateResponseDto>>(It.IsAny<ICollection<WorkoutTemplate>>()))
                .Returns(workoutTemplateResponseDto);

            WorkoutTemplateService workoutTemplateService = new(
                workoutTemplateRepositoryMock.Object,
                mapperMock.Object
                );

            //Act
            ICollection<WorkoutTemplateResponseDto> result = await workoutTemplateService.GetWorkoutTemplatesAsync();

            //Assert
            result.Should().NotBeNull();

            workoutTemplateRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            mapperMock.Verify(m => m.Map<ICollection<WorkoutTemplateResponseDto>>(It.IsAny<ICollection<WorkoutTemplate>>()), Times.Once);
        }

        [Fact]
        public async Task GetWorkoutTemplateByIdWithExercisesAsync_WhenWorkoutTemplateIsFound_ShouldReturnWorkoutTemplateWithExercises()
        {
            Guid workoutTemplateId = Guid.NewGuid();

            // Arrange
            Mock<IWorkoutTemplateRepository> workoutTemplateRepositoryMock = new();
            Mock<IMapper> mapperMock = new();

            WorkoutTemplate workoutTemplate = new() { Id = workoutTemplateId };
            workoutTemplateRepositoryMock
                .Setup(r => r.GetWorkoutTemplateByIdWithExercises(workoutTemplateId))
                .ReturnsAsync(workoutTemplate);

            WorkoutTemplateWithExercisesDto? workoutTemplateWithExercisesDto = new() { Id = workoutTemplateId };
            mapperMock
                .Setup(m => m.Map<WorkoutTemplateWithExercisesDto>(It.IsAny<WorkoutTemplate>()))
                .Returns(workoutTemplateWithExercisesDto);

            WorkoutTemplateService workoutTemplateService = new(
                workoutTemplateRepositoryMock.Object,
                mapperMock.Object
                );

            //Act
            WorkoutTemplateWithExercisesDto? result = await workoutTemplateService.GetWorkoutTemplateByIdWithExercisesAsync(workoutTemplateId);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(workoutTemplateId);

            workoutTemplateRepositoryMock.Verify(r => r.GetWorkoutTemplateByIdWithExercises(workoutTemplateId), Times.Once);
            mapperMock.Verify(m => m.Map<WorkoutTemplateWithExercisesDto>(workoutTemplate), Times.Once);
        }

        [Fact]
        public async Task GetWorkoutTemplateByIdWithExercisesAsync_WhenWorkoutTemplateIsNotFound_ShouldThrowWorkoutTemplateNotFoundException()
        {
            Guid workoutTemplateId = Guid.NewGuid();

            // Arrange
            Mock<IWorkoutTemplateRepository> workoutTemplateRepositoryMock = new();
            Mock<IMapper> mapperMock = new();

            WorkoutTemplate workoutTemplate = new() { Id = workoutTemplateId };
            workoutTemplateRepositoryMock
                .Setup(r => r.GetWorkoutTemplateByIdWithExercises(workoutTemplateId))
                .ReturnsAsync((WorkoutTemplate?)null);

            WorkoutTemplateService workoutTemplateService = new(
                workoutTemplateRepositoryMock.Object,
                mapperMock.Object
                );

            //Act
            Func<Task> act = async () => await workoutTemplateService.GetWorkoutTemplateByIdWithExercisesAsync(workoutTemplateId);

            //Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(WorkoutTemplate)}: {workoutTemplateId} is not found.");

            workoutTemplateRepositoryMock.Verify(r => r.GetWorkoutTemplateByIdWithExercises(workoutTemplateId), Times.Once);
            mapperMock.Verify(m => m.Map<WorkoutTemplateWithExercisesDto>(It.IsAny<WorkoutTemplate>()), Times.Never);
        }
    }
}
