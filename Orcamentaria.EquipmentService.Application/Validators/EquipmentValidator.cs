using FluentValidation;
using FluentValidation.Results;
using Orcamentaria.EquipmentService.Domain.Enums;
using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.EquipmentService.Domain.Repositories;
using Orcamentaria.EquipmentService.Domain.Services;
using Orcamentaria.EquipmentService.Domain.Services.Internal;
using Orcamentaria.Lib.Domain.Validators;

namespace Orcamentaria.EquipmentService.Application.Validators
{
    public class EquipmentValidator : AbstractValidator<Equipment>, IValidatorEntity<Equipment>
    {
        private readonly IEquipmentRepository _repository;
        private readonly IEquipmentTypeInternalService _equipmentTypeInternalService;

        public EquipmentValidator(
            IEquipmentRepository repository,
            IEquipmentTypeInternalService equipmentTypeInternalService)
        {
            _repository = repository;
            _equipmentTypeInternalService = equipmentTypeInternalService;
        }

        public void CommonValidation(Equipment entity) 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .MaximumLength(60).WithMessage("O tamanho máximo do {PropertyName} é de {MaxLength} caracteres.");
            RuleFor(x => x.Description)
                .MaximumLength(256).WithMessage("O tamanho máximo do {PropertyName} é de {MaxLength} caracteres.");
            RuleFor(x => x.Manufacturer)
                .MaximumLength(150).WithMessage("O tamanho máximo do {PropertyName} é de {MaxLength} caracteres.");
            RuleFor(x => x.TypeId)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .Must((x, cancellation) =>
                {
                    var type = _equipmentTypeInternalService.GetByIdAsync(x.TypeId).GetAwaiter().GetResult();
                    return type is not null;
                }).WithMessage("O tipo informado não existe.");
            RuleFor(x => x.MaintenancePeriod)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .Must(x => Enum.IsDefined(typeof(MaintenancePeriodEnum), x)).WithMessage("O {PropertyName} é inválido.");
        }

        public ValidationResult ValidateBeforeInsert(Equipment entity)
        {
            CommonValidation(entity);

            RuleFor(x => x.Id)
                .Empty().WithMessage("O {PropertyName} não deve ser informado.");

            return Validate(entity);
        }

        public ValidationResult ValidateBeforeUpdate(Equipment entity)
        {
            CommonValidation(entity);

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O {PropertyName} deve ser informado.");

            RuleFor(x => x.Id)
               .Must((x, cancelation) =>
               {
                   var entity = _repository.GetByIdAsync(x.Id).GetAwaiter().GetResult();

                   return entity is not null;

               }).WithMessage("Id não encontrado.");

            return Validate(entity);
        }
    }
}
