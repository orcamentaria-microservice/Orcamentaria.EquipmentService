using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Orcamentaria.EquipmentService.Application.Validators;
using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.EquipmentService.Domain.Repositories;
using Orcamentaria.EquipmentService.Domain.Services.Internal;
using Orcamentaria.EquipmentService.Test.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Orcamentaria.EquipmentService.Test.Validators
{
    [Collection(nameof(EquipmentMaintenanceCollection))]
    public class EquipmentMaintenanceValidatorTest
    {
        private readonly EquipmentMaintenanceFixture _fixture;
        private readonly AutoMocker _mocker;
        private readonly EquipmentMaintenanceValidator _validator;

        public EquipmentMaintenanceValidatorTest(EquipmentMaintenanceFixture fixture)
        {
            _fixture = fixture;
            _mocker = new AutoMocker();
            _validator = _mocker.CreateInstance<EquipmentMaintenanceValidator>(true);
        }

        #region ValidateBeforeInsert
        [Fact]
        public void ValidateBeforeInsert_WhenEntityValid_ReturnsIsValid()
        {
            var entity = _fixture.CreateEntity(0);

            _mocker.GetMock<IEquipmentInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Equipment());

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();

            _mocker.GetMock<IEquipmentInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenIdGreaterThenZero_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(1);

            _mocker.GetMock<IEquipmentInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Equipment());

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Id não deve ser informado.");

            _mocker.GetMock<IEquipmentInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenDescriptionIsEmpty_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            entity.Description = string.Empty;

            _mocker.GetMock<IEquipmentInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Equipment());

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Description é obrigatório.");

            _mocker.GetMock<IEquipmentInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenDescriptionIsNull_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            entity.Description = null;

            _mocker.GetMock<IEquipmentInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Equipment());

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Description é obrigatório.");

            _mocker.GetMock<IEquipmentInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenDescriptionLengthGreaterThan256_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            entity.Description = _fixture.Faker.Random.AlphaNumeric(257);

            _mocker.GetMock<IEquipmentInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new Equipment());

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O tamanho máximo do Description é de 256 caracteres.");

            _mocker.GetMock<IEquipmentInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenEquipmentIdIsEmpty_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            entity.EquipmentId = 0;

            _mocker.GetMock<IEquipmentInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync((Equipment)null);

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Equipment Id é obrigatório.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "O equipamento informado não existe.");
            
            _mocker.GetMock<IEquipmentInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.AtMostOnce());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenEquipmentNotFound_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            entity.EquipmentId = 999;

            _mocker.GetMock<IEquipmentInternalService>()
                .Setup(s => s.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync((Equipment)null);

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O equipamento informado não existe.");

            _mocker.GetMock<IEquipmentInternalService>()
                .Verify(s => s.GetByIdAsync(It.IsAny<long>()), Times.Once());
        }
        #endregion

        #region ValidateBeforeUpdate
        [Fact]
        public void ValidateBeforeUpdate_WhenCalled_ThrowsNotImplementedException()
        {
            var entity = _fixture.CreateEntity(1);

            Action act = () => _validator.ValidateBeforeUpdate(entity);

            act.Should().Throw<NotImplementedException>();
        }
        #endregion
    }
}
