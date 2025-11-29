using AutoMapper;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Moq.AutoMock;
using Orcamentaria.EquipmentService.Domain.DTOs.Equipment;
using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.EquipmentService.Domain.Repositories;
using Orcamentaria.EquipmentService.Test.Fixtures;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Exceptions;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.Lib.Domain.Validators;
using System.Linq.Expressions;
using Xunit;

namespace Orcamentaria.EquipmentService.Test.Services
{
    [Collection(nameof(EquipmentMaintenanceCollection))]
    public class EquipmentMaintenanceServiceTest
    {
        private readonly EquipmentMaintenanceFixture _fixture;
        private readonly AutoMocker _mocker;
        private readonly Application.Services.EquipmentMaintenanceService _service;

        public EquipmentMaintenanceServiceTest(EquipmentMaintenanceFixture fixture)
        {
            _fixture = fixture;
            _mocker = new AutoMocker();
            _service = _mocker.CreateInstance<Application.Services.EquipmentMaintenanceService>(true);
        }

        #region GetAsync

        [Fact]
        public async Task GetAsync_WhenHaveData_ReturnsSuccessTrue()
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();
            var repositoryResponse = (
                new List<EquipmentMaintenance>() { new EquipmentMaintenance(), new EquipmentMaintenance() },
                new ResponsePagination(1, 10, 1)
            );

            var mapperResponseDTO = new EquipmentMaintenanceResponseDTO();

            _mocker.GetMock<IEquipmentMaintenanceRepository>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentMaintenance, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EquipmentMaintenance, EquipmentMaintenanceResponseDTO>(It.IsAny<EquipmentMaintenance>()))
                .Returns(mapperResponseDTO);

            var response = await _service.GetAsync(gridParams);

            response.Success.Should().BeTrue();
            response.Data.Should().HaveCount(repositoryResponse.Item1.Count());
            response.Error.Should().BeNull();
            response.Data.Should().NotBeNull();

            _mocker.GetMock<IEquipmentMaintenanceRepository>()
                .Verify(r => r.GetAsync(It.Is<GridParams>(p => p == gridParams),
                    It.IsAny<Expression<Func<EquipmentMaintenance, object>>[]>()), Times.Once);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentMaintenance, EquipmentMaintenanceResponseDTO>(It.IsAny<EquipmentMaintenance>()), Times.Exactly(repositoryResponse.Item1.Count()));
        }

        [Fact]
        public async Task GetAsync_WhenNotHaveData_ThrowsInfoException()
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();
            var repositoryResponse = (
                new List<EquipmentMaintenance>(),
                new ResponsePagination(1, 10, 0)
            );

            _mocker.GetMock<IEquipmentMaintenanceRepository>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentMaintenance, object>>[]>()
                ))
                .ReturnsAsync(repositoryResponse);

            Func<Task> act = async () => await _service.GetAsync(gridParams);

            var exception = await act.Should().ThrowAsync<InfoException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.NotFound);

            _mocker.GetMock<IEquipmentMaintenanceRepository>()
                .Verify(r => r.GetAsync(It.Is<GridParams>(p => p == gridParams),
                    It.IsAny<Expression<Func<EquipmentMaintenance, object>>[]>()), Times.Once);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentMaintenance, EquipmentMaintenanceResponseDTO>(It.IsAny<EquipmentMaintenance>()), Times.Never);
        }

        [Fact]
        public async Task GetAsync_WhenRepositoryThrowsDatabaseException_PropagatesException()
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();
            var repositoryException = new DatabaseException("Error message database.");

            _mocker.GetMock<IEquipmentMaintenanceRepository>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentMaintenance, object>>[]>()
                ))
                .ThrowsAsync(repositoryException);

            Func<Task> act = async () => await _service.GetAsync(gridParams);

            var exception = await act.Should().ThrowAsync<DatabaseException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.DatabaseError);

            _mocker.GetMock<IEquipmentMaintenanceRepository>()
                .Verify(r => r.GetAsync(It.Is<GridParams>(p => p == gridParams),
                    It.IsAny<Expression<Func<EquipmentMaintenance, object>>[]>()), Times.Once);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentMaintenance, EquipmentMaintenanceResponseDTO>(It.IsAny<EquipmentMaintenance>()), Times.Never);
        }

        #endregion

        #region InsertAsync

        [Fact]
        public async Task InsertAsync_WhenValidBody_ReturnsSuccessTrue()
        {
            var mapperInsertToEntity = new EquipmentMaintenance();
            var mapperResponseDTO = new EquipmentMaintenanceResponseDTO();
            var validationResult = new ValidationResult();
            var repositoryResponse = new EquipmentMaintenance();
            var serviceRequest = new EquipmentMaintenanceInsertDTO();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EquipmentMaintenanceInsertDTO, EquipmentMaintenance>(It.IsAny<EquipmentMaintenanceInsertDTO>()))
                .Returns(mapperInsertToEntity);

            _mocker.GetMock<IValidatorEntity<EquipmentMaintenance>>()
                .Setup(v => v.ValidateBeforeInsert(It.IsAny<EquipmentMaintenance>()))
                .Returns(validationResult);

            _mocker.GetMock<IEquipmentMaintenanceRepository>()
                .Setup(r => r.InsertAsync(It.IsAny<EquipmentMaintenance>()))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EquipmentMaintenance, EquipmentMaintenanceResponseDTO>(It.IsAny<EquipmentMaintenance>()))
                .Returns(mapperResponseDTO);

            var response = await _service.InsertAsync(serviceRequest);

            response.Success.Should().BeTrue();
            response.Data.Should().BeSameAs(mapperResponseDTO);
            response.Error.Should().BeNull();
            response.Data.Should().NotBeNull();

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentMaintenanceInsertDTO, EquipmentMaintenance>(It.IsAny<EquipmentMaintenanceInsertDTO>()), Times.Once);

            _mocker.GetMock<IValidatorEntity<EquipmentMaintenance>>()
                .Verify(v => v.ValidateBeforeInsert(It.IsAny<EquipmentMaintenance>()), Times.Once);

            _mocker.GetMock<IEquipmentMaintenanceRepository>()
                .Verify(r => r.InsertAsync(It.IsAny<EquipmentMaintenance>()), Times.Once);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentMaintenance, EquipmentMaintenanceResponseDTO>(It.IsAny<EquipmentMaintenance>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_WhenInvalidBody_ThrowsValidationException()
        {
            var mapperInsertToEntity = new EquipmentMaintenance();
            var validationResult = new ValidationResult
            {
                Errors = new List<ValidationFailure> { new ValidationFailure("Property", "Error message validation.") }
            };
            var serviceRequest = new EquipmentMaintenanceInsertDTO();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EquipmentMaintenanceInsertDTO, EquipmentMaintenance>(It.IsAny<EquipmentMaintenanceInsertDTO>()))
                .Returns(mapperInsertToEntity);

            _mocker.GetMock<IValidatorEntity<EquipmentMaintenance>>()
                .Setup(v => v.ValidateBeforeInsert(It.IsAny<EquipmentMaintenance>()))
                .Returns(validationResult);

            Func<Task> act = async () => await _service.InsertAsync(serviceRequest);

            var exception = await act.Should().ThrowAsync<ValidationException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.ValidationFailed);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentMaintenanceInsertDTO, EquipmentMaintenance>(It.IsAny<EquipmentMaintenanceInsertDTO>()), Times.Once);

            _mocker.GetMock<IValidatorEntity<EquipmentMaintenance>>()
                .Verify(v => v.ValidateBeforeInsert(It.IsAny<EquipmentMaintenance>()), Times.Once);

            _mocker.GetMock<IEquipmentMaintenanceRepository>()
                .Verify(r => r.InsertAsync(It.IsAny<EquipmentMaintenance>()), Times.Never);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentMaintenance, EquipmentMaintenanceResponseDTO>(It.IsAny<EquipmentMaintenance>()), Times.Never);
        }

        [Fact]
        public async Task InsertAsync_WhenRepositoryThrowsDatabaseException_PropagatesException()
        {
            var mapperInsertToEntity = new EquipmentMaintenance();
            var repositoryException = new DatabaseException("Error message database.");
            var validationResult = new ValidationResult();
            var serviceRequest = new EquipmentMaintenanceInsertDTO();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EquipmentMaintenanceInsertDTO, EquipmentMaintenance>(It.IsAny<EquipmentMaintenanceInsertDTO>()))
                .Returns(mapperInsertToEntity);

            _mocker.GetMock<IValidatorEntity<EquipmentMaintenance>>()
                .Setup(v => v.ValidateBeforeInsert(It.IsAny<EquipmentMaintenance>()))
                .Returns(validationResult);

            _mocker.GetMock<IEquipmentMaintenanceRepository>()
                .Setup(r => r.InsertAsync(It.IsAny<EquipmentMaintenance>()))
                .ThrowsAsync(repositoryException);

            Func<Task> act = async () => await _service.InsertAsync(serviceRequest);

            var exception = await act.Should().ThrowAsync<DatabaseException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.DatabaseError);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentMaintenanceInsertDTO, EquipmentMaintenance>(It.IsAny<EquipmentMaintenanceInsertDTO>()), Times.Once);

            _mocker.GetMock<IValidatorEntity<EquipmentMaintenance>>()
                .Verify(v => v.ValidateBeforeInsert(It.IsAny<EquipmentMaintenance>()), Times.Once);

            _mocker.GetMock<IEquipmentMaintenanceRepository>()
                .Verify(r => r.InsertAsync(It.IsAny<EquipmentMaintenance>()), Times.Once);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentMaintenance, EquipmentMaintenanceResponseDTO>(It.IsAny<EquipmentMaintenance>()), Times.Never);
        }

        #endregion
    }
}
