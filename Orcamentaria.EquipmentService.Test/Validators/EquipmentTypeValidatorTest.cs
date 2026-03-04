using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Orcamentaria.EquipmentService.Application.Validators;
using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.EquipmentService.Domain.Repositories;
using Orcamentaria.EquipmentService.Test.Fixtures;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;
using System.Linq.Expressions;
using Xunit;

namespace Orcamentaria.EquipmentService.Test.Validators
{
    [Collection(nameof(EquipmentTypeCollection))]
    public class EquipmentTypeValidatorTest
    {
        private readonly EquipmentTypeFixture _fixture;
        private readonly AutoMocker _mocker;
        private readonly EquipmentTypeValidator _validator;

        public EquipmentTypeValidatorTest(EquipmentTypeFixture fixture)
        {
            _fixture = fixture;
            _mocker = new AutoMocker();
            _validator = _mocker.CreateInstance<EquipmentTypeValidator>(true); ;
        }

        #region ValidateBeforeInsert
        [Fact]
        public void ValidateBeforeInsert_WhenEntityValid_ReturnsIsValid()
        {
            var entity = _fixture.CreateEntity(0);
            var repositoryResponse = (
                new List<EquipmentType>(),
                new ResponsePagination(1, 10, 0)
                );

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
                ), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenNameAlreadyExists_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            var repositoryResponse = (
                new List<EquipmentType>() { new() },
                new ResponsePagination(1, 10, 0)
                );

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
            result.Errors.Should().Contain(x => x.ErrorMessage == "Esse tipo já existe.");

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
                ), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenIdGreaterThenZero_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(1);
            var repositoryResponse = (
                new List<EquipmentType>(),
                new ResponsePagination(1, 10, 0)
                );

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Id não deve ser informado.");

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
                ), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenNameIsEmpty_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            var repositoryResponse = (
                new List<EquipmentType>(),
                new ResponsePagination(1, 10, 0)
                );

            entity.Name = string.Empty;

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Name é obrigatório.");

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
                ), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenNameIsNull_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            var repositoryResponse = (
                new List<EquipmentType>(),
                new ResponsePagination(1, 10, 0)
                );

            entity.Name = null;

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Name é obrigatório.");

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
                ), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenNameLengthGreaterThan40Caracters_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            var repositoryResponse = (
                new List<EquipmentType>(),
                new ResponsePagination(1, 10, 0)
                );

            entity.Name = _fixture.Faker.Random.AlphaNumeric(41);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
            result.Errors.Should().Contain(x => x.ErrorMessage == "O tamanho máximo do Name é de 40 caracteres.");

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
                ), Times.Once());
        }
        #endregion

        #region ValidateBeforeUpdate
        [Fact]
        public void ValidateBeforeUpdate_WhenEntityValid_ReturnsIsValid()
        {
            var entity = _fixture.CreateEntity(1);
            var repositoryResponse = (
                new List<EquipmentType>(),
                new ResponsePagination(1, 10, 0)
                );

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
                .ReturnsAsync(entity);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var result = _validator.ValidateBeforeUpdate(entity);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
               .Verify(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
                ), Times.Once());

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
                ), Times.Once());
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenIdThanZero_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            var repositoryResponse = (
                new List<EquipmentType>() { },
                new ResponsePagination(1, 10, 0)
                );

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var result = _validator.ValidateBeforeUpdate(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Id deve ser informado.");

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
              .Verify(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
               ), Times.Once());

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
                ), Times.Once());
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenIdNotFound_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(52);
            var repositoryResponse = (
                new List<EquipmentType>() { new() },
                new ResponsePagination(1, 10, 0)
                );

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
                .ReturnsAsync((EquipmentType)null);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var result = _validator.ValidateBeforeUpdate(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
            result.Errors.Should().Contain(x => x.ErrorMessage == "Id não encontrado.");

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
              .Verify(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
               ), Times.Once());

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
                ), Times.Once());
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenNameAlreadyExists_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            var repositoryResponse = (
                new List<EquipmentType>() { new() },
                new ResponsePagination(1, 10, 0)
                );

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var result = _validator.ValidateBeforeUpdate(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
            result.Errors.Should().Contain(x => x.ErrorMessage == "Esse tipo já existe.");

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
              .Verify(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
               ), Times.Once());

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
                ), Times.Once());
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenNameBelongsToSameEntity_ReturnsValid()
        {
            var entity = _fixture.CreateEntity(1);
            var repositoryResponse = (
                new List<EquipmentType>() { entity },
                new ResponsePagination(1, 10, 0)
                );

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
                .ReturnsAsync(entity);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var result = _validator.ValidateBeforeUpdate(entity);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
              .Verify(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
               ), Times.Once());

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
                ), Times.Once());
        }


        [Fact]
        public void ValidateBeforeUpdate_WhenNameIsEmpty_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(1);
            var repositoryResponse = (
                new List<EquipmentType>(),
                new ResponsePagination(1, 10, 0)
                );

            entity.Name = string.Empty;

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
                .ReturnsAsync(entity);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var result = _validator.ValidateBeforeUpdate(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Name é obrigatório.");

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
              .Verify(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
               ), Times.Once());

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
                ), Times.Once());
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenNameIsNull_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(1);
            var repositoryResponse = (
                new List<EquipmentType>(),
                new ResponsePagination(1, 10, 0)
                );

            entity.Name = null;

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
               .Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
               .ReturnsAsync(entity);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var result = _validator.ValidateBeforeUpdate(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Name é obrigatório.");

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
             .Verify(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
              ), Times.Once());

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
                ), Times.Once());
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenNameLengthGreaterThan40Caracters_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(1);
            var repositoryResponse = (
                new List<EquipmentType>(),
                new ResponsePagination(1, 10, 0)
                );

            entity.Name = _fixture.Faker.Random.AlphaNumeric(41);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
               .Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
               .ReturnsAsync(entity);

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var result = _validator.ValidateBeforeUpdate(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
            result.Errors.Should().Contain(x => x.ErrorMessage == "O tamanho máximo do Name é de 40 caracteres.");

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
             .Verify(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
              ), Times.Once());

            _mocker.GetMock<IEquipmentTypeRepository<EquipmentType>>()
                .Verify(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<EquipmentType, object>>[]>()
                ), Times.Once());
        }
        #endregion
    }
}
