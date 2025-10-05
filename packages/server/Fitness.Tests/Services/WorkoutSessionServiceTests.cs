using System.Collections.Generic;
using AutoMapper;
using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.WorkoutSession;
using Fitness.Application.Exceptions;
using Fitness.Application.Interfaces.Repositories;
using Fitness.Application.Services;
using Fitness.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Fitness.Tests.Services
{
    public class WorkoutSessionServiceTests
    {
        private static readonly Guid _workoutTemplateId = Guid.NewGuid();
        private static readonly Guid _workoutSessionId = Guid.NewGuid();
        private static readonly Guid _userId = Guid.NewGuid();
        private readonly Mock<IWorkoutSessionRepository> _workoutSessionRepositoryMock = new();
        private readonly Mock<IWorkoutTemplateRepository> _workoutTemplateRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private WorkoutSessionService WorkoutSessionService() =>
            new(_workoutSessionRepositoryMock.Object, _workoutTemplateRepositoryMock.Object, _mapperMock.Object);
        [Fact]
        public async Task CreateWorkoutSessionAsync_WhenTemplateExists_ShouldCreateSession()
        {
            // Arrange
            WorkoutTemplate workoutTemplate = new() { Id = _workoutTemplateId };
            WorkoutSession workoutSession = new() { Id = _workoutSessionId, UserId = _userId, WorkoutTemplateId = _workoutTemplateId };
            WorkoutSessionResponseDto workoutSessionResponseDto = new() { Id = _workoutSessionId };

            _workoutTemplateRepositoryMock
                .Setup(r => r.GetByIdAsync(_workoutTemplateId))
                .ReturnsAsync(workoutTemplate);

            _workoutSessionRepositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<WorkoutSession>()))
                .ReturnsAsync(workoutSession);

            _mapperMock
                .Setup(m => m.Map<WorkoutSessionResponseDto>(It.IsAny<WorkoutSession>()))
                .Returns(workoutSessionResponseDto);

            WorkoutSessionService workoutSessionService = WorkoutSessionService();
            
            // Act
            WorkoutSessionResponseDto result = await workoutSessionService.CreateWorkoutSessionAsync(_userId, _workoutTemplateId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(_workoutSessionId);

            _workoutTemplateRepositoryMock.Verify(r => r.GetByIdAsync(_workoutTemplateId), Times.Once);
            _workoutSessionRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<WorkoutSession>()), Times.Once);
            _mapperMock.Verify(m => m.Map<WorkoutSessionResponseDto>(It.IsAny<WorkoutSession>()), Times.Once);
        }

        [Fact]
        public async Task CreateWorkoutSessionAsync_WhenTemplateDoesNotExist_ShouldThrowBadRequest()
        {
            // Arrange
            _workoutTemplateRepositoryMock
                .Setup(r => r.GetByIdAsync(_workoutTemplateId))
                .ReturnsAsync((WorkoutTemplate?)null);

            WorkoutSessionService workoutSessionService = WorkoutSessionService();

            // Act
            Func<Task> act = async () => await workoutSessionService.CreateWorkoutSessionAsync(_userId, _workoutTemplateId);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage($"{nameof(WorkoutTemplate)}: {_workoutTemplateId} is not found.");

            _workoutTemplateRepositoryMock.Verify(r => r.GetByIdAsync(_workoutTemplateId), Times.Once);
            _workoutSessionRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<WorkoutSession>()), Times.Never);
        }
        [Fact]
        public async Task UpdateWorkoutSessionAsync_WhenSessionExists_ShouldUpdateSession()
        {
            // Arrange
            UpdateWorkoutSessionDto updateWorkoutSessionDto = new () { Duration = 1 };
            WorkoutSession workoutSession = new() { Id = _workoutSessionId, UserId = _userId, WorkoutTemplateId = _workoutTemplateId };
            WorkoutSessionResponseDto workoutSessionResponseDto = new () { Id = _workoutSessionId };

            _workoutSessionRepositoryMock
                .Setup(r => r.GetByIdAsync(_workoutSessionId))
                .ReturnsAsync(workoutSession);

            _mapperMock
                .Setup(m => m.Map(updateWorkoutSessionDto, workoutSession))
                .Returns(workoutSession);

            _workoutSessionRepositoryMock
                .Setup(r => r.UpdateAsync(workoutSession))
                .ReturnsAsync(workoutSession);

            _mapperMock
                .Setup(m => m.Map<WorkoutSessionResponseDto>(workoutSession))
                .Returns(workoutSessionResponseDto);

            WorkoutSessionService workoutSessionService = WorkoutSessionService();

            // Act
            WorkoutSessionResponseDto result = await workoutSessionService.UpdateWorkoutSessionAsync(_workoutSessionId, updateWorkoutSessionDto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(_workoutSessionId);

            _workoutSessionRepositoryMock.Verify(r => r.GetByIdAsync(_workoutSessionId), Times.Once);
            _workoutSessionRepositoryMock.Verify(r => r.UpdateAsync(workoutSession), Times.Once);
        }

        [Fact]
        public async Task UpdateWorkoutSessionAsync_WhenSessionDoesNotExist_ShouldThrowNotFound()
        {
            // Arrange
            UpdateWorkoutSessionDto updateWorkoutSessionDto = new () { Duration = 1 };

            _workoutSessionRepositoryMock
                .Setup(r => r.GetByIdAsync(_workoutSessionId))
                .ReturnsAsync((WorkoutSession?)null);

            WorkoutSessionService workoutSessionService = WorkoutSessionService();

            // Act
            Func<Task> act = async () => await workoutSessionService.UpdateWorkoutSessionAsync(_workoutSessionId, updateWorkoutSessionDto);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(WorkoutSession)}: {_workoutSessionId} is not found.");

            _workoutSessionRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<WorkoutSession>()), Times.Never);
        }

        [Fact]
        public async Task GetWorkoutSessionByIdAsync_WhenFound_ShouldReturnDto()
        {
            // Arrange
            WorkoutSession workoutSession = new () { Id = _workoutSessionId };
            WorkoutSessionResponseDto workoutSessionResponseDto = new () { Id = _workoutSessionId };

            _workoutSessionRepositoryMock
                .Setup(r => r.GetByIdAsync(_workoutSessionId))
                .ReturnsAsync(workoutSession);

            _mapperMock
                .Setup(m => m.Map<WorkoutSessionResponseDto>(workoutSession))
                .Returns(workoutSessionResponseDto);

            WorkoutSessionService workoutSessionService = WorkoutSessionService();

            // Act
            WorkoutSessionResponseDto result = await workoutSessionService.GetWorkoutSessionByIdAsync(_workoutSessionId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(_workoutSessionId);

            _workoutSessionRepositoryMock.Verify(r => r.GetByIdAsync(_workoutSessionId), Times.Once);
            _mapperMock.Verify(m => m.Map<WorkoutSessionResponseDto>(workoutSession), Times.Once);
        }

        [Fact]
        public async Task GetWorkoutSessionByIdAsync_WhenNotFound_ShouldThrowNotFound()
        {
            // Arrange
            _workoutSessionRepositoryMock
                .Setup(r => r.GetByIdAsync(_workoutSessionId))
                .ReturnsAsync((WorkoutSession?)null);

            WorkoutSessionService workoutSessionService = WorkoutSessionService();

            // Act
            Func<Task> act = async () => await workoutSessionService.GetWorkoutSessionByIdAsync(_workoutSessionId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(WorkoutSession)}: {_workoutSessionId} is not found.");
        }

        [Fact]
        public async Task GetAllWorkoutSessionsAsync_WhenCalled_ShouldReturnList()
        {
            // Arrange
            List<WorkoutSession> workoutSessions = [];
            List<WorkoutSessionLightDto> response = [];

            _workoutSessionRepositoryMock
                .Setup(r => r.GetAllListAsync())
                .ReturnsAsync(workoutSessions);

            _mapperMock
                .Setup(m => m.Map<List<WorkoutSessionLightDto>>(workoutSessions))
                .Returns(response);

            WorkoutSessionService workoutSessionService = WorkoutSessionService();

            // Act
            List<WorkoutSessionLightDto> result = await workoutSessionService.GetAllWorkoutSessionsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(response);

            _workoutSessionRepositoryMock.Verify(r => r.GetAllListAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<List<WorkoutSessionLightDto>>(workoutSessions), Times.Once);
        }

        [Fact]
        public async Task DeleteWorkoutSessionAsync_WhenExists_ShouldDelete()
        {
            // Arrange
            _workoutSessionRepositoryMock
                .Setup(r => r.DeleteAsync(_workoutSessionId))
                .ReturnsAsync(true);

            WorkoutSessionService workoutSessionService = WorkoutSessionService();

            // Act
            DeleteResponseMessageDto result = await workoutSessionService.DeleteWorkoutSessionAsync(_workoutSessionId);

            // Assert
            result.Should().NotBeNull();
            result.Message.Should().Be($"{nameof(WorkoutSession)} with id {_workoutSessionId} has been deleted successfully.");

            _workoutSessionRepositoryMock.Verify(r => r.DeleteAsync(_workoutSessionId), Times.Once);
        }

        [Fact]
        public async Task DeleteWorkoutSessionAsync_WhenNotFound_ShouldThrowNotFound()
        {
            // Arrange
            _workoutSessionRepositoryMock
                .Setup(r => r.DeleteAsync(_workoutSessionId))
                .ReturnsAsync(false);

            WorkoutSessionService workoutSessionService = WorkoutSessionService();

            // Act
            Func<Task> act = async () => await workoutSessionService.DeleteWorkoutSessionAsync(_workoutSessionId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(WorkoutSession)}: {_workoutSessionId} is not found.");

            _workoutSessionRepositoryMock.Verify(r => r.DeleteAsync(_workoutSessionId), Times.Once);
        }
    }
}
