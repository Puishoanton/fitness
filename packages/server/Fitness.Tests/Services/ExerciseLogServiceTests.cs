using AutoMapper;
using Fitness.Application.DTOs.ExerciseLog;
using Fitness.Application.Exceptions;
using Fitness.Application.Interfaces.Repositories;
using Fitness.Application.Services;
using Fitness.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Fitness.Tests.Services
{
    public class ExerciseLogServiceTests
    {
        private static readonly Guid _exerciseId = Guid.NewGuid();
        private static readonly Guid _workoutSessionId = Guid.NewGuid();
        private static readonly Guid _exerciseLogId = Guid.NewGuid();

        private readonly Mock<IExerciseLogRepository> _exerciseLogRepositoryMock = new();
        private readonly Mock<IWorkoutSessionRepository> _workoutSessionRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private ExerciseLogService ExerciseLogService() =>
            new(_exerciseLogRepositoryMock.Object, _workoutSessionRepositoryMock.Object, _mapperMock.Object);

        [Fact]
        public async Task CreateExerciseLogAsync_WhenWorkoutSessionExists_ShouldCreateExerciseLog()
        {
            // Arrange
            WorkoutSession workoutSession = new() { Id = _workoutSessionId };
            ExerciseLog exerciseLog = new() { Id = _exerciseLogId, ExerciseId = _exerciseId, WorkoutSessionId = _workoutSessionId };
            ExerciseLogLightDto exerciseLogLightDto = new() { Id = _exerciseLogId };

            _workoutSessionRepositoryMock
                .Setup(r => r.GetByIdAsync(_workoutSessionId))
                .ReturnsAsync(workoutSession);

            _exerciseLogRepositoryMock
                .Setup(r => r.CountByWorkoutSessionIdAsync(_workoutSessionId))
                .ReturnsAsync(0);

            _exerciseLogRepositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<ExerciseLog>()))
                .ReturnsAsync(exerciseLog);

            _mapperMock
                .Setup(m => m.Map<ExerciseLogLightDto>(It.IsAny<ExerciseLog>()))
                .Returns(exerciseLogLightDto);

            ExerciseLogService exerciseLogService = ExerciseLogService();

            // Act
            ExerciseLogLightDto result = await exerciseLogService.CreateExerciseLogAsync(_exerciseId, _workoutSessionId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(_exerciseLogId);

            _workoutSessionRepositoryMock.Verify(r => r.GetByIdAsync(_workoutSessionId), Times.Once);
            _exerciseLogRepositoryMock.Verify(r => r.CountByWorkoutSessionIdAsync(_workoutSessionId), Times.Once);
            _exerciseLogRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<ExerciseLog>()), Times.Once);
            _mapperMock.Verify(m => m.Map<ExerciseLogLightDto>(It.IsAny<ExerciseLog>()), Times.Once);
        }

        [Fact]
        public async Task CreateExerciseLogAsync_WhenWorkoutSessionDoesNotExist_ShouldThrowBadRequest()
        {
            // Arrange
            _workoutSessionRepositoryMock
                .Setup(r => r.GetByIdAsync(_workoutSessionId))
                .ReturnsAsync((WorkoutSession?)null);

            ExerciseLogService exerciseLogService = ExerciseLogService();

            // Act
            Func<Task> act = async () => await exerciseLogService.CreateExerciseLogAsync(_exerciseId, _workoutSessionId);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage($"{nameof(WorkoutSession)}: {_workoutSessionId} is not found.");

            _workoutSessionRepositoryMock.Verify(r => r.GetByIdAsync(_workoutSessionId), Times.Once);
            _exerciseLogRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<ExerciseLog>()), Times.Never);
        }

        [Fact]
        public async Task UpdateExerciseLogAsync_WhenExists_ShouldUpdateExerciseLog()
        {
            // Arrange
            UpdateExerciseLogDto updateExerciseLogDto = new() { Order = 2 };
            ExerciseLog exerciseLog = new() { Id = _exerciseLogId, ExerciseId = _exerciseId };
            ExerciseLogLightDto exerciseLogLightDto = new() { Id = _exerciseLogId };

            _exerciseLogRepositoryMock
                .Setup(r => r.GetByIdAsync(_exerciseLogId))
                .ReturnsAsync(exerciseLog);

            _mapperMock
                .Setup(m => m.Map(updateExerciseLogDto, exerciseLog))
                .Returns(exerciseLog);

            _exerciseLogRepositoryMock
                .Setup(r => r.UpdateAsync(exerciseLog))
                .ReturnsAsync(exerciseLog);

            _mapperMock
                .Setup(m => m.Map<ExerciseLogLightDto>(exerciseLog))
                .Returns(exerciseLogLightDto);

            ExerciseLogService exerciseLogService = ExerciseLogService();

            // Act
            ExerciseLogLightDto result = await exerciseLogService.UpdateExerciseLogAsync(_exerciseLogId, updateExerciseLogDto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(_exerciseLogId);

            _exerciseLogRepositoryMock.Verify(r => r.GetByIdAsync(_exerciseLogId), Times.Once);
            _exerciseLogRepositoryMock.Verify(r => r.UpdateAsync(exerciseLog), Times.Once);
            _mapperMock.Verify(m => m.Map(updateExerciseLogDto, exerciseLog), Times.Once);
            _mapperMock.Verify(m => m.Map<ExerciseLogLightDto>(exerciseLog), Times.Once);
        }

        [Fact]
        public async Task UpdateExerciseLogAsync_WhenNotFound_ShouldThrowNotFound()
        {
            // Arrange
            UpdateExerciseLogDto updateExerciseLogDto = new() { Order = 2 };

            _exerciseLogRepositoryMock
                .Setup(r => r.GetByIdAsync(_exerciseLogId))
                .ReturnsAsync((ExerciseLog?)null);

            ExerciseLogService exerciseLogService = ExerciseLogService();

            // Act
            Func<Task> act = async () => await exerciseLogService.UpdateExerciseLogAsync(_exerciseLogId, updateExerciseLogDto);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(ExerciseLog)}: {_exerciseLogId} is not found.");

            _exerciseLogRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<ExerciseLog>()), Times.Never);
        }

        [Fact]
        public async Task GetExerciseLogByIdAsync_WhenFound_ShouldReturnDto()
        {
            // Arrange
            ExerciseLog exerciseLog = new() { Id = _exerciseLogId };
            ExerciseLogLightDto exerciseLogLightDto = new() { Id = _exerciseLogId };

            _exerciseLogRepositoryMock
                .Setup(r => r.GetByIdAsync(_exerciseLogId))
                .ReturnsAsync(exerciseLog);

            _mapperMock
                .Setup(m => m.Map<ExerciseLogLightDto>(exerciseLog))
                .Returns(exerciseLogLightDto);

            ExerciseLogService exerciseLogService = ExerciseLogService();

            // Act
            ExerciseLogLightDto result = await exerciseLogService.GetExerciseLogByIdAsync(_exerciseLogId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(_exerciseLogId);

            _exerciseLogRepositoryMock.Verify(r => r.GetByIdAsync(_exerciseLogId), Times.Once);
            _mapperMock.Verify(m => m.Map<ExerciseLogLightDto>(exerciseLog), Times.Once);
        }

        [Fact]
        public async Task GetExerciseLogByIdAsync_WhenNotFound_ShouldThrowNotFound()
        {
            // Arrange
            _exerciseLogRepositoryMock
                .Setup(r => r.GetByIdAsync(_exerciseLogId))
                .ReturnsAsync((ExerciseLog?)null);

            ExerciseLogService exerciseLogService = ExerciseLogService();

            // Act
            Func<Task> act = async () => await exerciseLogService.GetExerciseLogByIdAsync(_exerciseLogId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(ExerciseLog)}: {_exerciseLogId} is not found.");

            _mapperMock.Verify(m => m.Map<ExerciseLogLightDto>(It.IsAny<ExerciseLog>()), Times.Never);
        }

        [Fact]
        public async Task GetAllExerciseLogsAsync_WhenCalled_ShouldReturnList()
        {
            // Arrange
            List<ExerciseLog> exerciseLogs = [];
            List<ExerciseLogLightDto> response = [];

            _exerciseLogRepositoryMock
                .Setup(r => r.GetAllListAsync())
                .ReturnsAsync(exerciseLogs);

            _mapperMock
                .Setup(m => m.Map<List<ExerciseLogLightDto>>(exerciseLogs))
                .Returns(response);

            ExerciseLogService exerciseLogService = ExerciseLogService();

            // Act
            List<ExerciseLogLightDto> result = await exerciseLogService.GetAllExerciseLogsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(response);

            _exerciseLogRepositoryMock.Verify(r => r.GetAllListAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<List<ExerciseLogLightDto>>(exerciseLogs), Times.Once);
        }
    }
}
