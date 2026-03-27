using AutoMapper;
using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.SetLog;
using Fitness.Application.Exceptions;
using Fitness.Application.Interfaces.Repositories;
using Fitness.Application.Services;
using Fitness.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Fitness.Tests.Services
{
    public class SetLogServiceTests
    {
        private static readonly Guid _setLogId = Guid.NewGuid();
        private static readonly Guid _exerciseLogId = Guid.NewGuid();

        private readonly Mock<ISetLogRepository> _setLogRepositoryMock = new();
        private readonly Mock<IExerciseLogRepository> _exerciseLogRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private SetLogService SetLogService() =>
            new(_setLogRepositoryMock.Object, _exerciseLogRepositoryMock.Object, _mapperMock.Object);

        [Fact]
        public async Task CreateSetLogAsync_WhenExerciseLogExists_ShouldCreateSetLog()
        {
            // Arrange
            CreateSetLogDto createSetLogDto = new() { Weight = 50, Reps = 10 };
            ExerciseLog exerciseLog = new() { Id = _exerciseLogId };
            SetLog setLog = new() { Id = _setLogId, ExerciseLogId = _exerciseLogId };
            SetLogResponseDto setLogResponseDto = new() { Id = _setLogId };

            _exerciseLogRepositoryMock
                .Setup(r => r.GetByIdAsync(_exerciseLogId))
                .ReturnsAsync(exerciseLog);

            _setLogRepositoryMock
                .Setup(r => r.CountByExerciseLogIdAsync(_exerciseLogId))
                .ReturnsAsync(2);

            _mapperMock
                .Setup(m => m.Map<SetLog>(createSetLogDto))
                .Returns(setLog);

            _setLogRepositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<SetLog>()))
                .ReturnsAsync(setLog);

            _mapperMock
                .Setup(m => m.Map<SetLogResponseDto>(setLog))
                .Returns(setLogResponseDto);

            SetLogService service = SetLogService();

            // Act
            SetLogResponseDto result = await service.CreateSetLogAsync(createSetLogDto, _exerciseLogId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(_setLogId);

            _exerciseLogRepositoryMock.Verify(r => r.GetByIdAsync(_exerciseLogId), Times.Once);
            _setLogRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<SetLog>()), Times.Once);
            _mapperMock.Verify(m => m.Map<SetLogResponseDto>(setLog), Times.Once);
        }

        [Fact]
        public async Task CreateSetLogAsync_WhenExerciseLogNotFound_ShouldThrowBadRequest()
        {
            // Arrange
            CreateSetLogDto createSetLogDto = new();
            _exerciseLogRepositoryMock
                .Setup(r => r.GetByIdAsync(_exerciseLogId))
                .ReturnsAsync((ExerciseLog?)null);

            SetLogService service = SetLogService();

            // Act
            Func<Task> act = async () => await service.CreateSetLogAsync(createSetLogDto, _exerciseLogId);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage($"{nameof(ExerciseLog)}: {_exerciseLogId} is not found.");

            _setLogRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<SetLog>()), Times.Never);
        }

        [Fact]
        public async Task UpdateSetLogAsync_WhenSetLogExists_ShouldUpdateSetLog()
        {
            // Arrange
            UpdateSetLogDto updateSetLogDto = new() { Weight = 60 };
            SetLog existingSetLog = new() { Id = _setLogId, Weight = 50 };
            SetLogResponseDto setLogResponseDto = new() { Id = _setLogId, Weight = 60 };

            _setLogRepositoryMock
                .Setup(r => r.GetByIdAsync(_setLogId))
                .ReturnsAsync(existingSetLog);

            _mapperMock
                .Setup(m => m.Map(updateSetLogDto, existingSetLog))
                .Returns(existingSetLog);

            _setLogRepositoryMock
                .Setup(r => r.UpdateAsync(existingSetLog))
                .ReturnsAsync(existingSetLog);

            _mapperMock
                .Setup(m => m.Map<SetLogResponseDto>(existingSetLog))
                .Returns(setLogResponseDto);

            SetLogService service = SetLogService();

            // Act
            SetLogResponseDto result = await service.UpdateSetLogAsync(_setLogId, updateSetLogDto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(_setLogId);

            _setLogRepositoryMock.Verify(r => r.GetByIdAsync(_setLogId), Times.Once);
            _setLogRepositoryMock.Verify(r => r.UpdateAsync(existingSetLog), Times.Once);
        }

        [Fact]
        public async Task UpdateSetLogAsync_WhenSetLogNotFound_ShouldThrowBadRequest()
        {
            // Arrange
            UpdateSetLogDto updateSetLogDto = new() { Weight = 60 };

            _setLogRepositoryMock
                .Setup(r => r.GetByIdAsync(_setLogId))
                .ReturnsAsync((SetLog?)null);

            SetLogService service = SetLogService();

            // Act
            Func<Task> act = async () => await service.UpdateSetLogAsync(_setLogId, updateSetLogDto);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage($"{nameof(SetLog)}: {_setLogId} is not found.");

            _setLogRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<SetLog>()), Times.Never);
        }

        [Fact]
        public async Task GetSetLogByIdAsync_WhenFound_ShouldReturnDto()
        {
            // Arrange
            SetLog setLog = new() { Id = _setLogId };
            SetLogResponseDto response = new() { Id = _setLogId };

            _setLogRepositoryMock
                .Setup(r => r.GetByIdAsync(_setLogId))
                .ReturnsAsync(setLog);

            _mapperMock
                .Setup(m => m.Map<SetLogResponseDto>(setLog))
                .Returns(response);

            SetLogService service = SetLogService();

            // Act
            SetLogResponseDto result = await service.GetSetLogByIdAsync(_setLogId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(_setLogId);

            _setLogRepositoryMock.Verify(r => r.GetByIdAsync(_setLogId), Times.Once);
        }

        [Fact]
        public async Task GetSetLogByIdAsync_WhenNotFound_ShouldThrowNotFound()
        {
            // Arrange
            _setLogRepositoryMock
                .Setup(r => r.GetByIdAsync(_setLogId))
                .ReturnsAsync((SetLog?)null);

            SetLogService service = SetLogService();

            // Act
            Func<Task> act = async () => await service.GetSetLogByIdAsync(_setLogId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(SetLog)}: {_setLogId} is not found.");
        }

        [Fact]
        public async Task GetAllSetLogsAsync_WhenCalled_ShouldReturnList()
        {
            // Arrange
            List<SetLog> setLogs = [];
            List<SetLogResponseDto> response = [];

            _setLogRepositoryMock
                .Setup(r => r.GetAllListAsync())
                .ReturnsAsync(setLogs);

            _mapperMock
                .Setup(m => m.Map<List<SetLogResponseDto>>(setLogs))
                .Returns(response);

            SetLogService service = SetLogService();

            // Act
            List<SetLogResponseDto> result = await service.GetAllSetLogsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(response);

            _setLogRepositoryMock.Verify(r => r.GetAllListAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<List<SetLogResponseDto>>(setLogs), Times.Once);
        }

        [Fact]
        public async Task DeleteSetLogAsync_WhenExists_ShouldDelete()
        {
            // Arrange
            _setLogRepositoryMock
                .Setup(r => r.DeleteAsync(_setLogId))
                .ReturnsAsync(true);

            SetLogService service = SetLogService();

            // Act
            DeleteResponseMessageDto result = await service.DeleteSetLogAsync(_setLogId);

            // Assert
            result.Should().NotBeNull();
            result.Message.Should().Be($"{nameof(SetLog)} with id {_setLogId} has been deleted successfully.");

            _setLogRepositoryMock.Verify(r => r.DeleteAsync(_setLogId), Times.Once);
        }

        [Fact]
        public async Task DeleteSetLogAsync_WhenNotFound_ShouldThrowNotFound()
        {
            // Arrange
            _setLogRepositoryMock
                .Setup(r => r.DeleteAsync(_setLogId))
                .ReturnsAsync(false);

            SetLogService service = SetLogService();

            // Act
            Func<Task> act = async () => await service.DeleteSetLogAsync(_setLogId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(SetLog)}: {_setLogId} is not found.");

            _setLogRepositoryMock.Verify(r => r.DeleteAsync(_setLogId), Times.Once);
        }
    }
}
