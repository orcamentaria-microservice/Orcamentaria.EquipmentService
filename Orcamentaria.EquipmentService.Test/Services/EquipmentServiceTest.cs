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
    [Collection(nameof(EquipmentCollection))]
    public class EquipmentServiceTest
    {
        private readonly EquipmentFixture _fixture;
        private readonly AutoMocker _mocker;
        private readonly Application.Services.EquipmentService _service;

        public EquipmentServiceTest(EquipmentFixture fixture)
        {
            _fixture = fixture;
            _mocker = new AutoMocker();
            _service = _mocker.CreateInstance<Application.Services.EquipmentService>(true);
        }

        #region GetByIdAsync

        [Xunit.Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(6)]
        public async Task GetByIdAsync_WhenHaveData_ReturnsData(long id)
        {
            var repositoryResponse = _fixture.CreateEntity(1);

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Setup(r => r.GetByIdAsync(It.Is<long>(p => p == id), It.IsAny<Expression<Func<Equipment, object>>[]>()))
            .ReturnsAsync(repositoryResponse);

            var response = await _service.GetByIdAsync(id);

            response.Should().NotBeNull();
            response.Should().BeSameAs(repositoryResponse);

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Verify(r => r.GetByIdAsync(It.Is<long>(p => p == id), It.IsAny<Expression<Func<Equipment, object>>[]>()
                ), Times.Once());
        }

        [Xunit.Theory]
        [InlineData(0)]
        public async Task GetByIdAsync_WhenNotHaveData_ReturnsNull(long id)
        {
            var repositoryResponse = (Equipment)null;

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Setup(r => r.GetByIdAsync(It.Is<long>(p => p == id), It.IsAny<Expression<Func<Equipment, object>>[]>()))
            .ReturnsAsync(repositoryResponse);

            var response = await _service.GetByIdAsync(id);

            response.Should().BeNull();

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Verify(r => r.GetByIdAsync(
                    It.Is<long>(p => p == id), It.IsAny<Expression<Func<Equipment, object>>[]>()
                ), Times.Once());
        }

        [Xunit.Theory]
        [InlineData(87)]
        public async Task GetByIdAsync_WhenRepositoryThrowsDatabaseException_PropagatesException(long id)
        {
            var repositoryResponse = new DatabaseException("Error message database.");

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Setup(r => r.GetByIdAsync(It.Is<long>(p => p == id), It.IsAny<Expression<Func<Equipment, object>>[]>()))
                .ThrowsAsync(repositoryResponse);

            Func<Task> act = async () => await _service.GetByIdAsync(id);

            var exception = await act.Should().ThrowAsync<DatabaseException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.DatabaseError);

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Verify(r => r.GetByIdAsync(
                    It.Is<long>(p => p == id), It.IsAny<Expression<Func<Equipment, object>>[]>()
                ), Times.Once());
        }

        #endregion

        #region GetAsync

        [Xunit.Theory]
        [InlineData("id", "eq", 1)]
        [InlineData("type", "gt", 5)]
        public async Task GetAsync_WhenHaveData_ReturnsSuccessTrue(string field, string op, object value)
        {
            var gridParams = _fixture.CreateGridParamsWithOneFilter(new FilterParam { Field = field, Operator = op, Value = value });
            var repositoryResponse = (
                new List<Equipment>() { new Equipment() }, 
                new ResponsePagination(1, 10, 1)
                );

            var mapperResponseDTOResponse = new EquipmentResponseDTO();

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<Equipment, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
                .Setup(r => r.Map<Equipment, EquipmentResponseDTO>(It.IsAny<Equipment>()))
                .Returns(mapperResponseDTOResponse);

            var response = await _service.GetAsync(gridParams);

            response.Success.Should().BeTrue();
            response.Data.Should().HaveCount(repositoryResponse.Item1.Count());
            response.Error.Should().BeNull();
            response.Data.Should().NotBeNull();

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
               .Verify(r => r.GetAsync(It.Is<GridParams>(p => p == gridParams), It.IsAny<Expression<Func<Equipment, object>>[]>()), Times.Once());

            _mocker.GetMock<IMapper>()
                .Verify(r => r.Map<Equipment, EquipmentResponseDTO>(It.IsAny<Equipment>()), Times.Once());
        }

        [Fact]
        public async Task GetAsync_WhenNotHaveData_ThrowsInfoException()
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();
            var repositoryResponse = (
                new List<Equipment>(), 
                new ResponsePagination(1, 10, 0)
                );

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<Equipment, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            Func<Task> act = async () => await _service.GetAsync(gridParams);

            var exception = await act.Should().ThrowAsync<InfoException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.NotFound);

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Verify(r => r.GetAsync(It.Is<GridParams>(p => p == gridParams), It.IsAny<Expression<Func<Equipment, object>>[]>()), Times.Once);

            _mocker.GetMock<IMapper>()
                .Verify(r => r.Map<Equipment, EquipmentResponseDTO>(It.IsAny<Equipment>()), Times.Never());
        }

        [Xunit.Theory]
        [InlineData("id", "eq", 96)]
        public async Task GetAsync_WhenRepositoryThrowsDatabaseException_PropagatesException(string field, string op, object value)
        {
            var gridParams = _fixture.CreateGridParamsWithOneFilter(new FilterParam { Field = field, Operator = op, Value = value });;
            var repositoryResponse = new DatabaseException("Error message database.");

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<Equipment, object>>[]>()))
                .ThrowsAsync(repositoryResponse);

            Func<Task> act = async () => await _service.GetAsync(gridParams);

            var exception = await act.Should().ThrowAsync<DatabaseException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.DatabaseError);

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Verify(r => r.GetAsync(It.Is<GridParams>(p => p == gridParams), It.IsAny<Expression<Func<Equipment, object>>[]>()), Times.Once());

            _mocker.GetMock<IMapper>()
                .Verify(r => r.Map<Equipment, EquipmentResponseDTO>(It.IsAny<Equipment>()), Times.Never());
        }

        #endregion

        #region InsertAsync

        [Fact]
        public async Task InsertAsync_WhenValidBody_ReturnsSuccessTrue()
        {
            var mapperInsertDTOResponse = new Equipment();
            var mapperResponseDTOResponse = new EquipmentResponseDTO();
            var validationResponse = new ValidationResult();
            var repositoryResponse = new Equipment();
            var serviceRequest = new EquipmentInsertDTO();

            _mocker.GetMock<IMapper>()
                .Setup(r => r.Map<EquipmentInsertDTO, Equipment>(It.IsAny<EquipmentInsertDTO>()))
                .Returns(mapperInsertDTOResponse);

            _mocker.GetMock<IValidatorEntity<Equipment>>()
                .Setup(r => r.ValidateBeforeInsert(It.IsAny<Equipment>()))
                .Returns(validationResponse);

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Setup(r => r.InsertAsync(It.IsAny<Equipment>()))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
               .Setup(r => r.Map<Equipment, EquipmentResponseDTO>(It.IsAny<Equipment>()))
               .Returns(mapperResponseDTOResponse);

            var response = await _service.InsertAsync(serviceRequest);

            response.Success.Should().BeTrue();
            response.Data.Should().BeSameAs(mapperResponseDTOResponse);
            response.Error.Should().BeNull();
            response.Data.Should().NotBeNull();

            _mocker.GetMock<IMapper>()
                .Verify(r => r.Map<EquipmentInsertDTO, Equipment>(It.IsAny<EquipmentInsertDTO>()), Times.Once());

            _mocker.GetMock<IValidatorEntity<Equipment>>()
                .Verify(r => r.ValidateBeforeInsert(It.IsAny<Equipment>()), Times.Once());

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
               .Verify(r => r.InsertAsync(It.IsAny<Equipment>()), Times.Once());

            _mocker.GetMock<IMapper>()
                .Verify(r => r.Map<Equipment, EquipmentResponseDTO>(It.IsAny<Equipment>()), Times.Once());
        }

        [Fact]
        public async Task InsertAsync_WhenInvalidBody_ThrowsValidationException()
        {
            var mapperInsertDTOResponse = new Equipment();
            var validationResponse = new ValidationResult
            {
                Errors = new List<ValidationFailure> { new ValidationFailure("Property", "Error message validation.") }
            };
            var serviceRequest = new EquipmentInsertDTO();

            _mocker.GetMock<IMapper>()
                .Setup(r => r.Map<EquipmentInsertDTO, Equipment>(It.IsAny<EquipmentInsertDTO>()))
                .Returns(mapperInsertDTOResponse);

            _mocker.GetMock<IValidatorEntity<Equipment>>()
                .Setup(r => r.ValidateBeforeInsert(It.IsAny<Equipment>()))
                .Returns(validationResponse);

            Func<Task> act = async () => await _service.InsertAsync(serviceRequest);

            var exception = await act.Should().ThrowAsync<ValidationException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.ValidationFailed);

            _mocker.GetMock<IMapper>()
                .Verify(r => r.Map<EquipmentInsertDTO, Equipment>(It.IsAny<EquipmentInsertDTO>()), Times.Once());

            _mocker.GetMock<IValidatorEntity<Equipment>>()
                .Verify(r => r.ValidateBeforeInsert(It.IsAny<Equipment>()), Times.Once());

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
               .Verify(r => r.InsertAsync(It.IsAny<Equipment>()), Times.Never());

            _mocker.GetMock<IMapper>()
                .Verify(r => r.Map<Equipment, EquipmentResponseDTO>(It.IsAny<Equipment>()), Times.Never());
        }

        [Fact]
        public async Task InsertAsync_WhenRepositoryThrowsDatabaseException_PropagatesException()
        {
            var mapperInsertDTOResponse = new Equipment();
            var repositoryResponse = new DatabaseException("Error message database.");
            var validationResponse = new ValidationResult();
            var serviceRequest = new EquipmentInsertDTO();

            _mocker.GetMock<IMapper>()
                .Setup(r => r.Map<EquipmentInsertDTO, Equipment>(It.IsAny<EquipmentInsertDTO>()))
                .Returns(mapperInsertDTOResponse);

            _mocker.GetMock<IValidatorEntity<Equipment>>()
                .Setup(r => r.ValidateBeforeInsert(It.IsAny<Equipment>()))
                .Returns(validationResponse);

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Setup(r => r.InsertAsync(It.IsAny<Equipment>()))
                .ThrowsAsync(repositoryResponse);

            Func<Task> act = async () => await _service.InsertAsync(serviceRequest);

            var exception = await act.Should().ThrowAsync<DatabaseException>(); 
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.DatabaseError);

            _mocker.GetMock<IMapper>()
                .Verify(r => r.Map<EquipmentInsertDTO, Equipment>(It.IsAny<EquipmentInsertDTO>()), Times.Once());

            _mocker.GetMock<IValidatorEntity<Equipment>>()
                .Verify(r => r.ValidateBeforeInsert(It.IsAny<Equipment>()), Times.Once());

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
               .Verify(r => r.InsertAsync(It.IsAny<Equipment>()), Times.Once());

            _mocker.GetMock<IMapper>()
                .Verify(r => r.Map<Equipment, EquipmentResponseDTO>(It.IsAny<Equipment>()), Times.Never());
        }

        #endregion

        #region UpdateAsync

        [Xunit.Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(6)]
        public async Task UpdateAsync_WhenValidParameterAndBody_ReturnsSuccessTrue(long id)
        {
            var mapperUpdateDTOResponse = new Equipment();
            var mapperResponseDTOResponse = new EquipmentResponseDTO();
            var validationResponse = new ValidationResult();
            var repositoryResponse = new Equipment();
            var serviceRequest = new EquipmentUpdateDTO();

            _mocker.GetMock<IMapper>()
                .Setup(r => r.Map<EquipmentUpdateDTO, Equipment>(It.IsAny<EquipmentUpdateDTO>()))
                .Returns(mapperUpdateDTOResponse);

            _mocker.GetMock<IValidatorEntity<Equipment>>()
                .Setup(r => r.ValidateBeforeUpdate(It.IsAny<Equipment>()))
                .Returns(validationResponse);

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Setup(r => r.UpdateAsync(It.IsAny<long>(),It.IsAny<Equipment>()))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
              .Setup(r => r.Map<Equipment, EquipmentResponseDTO>(It.IsAny<Equipment>()))
              .Returns(mapperResponseDTOResponse);

            var response = await _service.UpdateAsync(id, serviceRequest);

            response.Success.Should().BeTrue();
            response.Data.Should().BeSameAs(mapperResponseDTOResponse);
            response.Error.Should().BeNull();
            response.Data.Should().NotBeNull();

            _mocker.GetMock<IMapper>()
                .Verify(r => r.Map<EquipmentUpdateDTO, Equipment>(It.IsAny<EquipmentUpdateDTO>()), Times.Once());

            _mocker.GetMock<IValidatorEntity<Equipment>>()
                .Verify(r => r.ValidateBeforeUpdate(It.IsAny<Equipment>()), Times.Once());

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
               .Verify(r => r.UpdateAsync(It.Is<long>(p => p == id), It.IsAny<Equipment>()), Times.Once());

            _mocker.GetMock<IMapper>()
                .Verify(r => r.Map<Equipment, EquipmentResponseDTO>(It.IsAny<Equipment>()), Times.Once());
        }

        [Xunit.Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(6)]
        public async Task UpdateAsync_WhenValidParameterAndInvalidBody_ThrowsValidationException(long id)
        {
            var mapperUpdateDTOResponse = new Equipment();
            var validationResponse = new ValidationResult
            {
                Errors = new List<ValidationFailure> { new ValidationFailure("Property", "Error message validation.") }
            };
            var serviceRequest = new EquipmentUpdateDTO();

            _mocker.GetMock<IMapper>()
                .Setup(r => r.Map<EquipmentUpdateDTO, Equipment>(It.IsAny<EquipmentUpdateDTO>()))
                .Returns(mapperUpdateDTOResponse);

            _mocker.GetMock<IValidatorEntity<Equipment>>()
                .Setup(r => r.ValidateBeforeUpdate(It.IsAny<Equipment>()))
                .Returns(validationResponse);

            Func<Task> act = async () => await _service.UpdateAsync(id, serviceRequest);

            var exception = await act.Should().ThrowAsync<ValidationException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.ValidationFailed);

            _mocker.GetMock<IMapper>()
                .Verify(r => r.Map<EquipmentUpdateDTO, Equipment>(It.IsAny<EquipmentUpdateDTO>()), Times.Once());

            _mocker.GetMock<IValidatorEntity<Equipment>>()
                .Verify(r => r.ValidateBeforeUpdate(It.IsAny<Equipment>()), Times.Once());

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
               .Verify(r => r.UpdateAsync(It.Is<long>(p => p == id), It.IsAny<Equipment>()), Times.Never());

            _mocker.GetMock<IMapper>()
                .Verify(r => r.Map<Equipment, EquipmentResponseDTO>(It.IsAny<Equipment>()), Times.Never());
        }

        [Xunit.Theory]
        [InlineData(0)]
        public async Task UpdateAsync_WhenInvalidParameterAndValidBody_ThrowsValidationException(long id)
        {
            var mapperUpdateDTOResponse = new Equipment();
            var validationResponse = new ValidationResult
            {
                Errors = new List<ValidationFailure> { new ValidationFailure("Id", "Id Error message validation.") }
            };
            var serviceRequest = new EquipmentUpdateDTO();

            _mocker.GetMock<IMapper>()
                .Setup(r => r.Map<EquipmentUpdateDTO, Equipment>(It.IsAny<EquipmentUpdateDTO>()))
                .Returns(mapperUpdateDTOResponse);

            _mocker.GetMock<IValidatorEntity<Equipment>>()
                .Setup(r => r.ValidateBeforeUpdate(It.IsAny<Equipment>()))
                .Returns(validationResponse);

            Func<Task> act = async () => await _service.UpdateAsync(id, serviceRequest);

            var exception = await act.Should().ThrowAsync<ValidationException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.ValidationFailed);

            _mocker.GetMock<IMapper>()
                .Verify(r => r.Map<EquipmentUpdateDTO, Equipment>(It.IsAny<EquipmentUpdateDTO>()), Times.Once());

            _mocker.GetMock<IValidatorEntity<Equipment>>()
                .Verify(r => r.ValidateBeforeUpdate(It.IsAny<Equipment>()), Times.Once());

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
               .Verify(r => r.UpdateAsync(It.Is<long>(p => p == id), It.IsAny<Equipment>()), Times.Never());

            _mocker.GetMock<IMapper>()
                .Verify(r => r.Map<Equipment, EquipmentResponseDTO>(It.IsAny<Equipment>()), Times.Never());
        }

        [Xunit.Theory]
        [InlineData(1)]
        public async Task UpdateAsync_WhenRepositoryThrowsDatabaseException_PropagatesException(long id)
        {
            var mapperUpdateDTOResponse = new Equipment();
            var repositoryResponse = new DatabaseException("Error message database.");
            var validationResponse = new ValidationResult();
            var serviceRequest = new EquipmentUpdateDTO();

            _mocker.GetMock<IMapper>()
                .Setup(r => r.Map<EquipmentUpdateDTO, Equipment>(It.IsAny<EquipmentUpdateDTO>()))
                .Returns(mapperUpdateDTOResponse);

            _mocker.GetMock<IValidatorEntity<Equipment>>()
                .Setup(r => r.ValidateBeforeUpdate(It.IsAny<Equipment>()))
                .Returns(validationResponse);

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Setup(r => r.UpdateAsync(It.IsAny<long>(), It.IsAny<Equipment>()))
                .ThrowsAsync(repositoryResponse);

            Func<Task> act = async () => await _service.UpdateAsync(id, serviceRequest);

            var exception = await act.Should().ThrowAsync<DatabaseException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.DatabaseError);

            _mocker.GetMock<IMapper>()
                .Verify(r => r.Map<EquipmentUpdateDTO, Equipment>(It.IsAny<EquipmentUpdateDTO>()), Times.Once());

            _mocker.GetMock<IValidatorEntity<Equipment>>()
                .Verify(r => r.ValidateBeforeUpdate(It.IsAny<Equipment>()), Times.Once());

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
               .Verify(r => r.UpdateAsync(It.Is<long>(p => p == id), It.IsAny<Equipment>()), Times.Once());

            _mocker.GetMock<IMapper>()
                .Verify(r => r.Map<Equipment, EquipmentResponseDTO>(It.IsAny<Equipment>()), Times.Never());
        }

        #endregion
    }
}
