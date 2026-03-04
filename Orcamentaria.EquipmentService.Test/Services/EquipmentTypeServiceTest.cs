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
using Xunit;

namespace Orcamentaria.EquipmentService.Test.Services
{
    [Collection(nameof(EquipmentTypeCollection))]
    public class EquipmentTypeServiceTest
    {
        private readonly EquipmentTypeFixture _fixture;
        private readonly AutoMocker _mocker;
        private readonly Application.Services.EquipmentTypeService _service;

        public EquipmentTypeServiceTest(EquipmentTypeFixture fixture)
        {
            _fixture = fixture;
            _mocker = new AutoMocker();
            _service = _mocker.CreateInstance<Application.Services.EquipmentTypeService>(true);
        }

        #region GetByIdAsync

        [Xunit.Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(6)]
        public async Task GetByIdAsync_WhenHaveData_ReturnsData(long id)
        {
            var repositoryResponse = _fixture.CreateEntity(id);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetByIdAsync(It.Is<long>(p => p == id)))
                .ReturnsAsync(repositoryResponse);

            var response = await _service.GetByIdAsync(id);

            response.Should().NotBeNull();
            response.Should().BeSameAs(repositoryResponse);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetByIdAsync(It.Is<long>(p => p == id)), Times.Once());
        }

        [Xunit.Theory]
        [InlineData(0)]
        public async Task GetByIdAsync_WhenNotHaveData_ReturnsNull(long id)
        {
            EquipmentType repositoryResponse = null;

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetByIdAsync(It.Is<long>(p => p == id)))
                .ReturnsAsync(repositoryResponse);

            var response = await _service.GetByIdAsync(id);

            response.Should().BeNull();

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetByIdAsync(It.Is<long>(p => p == id)), Times.Once());
        }

        [Xunit.Theory]
        [InlineData(87)]
        public async Task GetByIdAsync_WhenRepositoryThrowsDatabaseException_PropagatesException(long id)
        {
            var repositoryException = new DatabaseException("Error message database.");

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetByIdAsync(It.Is<long>(p => p == id)))
                .ThrowsAsync(repositoryException);

            Func<Task> act = async () => await _service.GetByIdAsync(id);

            var exception = await act.Should().ThrowAsync<DatabaseException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.DatabaseError);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetByIdAsync(It.Is<long>(p => p == id)), Times.Once());
        }

        #endregion

        #region GetAsync

        [Fact]
        public async Task GetAsync_WhenHaveData_ReturnsSuccessTrue()
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();
            var repositoryResponse = (
                new List<EquipmentType>() { new EquipmentType { Id = 1, Name = "A" } },
                new ResponsePagination(1, 10, 1)
            );

            var mapperResponseDTO = new EquipmentTypeResponseDTO();

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>()))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EquipmentType, EquipmentTypeResponseDTO>(It.IsAny<EquipmentType>()))
                .Returns(mapperResponseDTO);

            var response = await _service.GetAsync(gridParams);

            response.Success.Should().BeTrue();
            response.Data.Should().HaveCount(repositoryResponse.Item1.Count());
            response.Error.Should().BeNull();
            response.Data.Should().NotBeNull();

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetAsync(It.Is<GridParams>(p => p == gridParams)), Times.Once);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentType, EquipmentTypeResponseDTO>(It.IsAny<EquipmentType>()), Times.Exactly(repositoryResponse.Item1.Count()));
        }

        [Fact]
        public async Task GetAsync_WhenNotHaveData_ThrowsInfoException()
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();
            var repositoryResponse = (
                new List<EquipmentType>(),
                new ResponsePagination(1, 10, 0)
            );

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>()))
                .ReturnsAsync(repositoryResponse);

            Func<Task> act = async () => await _service.GetAsync(gridParams);

            var exception = await act.Should().ThrowAsync<InfoException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.NotFound);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetAsync(It.Is<GridParams>(p => p == gridParams)), Times.Once);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentType, EquipmentTypeResponseDTO>(It.IsAny<EquipmentType>()), Times.Never);
        }

        [Xunit.Theory]
        [InlineData("any")]
        public async Task GetAsync_WhenRepositoryThrowsDatabaseException_PropagatesException(string dummy)
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();
            var repositoryException = new DatabaseException("Error message database.");

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>()))
                .ThrowsAsync(repositoryException);

            Func<Task> act = async () => await _service.GetAsync(gridParams);

            var exception = await act.Should().ThrowAsync<DatabaseException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.DatabaseError);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetAsync(It.Is<GridParams>(p => p == gridParams)), Times.Once);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentType, EquipmentTypeResponseDTO>(It.IsAny<EquipmentType>()), Times.Never);
        }

        #endregion

        #region InsertAsync

        [Fact]
        public async Task InsertAsync_WhenValidBody_ReturnsSuccessTrue_And_UppercasesName()
        {
            var mapperInsertToEntity = new EquipmentType { Name = "lowercase" };
            var expectedUpperName = "LOWERCASE";
            var mapperResponseDTO = new EquipmentTypeResponseDTO();
            var validationResult = new ValidationResult();
            var repositoryResponse = new EquipmentType { Id = 5, Name = expectedUpperName };
            var serviceRequest = new EquipmentTypeInsertDTO { Name = "lowercase" };

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EquipmentTypeInsertDTO, EquipmentType>(It.IsAny<EquipmentTypeInsertDTO>()))
                .Returns(mapperInsertToEntity);

            _mocker.GetMock<IValidatorEntity<EquipmentType>>()
                .Setup(v => v.ValidateBeforeInsert(It.IsAny<EquipmentType>()))
                .Returns(validationResult);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.InsertAsync(It.IsAny<EquipmentType>()))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EquipmentType, EquipmentTypeResponseDTO>(It.IsAny<EquipmentType>()))
                .Returns(mapperResponseDTO);

            var response = await _service.InsertAsync(serviceRequest);

            response.Success.Should().BeTrue();
            response.Data.Should().BeSameAs(mapperResponseDTO);
            response.Error.Should().BeNull();

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentTypeInsertDTO, EquipmentType>(It.IsAny<EquipmentTypeInsertDTO>()), Times.Once);

            // Verify validator received an Equipment with uppercased name
            _mocker.GetMock<IValidatorEntity<EquipmentType>>()
                .Verify(v => v.ValidateBeforeInsert(It.Is<EquipmentType>(e => e.Name == expectedUpperName)), Times.Once);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.InsertAsync(It.IsAny<EquipmentType>()), Times.Once);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentType, EquipmentTypeResponseDTO>(It.IsAny<EquipmentType>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_WhenInvalidBody_ThrowsValidationException()
        {
            var mapperInsertToEntity = new EquipmentType { Name = "lowercase" };
            var validationResult = new ValidationResult
            {
                Errors = new List<ValidationFailure> { new ValidationFailure("Property", "Error message validation.") }
            };
            var serviceRequest = new EquipmentTypeInsertDTO { Name = "lowercase" };

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EquipmentTypeInsertDTO, EquipmentType>(It.IsAny<EquipmentTypeInsertDTO>()))
                .Returns(mapperInsertToEntity);

            _mocker.GetMock<IValidatorEntity<EquipmentType>>()
                .Setup(v => v.ValidateBeforeInsert(It.IsAny<EquipmentType>()))
                .Returns(validationResult);

            Func<Task> act = async () => await _service.InsertAsync(serviceRequest);

            var exception = await act.Should().ThrowAsync<ValidationException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.ValidationFailed);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentTypeInsertDTO, EquipmentType>(It.IsAny<EquipmentTypeInsertDTO>()), Times.Once);

            _mocker.GetMock<IValidatorEntity<EquipmentType>>()
                .Verify(v => v.ValidateBeforeInsert(It.IsAny<EquipmentType>()), Times.Once);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.InsertAsync(It.IsAny<EquipmentType>()), Times.Never);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentType, EquipmentTypeResponseDTO>(It.IsAny<EquipmentType>()), Times.Never);
        }

        [Fact]
        public async Task InsertAsync_WhenRepositoryThrowsDatabaseException_PropagatesException()
        {
            var mapperInsertToEntity = new EquipmentType { Name = "lowercase" };
            var repositoryException = new DatabaseException("Error message database.");
            var validationResult = new ValidationResult();
            var serviceRequest = new EquipmentTypeInsertDTO { Name = "lowercase" };

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EquipmentTypeInsertDTO, EquipmentType>(It.IsAny<EquipmentTypeInsertDTO>()))
                .Returns(mapperInsertToEntity);

            _mocker.GetMock<IValidatorEntity<EquipmentType>>()
                .Setup(v => v.ValidateBeforeInsert(It.IsAny<EquipmentType>()))
                .Returns(validationResult);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.InsertAsync(It.IsAny<EquipmentType>()))
                .ThrowsAsync(repositoryException);

            Func<Task> act = async () => await _service.InsertAsync(serviceRequest);

            var exception = await act.Should().ThrowAsync<DatabaseException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.DatabaseError);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentTypeInsertDTO, EquipmentType>(It.IsAny<EquipmentTypeInsertDTO>()), Times.Once);

            _mocker.GetMock<IValidatorEntity<EquipmentType>>()
                .Verify(v => v.ValidateBeforeInsert(It.IsAny<EquipmentType>()), Times.Once);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.InsertAsync(It.IsAny<EquipmentType>()), Times.Once);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentType, EquipmentTypeResponseDTO>(It.IsAny<EquipmentType>()), Times.Never);
        }

        #endregion

        #region UpdateAsync

        [Xunit.Theory]
        [InlineData(1)]
        [InlineData(3)]
        public async Task UpdateAsync_WhenValidParameterAndBody_ReturnsSuccessTrue_And_UppercasesName(long id)
        {
            var mapperUpdateToEntity = new EquipmentType { Name = "lowercase" };
            var expectedUpperName = "LOWERCASE";
            var mapperResponseDTO = new EquipmentTypeResponseDTO();
            var validationResult = new ValidationResult();
            var repositoryResponse = new EquipmentType { Id = id, Name = expectedUpperName };
            var serviceRequest = new EquipmentTypeUpdateDTO { Name = "lowercase" };

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EquipmentTypeUpdateDTO, EquipmentType>(It.IsAny<EquipmentTypeUpdateDTO>()))
                .Returns(mapperUpdateToEntity);

            _mocker.GetMock<IValidatorEntity<EquipmentType>>()
                .Setup(v => v.ValidateBeforeUpdate(It.IsAny<EquipmentType>()))
                .Returns(validationResult);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.UpdateAsync(It.IsAny<long>(), It.IsAny<EquipmentType>()))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EquipmentType, EquipmentTypeResponseDTO>(It.IsAny<EquipmentType>()))
                .Returns(mapperResponseDTO);

            var response = await _service.UpdateAsync(id, serviceRequest);

            response.Success.Should().BeTrue();
            response.Data.Should().BeSameAs(mapperResponseDTO);
            response.Error.Should().BeNull();

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentTypeUpdateDTO, EquipmentType>(It.IsAny<EquipmentTypeUpdateDTO>()), Times.Once);

            _mocker.GetMock<IValidatorEntity<EquipmentType>>()
                .Verify(v => v.ValidateBeforeUpdate(It.Is<EquipmentType>(e => e.Id == id && e.Name == expectedUpperName)), Times.Once);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.UpdateAsync(It.Is<long>(p => p == id), It.IsAny<EquipmentType>()), Times.Once);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentType, EquipmentTypeResponseDTO>(It.IsAny<EquipmentType>()), Times.Once);
        }

        [Xunit.Theory]
        [InlineData(1)]
        public async Task UpdateAsync_WhenValidParameterAndInvalidBody_ThrowsValidationException(long id)
        {
            var mapperUpdateToEntity = new EquipmentType { Name = "lowercase" };
            var validationResult = new ValidationResult
            {
                Errors = new List<ValidationFailure> { new ValidationFailure("Property", "Error message validation.") }
            };
            var serviceRequest = new EquipmentTypeUpdateDTO { Name = "lowercase" };

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EquipmentTypeUpdateDTO, EquipmentType>(It.IsAny<EquipmentTypeUpdateDTO>()))
                .Returns(mapperUpdateToEntity);

            _mocker.GetMock<IValidatorEntity<EquipmentType>>()
                .Setup(v => v.ValidateBeforeUpdate(It.IsAny<EquipmentType>()))
                .Returns(validationResult);

            Func<Task> act = async () => await _service.UpdateAsync(id, serviceRequest);

            var exception = await act.Should().ThrowAsync<ValidationException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.ValidationFailed);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentTypeUpdateDTO, EquipmentType>(It.IsAny<EquipmentTypeUpdateDTO>()), Times.Once);

            _mocker.GetMock<IValidatorEntity<EquipmentType>>()
                .Verify(v => v.ValidateBeforeUpdate(It.IsAny<EquipmentType>()), Times.Once);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.UpdateAsync(It.IsAny<long>(), It.IsAny<EquipmentType>()), Times.Never);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentType, EquipmentTypeResponseDTO>(It.IsAny<EquipmentType>()), Times.Never);
        }

        [Xunit.Theory]
        [InlineData(0)]
        public async Task UpdateAsync_WhenInvalidParameterAndValidBody_ThrowsValidationException(long id)
        {
            var mapperUpdateToEntity = new EquipmentType { Name = "lowercase" };
            var validationResult = new ValidationResult
            {
                Errors = new List<ValidationFailure> { new ValidationFailure("Id", "Id Error message validation.") }
            };
            var serviceRequest = new EquipmentTypeUpdateDTO { Name = "lowercase" };

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EquipmentTypeUpdateDTO, EquipmentType>(It.IsAny<EquipmentTypeUpdateDTO>()))
                .Returns(mapperUpdateToEntity);

            _mocker.GetMock<IValidatorEntity<EquipmentType>>()
                .Setup(v => v.ValidateBeforeUpdate(It.IsAny<EquipmentType>()))
                .Returns(validationResult);

            Func<Task> act = async () => await _service.UpdateAsync(id, serviceRequest);

            var exception = await act.Should().ThrowAsync<ValidationException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.ValidationFailed);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentTypeUpdateDTO, EquipmentType>(It.IsAny<EquipmentTypeUpdateDTO>()), Times.Once);

            _mocker.GetMock<IValidatorEntity<EquipmentType>>()
                .Verify(v => v.ValidateBeforeUpdate(It.IsAny<EquipmentType>()), Times.Once);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.UpdateAsync(It.IsAny<long>(), It.IsAny<EquipmentType>()), Times.Never);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentType, EquipmentTypeResponseDTO>(It.IsAny<EquipmentType>()), Times.Never);
        }

        [Xunit.Theory]
        [InlineData(1)]
        public async Task UpdateAsync_WhenRepositoryThrowsDatabaseException_PropagatesException(long id)
        {
            var mapperUpdateToEntity = new EquipmentType { Name = "lowercase" };
            var repositoryException = new DatabaseException("Error message database.");
            var validationResult = new ValidationResult();
            var serviceRequest = new EquipmentTypeUpdateDTO { Name = "lowercase" };

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EquipmentTypeUpdateDTO, EquipmentType>(It.IsAny<EquipmentTypeUpdateDTO>()))
                .Returns(mapperUpdateToEntity);

            _mocker.GetMock<IValidatorEntity<EquipmentType>>()
                .Setup(v => v.ValidateBeforeUpdate(It.IsAny<EquipmentType>()))
                .Returns(validationResult);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.UpdateAsync(It.IsAny<long>(), It.IsAny<EquipmentType>()))
                .ThrowsAsync(repositoryException);

            Func<Task> act = async () => await _service.UpdateAsync(id, serviceRequest);

            var exception = await act.Should().ThrowAsync<DatabaseException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.DatabaseError);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentTypeUpdateDTO, EquipmentType>(It.IsAny<EquipmentTypeUpdateDTO>()), Times.Once);

            _mocker.GetMock<IValidatorEntity<EquipmentType>>()
                .Verify(v => v.ValidateBeforeUpdate(It.IsAny<EquipmentType>()), Times.Once);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.UpdateAsync(It.Is<long>(p => p == id), It.IsAny<EquipmentType>()), Times.Once);

            _mocker.GetMock<IMapper>()
                .Verify(m => m.Map<EquipmentType, EquipmentTypeResponseDTO>(It.IsAny<EquipmentType>()), Times.Never);
        }

        #endregion
    }
}
