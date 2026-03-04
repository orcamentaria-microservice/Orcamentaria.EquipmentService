using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Orcamentaria.EquipmentService.Application.Validators;
using Orcamentaria.EquipmentService.Domain.Enums;
using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.EquipmentService.Domain.Repositories;
using Orcamentaria.EquipmentService.Domain.Services.Internal;
using Orcamentaria.EquipmentService.Test.Fixtures;
using Orcamentaria.Lib.Domain.Models.Responses;
using System;
using System.Linq.Expressions;
using Xunit;

namespace Orcamentaria.EquipmentService.Test.Validators
{
    [Collection(nameof(EquipmentCollection))]
    public class EquipmentValidatorTest
    {
        private readonly EquipmentFixture _fixture;
        private readonly AutoMocker _mocker;
        private readonly EquipmentValidator _validator;

        public EquipmentValidatorTest(EquipmentFixture fixture)
        {
            _fixture = fixture;
            _mocker = new AutoMocker();
            _validator = _mocker.CreateInstance<EquipmentValidator>(true);
        }

        #region ValidateBeforeInsert
        [Fact]
        public void ValidateBeforeInsert_WhenEntityValid_ReturnsIsValid()
        {
            var entity = _fixture.CreateEntity(0);

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new EquipmentType());

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenIdGreaterThenZero_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(1);

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new EquipmentType());

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Id não deve ser informado.");

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenNameIsEmpty_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            entity.Name = string.Empty;

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new EquipmentType());

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Name é obrigatório.");

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenNameLengthGreaterThan60Caracters_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            entity.Name = _fixture.Faker.Random.AlphaNumeric(61);

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new EquipmentType());

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
            result.Errors.Should().Contain(x => x.ErrorMessage == "O tamanho máximo do Name é de 60 caracteres.");

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenDescriptionLengthGreaterThan256_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            entity.Description = _fixture.Faker.Random.AlphaNumeric(257);

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new EquipmentType());

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O tamanho máximo do Description é de 256 caracteres.");

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenManufacturerLengthGreaterThan150_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            entity.Manufacturer = _fixture.Faker.Random.AlphaNumeric(151);

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new EquipmentType());

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O tamanho máximo do Manufacturer é de 150 caracteres.");

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenTypeNotFound_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            entity.TypeId = 999;

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync((EquipmentType)null);

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O tipo informado não existe.");

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenMaintenancePeriodInvalid_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            entity.MaintenancePeriod = (MaintenancePeriodEnum)999;

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new EquipmentType());

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Maintenance Period é inválido.");

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }
        #endregion

        #region ValidateBeforeUpdate
        [Fact]
        public void ValidateBeforeUpdate_WhenEntityValid_ReturnsIsValid()
        {
            var entity = _fixture.CreateEntity(1);

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(entity);

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new EquipmentType());

            var result = _validator.ValidateBeforeUpdate(entity);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once());

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenIdThanZero_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync((Equipment)null);

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new EquipmentType());

            var result = _validator.ValidateBeforeUpdate(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Id deve ser informado.");

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once());

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenIdNotFound_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(52);
            entity.Id = 52;

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync((Equipment)null);

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new EquipmentType());

            var result = _validator.ValidateBeforeUpdate(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "Id não encontrado.");

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once());

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenTypeNotFound_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(1);
            entity.Id = 1;
            entity.TypeId = 999;

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(entity);

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync((EquipmentType)null);

            var result = _validator.ValidateBeforeUpdate(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O tipo informado não existe.");

            _mocker.GetMock<IEquipmentRepository<Equipment>>()
                .Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once());

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenMaintenancePeriodInvalid_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(1);
            entity.MaintenancePeriod = (MaintenancePeriodEnum)999;

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new EquipmentType());

            var result = _validator.ValidateBeforeUpdate(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Maintenance Period é inválido.");

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenNameIsEmpty_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(1);
            entity.Name = string.Empty;

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new EquipmentType());

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Name é obrigatório.");

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenNameLengthGreaterThan60Caracters_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(1);
            entity.Name = _fixture.Faker.Random.AlphaNumeric(61);

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new EquipmentType());

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
            result.Errors.Should().Contain(x => x.ErrorMessage == "O tamanho máximo do Name é de 60 caracteres.");

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenDescriptionLengthGreaterThan256_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(1);
            entity.Description = _fixture.Faker.Random.AlphaNumeric(257);

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new EquipmentType());

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O tamanho máximo do Description é de 256 caracteres.");

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenManufacturerLengthGreaterThan150_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(1);
            entity.Manufacturer = _fixture.Faker.Random.AlphaNumeric(151);

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new EquipmentType());

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O tamanho máximo do Manufacturer é de 150 caracteres.");

            _mocker.GetMock<IEquipmentTypeInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }
        #endregion
    }
}
