using AutoMapper;
using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.Exercise;
using Fitness.Application.Exceptions;
using Fitness.Application.Interfaces.Repositories;
using Fitness.Application.Services;
using Fitness.Domain.Entities;
using Fitness.Domain.Enums;
using FluentAssertions;
using Moq;

namespace Fitness.Tests.Services
{
    public class ExerciseServiceTests
    {
        private static readonly Guid _exerciseId = Guid.NewGuid();
        private const string _exerciseName = "exercise_name";
        private const string _exerciseDescription = "exercise_description";
        private const MuscleGroup _exerciseMuscleGroup = MuscleGroup.Chest;

        [Fact]
        public async Task CreateExerciseAsync_WhenCreateExerciseDtoIsValid_ShouldCreateExercise()
        {
            CreateExerciseDto createExerciseDto = new()
            {
                Name = _exerciseName,
                Description = _exerciseDescription,
                MuscleGroup = _exerciseMuscleGroup
            };
            Exercise exercise = new()
            {
                Name = _exerciseName,
                Description = _exerciseDescription,
                MuscleGroup = _exerciseMuscleGroup
            };
            ExerciseResponseDto responseExerciseDto = new()
            {
                Id = _exerciseId,
                Name = _exerciseName,
                Description = _exerciseDescription,
                MuscleGroup = _exerciseMuscleGroup
            };

            // Arrange
            Mock<IExerciseRepository> exerciseRepositoryMock = new();
            Mock<IMapper> mapperMock = new();


            mapperMock
                .Setup(m => m.Map<Exercise>(It.IsAny<CreateExerciseDto>()))
                .Returns(exercise);

            exerciseRepositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<Exercise>()))
                .ReturnsAsync(exercise);

            mapperMock
                .Setup(m => m.Map<ExerciseResponseDto>(It.IsAny<Exercise>()))
                .Returns(responseExerciseDto);

            ExerciseService exerciseService = new(
                exerciseRepositoryMock.Object,
                mapperMock.Object
                );

            //Act
            ExerciseResponseDto result = await exerciseService.CreateExerciseAsync(createExerciseDto);

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(_exerciseName);
            result.Description.Should().Be(_exerciseDescription);
            result.MuscleGroup.Should().Be(_exerciseMuscleGroup);

            mapperMock.Verify(m => m.Map<Exercise>(It.IsAny<CreateExerciseDto>()), Times.Once);
            exerciseRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Exercise>()), Times.Once);
            mapperMock.Verify(m => m.Map<ExerciseResponseDto>(It.IsAny<Exercise>()), Times.Once);
        }

        [Fact]
        public async Task UpdateExerciseAsync_WhenExerciseIdIsValid_ShouldUpdateExercise()
        {
            UpdateExerciseDto updateExerciseDto = new()
            {
                Name = _exerciseName,
                Description = _exerciseDescription,
                MuscleGroup = _exerciseMuscleGroup
            };
            Exercise exercise = new() { Id = _exerciseId };
            Exercise updateExercise = new()
            {
                Name = _exerciseName,
                Description = _exerciseDescription,
                MuscleGroup = _exerciseMuscleGroup
            };
            ExerciseResponseDto responseExerciseDto = new()
            {
                Id = _exerciseId,
                Name = _exerciseName,
                Description = _exerciseDescription,
                MuscleGroup = _exerciseMuscleGroup
            };

            // Arrange
            Mock<IExerciseRepository> exerciseRepositoryMock = new();
            Mock<IMapper> mapperMock = new();

            exerciseRepositoryMock
                .Setup(r => r.GetByIdAsync(_exerciseId))
                .ReturnsAsync(exercise);

            mapperMock
                .Setup(m => m.Map(updateExerciseDto, exercise))
                .Returns(updateExercise);

            exerciseRepositoryMock
                .Setup(r => r.UpdateAsync(updateExercise))
                .ReturnsAsync(updateExercise);

            mapperMock
                .Setup(m => m.Map<ExerciseResponseDto>(It.IsAny<Exercise>()))
                .Returns(responseExerciseDto);

            ExerciseService exerciseService = new(
                exerciseRepositoryMock.Object,
                mapperMock.Object
                );

            //Act
            ExerciseResponseDto result = await exerciseService.UpdateExerciseAsync(updateExerciseDto, _exerciseId);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(_exerciseId);
            result.Name.Should().Be(_exerciseName);
            result.Description.Should().Be(_exerciseDescription);
            result.MuscleGroup.Should().Be(_exerciseMuscleGroup);

            exerciseRepositoryMock.Verify(r => r.GetByIdAsync(_exerciseId), Times.Once);
            mapperMock.Verify(m => m.Map(updateExerciseDto, It.Is<Exercise>(e => e.Id == _exerciseId)), Times.Once);
            exerciseRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Exercise>()), Times.Once);
            mapperMock.Verify(m => m.Map<ExerciseResponseDto>(It.IsAny<Exercise>()), Times.Once);
        }

        [Fact]
        public async Task UpdateExerciseAsync_WhenExerciseIdIsNotValid_ShouldThrowNotFoundException()
        {
            UpdateExerciseDto updateExerciseDto = new()
            {
                Name = _exerciseName,
                Description = _exerciseDescription,
                MuscleGroup = _exerciseMuscleGroup
            };
            // Arrange
            Mock<IExerciseRepository> exerciseRepositoryMock = new();
            Mock<IMapper> mapperMock = new();

            exerciseRepositoryMock
                .Setup(r => r.GetByIdAsync(_exerciseId))
                .ReturnsAsync((Exercise?)null);

            ExerciseService exerciseService = new(
                exerciseRepositoryMock.Object,
                mapperMock.Object
                );

            //Act
            Func<Task> act = async () => await exerciseService.UpdateExerciseAsync(updateExerciseDto, _exerciseId);

            //Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(Exercise)}: {_exerciseId} is not found.");

            exerciseRepositoryMock.Verify(r => r.GetByIdAsync(_exerciseId), Times.Once);
            mapperMock.Verify(m => m.Map(It.IsAny<UpdateExerciseDto>(), It.IsAny<Exercise>()), Times.Never);
            exerciseRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Exercise>()), Times.Never);
            mapperMock.Verify(m => m.Map<ExerciseResponseDto>(It.IsAny<Exercise>()), Times.Never);
        }

        [Fact]
        public async Task DeleteExerciseAsync_WhenExerciseIdIsValid_ShouldDeleteExercise()
        {
            // Arrange
            Mock<IExerciseRepository> exerciseRepositoryMock = new();
            Mock<IMapper> mapperMock = new();

            exerciseRepositoryMock
                .Setup(r => r.DeleteAsync(_exerciseId))
                .ReturnsAsync(true);

            ExerciseService exerciseService = new(
                exerciseRepositoryMock.Object,
                mapperMock.Object
                );

            //Act
            DeleteResponseMessageDto result = await exerciseService.DeleteExerciseAsync(_exerciseId);

            //Assert
            result.Should().NotBeNull();
            result.Message.Should().Be($"{nameof(Exercise)} with id {_exerciseId} has been deleted successfully.");

            exerciseRepositoryMock.Verify(r => r.DeleteAsync(_exerciseId), Times.Once);
        }

        [Fact]
        public async Task DeleteExerciseAsync_WhenExerciseIdIsNotValid_ShouldThrowNotFoundException()
        {
            // Arrange
            Mock<IExerciseRepository> exerciseRepositoryMock = new();
            Mock<IMapper> mapperMock = new();

            exerciseRepositoryMock
                .Setup(r => r.DeleteAsync(_exerciseId))
                .ReturnsAsync(false);

            ExerciseService exerciseService = new(
                exerciseRepositoryMock.Object,
                mapperMock.Object
                );

            //Act
            Func<Task> act = async () => await exerciseService.DeleteExerciseAsync(_exerciseId);

            //Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(Exercise)}: {_exerciseId} is not found.");

            exerciseRepositoryMock.Verify(r => r.DeleteAsync(_exerciseId), Times.Once);
        }

        [Fact]
        public async Task GetAllExercisesAsync_WhenRequestIsValid_ShouldReturnAllExercises()
        {
            ICollection<Exercise> exercises = [];
            ICollection<ExerciseLightDto> exercisesResponseDto = [];

            // Arrange
            Mock<IExerciseRepository> exerciseRepositoryMock = new();
            Mock<IMapper> mapperMock = new();

            exerciseRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(exercises);
            mapperMock
                .Setup(m => m.Map<ICollection<ExerciseLightDto>>(It.IsAny<ICollection<Exercise>>()))
                .Returns(exercisesResponseDto);

            ExerciseService exerciseService = new(
                exerciseRepositoryMock.Object,
                mapperMock.Object
                );

            //Act
            ICollection<ExerciseLightDto> result = await exerciseService.GetAllExercisesAsync();

            //Assert
            result.Should().NotBeNull();

            exerciseRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            mapperMock.Verify(m => m.Map<ICollection<ExerciseLightDto>>(It.IsAny<ICollection<Exercise>>()), Times.Once);
        }

        [Fact]
        public async Task GetExerciseByIdAsync_WhenExerciseIsFound_ShouldReturnExerciseResponseDto()
        {
            Exercise exercise = new() { Id = _exerciseId };
            ExerciseResponseDto responseExerciseDto = new()
            {
                Id = _exerciseId,
                Name = _exerciseName,
                Description = _exerciseDescription,
                MuscleGroup = _exerciseMuscleGroup
            };

            // Arrange
            Mock<IExerciseRepository> exerciseRepositoryMock = new();
            Mock<IMapper> mapperMock = new();

            exerciseRepositoryMock
                .Setup(r => r.GetByIdAsync(_exerciseId))
                .ReturnsAsync(exercise);

            mapperMock
                .Setup(m => m.Map<ExerciseResponseDto>(exercise))
                .Returns(responseExerciseDto);

            ExerciseService exerciseService = new(
                exerciseRepositoryMock.Object,
                mapperMock.Object
                );

            //Act
            ExerciseResponseDto? result = await exerciseService.GetExerciseByIdAsync(_exerciseId);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(_exerciseId);

            exerciseRepositoryMock.Verify(r => r.GetByIdAsync(_exerciseId), Times.Once);
            mapperMock.Verify(m => m.Map<ExerciseResponseDto>(It.Is<Exercise>(e => e.Id == _exerciseId)), Times.Once);
        }

        [Fact]
        public async Task GetExerciseByIdAsync_WhenExerciseIsNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            Mock<IExerciseRepository> exerciseRepositoryMock = new();
            Mock<IMapper> mapperMock = new();

            exerciseRepositoryMock
                .Setup(r => r.GetByIdAsync(_exerciseId))
                .ReturnsAsync((Exercise?)null);

            ExerciseService exerciseService = new(
                exerciseRepositoryMock.Object,
                mapperMock.Object
                );

            //Act
            Func<Task> act = async () => await exerciseService.GetExerciseByIdAsync(_exerciseId);

            //Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(Exercise)}: {_exerciseId} is not found.");

            exerciseRepositoryMock.Verify(r => r.GetByIdAsync(_exerciseId), Times.Once);
            mapperMock.Verify(m => m.Map<ExerciseResponseDto>(It.IsAny<Exercise>()), Times.Never);
        }
    }
}
