using FluentValidation;
using FluentValidation.Results;
using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.EquipmentService.Domain.Repositories;
using Orcamentaria.EquipmentService.Domain.Services;
using Orcamentaria.EquipmentService.Domain.Services.Internal;
using Orcamentaria.Lib.Domain.Validators;

namespace Orcamentaria.EquipmentService.Application.Validators
{
    public class EquipmentMaintenanceValidator : AbstractValidator<EquipmentMaintenance>, IValidatorEntity<EquipmentMaintenance>
    {
        private readonly IEquipmentTypeRepository<EquipmentType> _repository;
        private readonly IEquipmentInternalService _equipmentInternalService;

        public EquipmentMaintenanceValidator(
            IEquipmentTypeRepository<EquipmentType> repository,
            IEquipmentInternalService equipmentInternalService)
        {
            _repository = repository;
            _equipmentInternalService = equipmentInternalService;
        }

        public void CommonValidation(EquipmentMaintenance entity)
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .MaximumLength(256).WithMessage("O tamanho máximo do {PropertyName} é de {MaxLength} caracteres.");
            RuleFor(x => x.EquipmentId)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .Must((equipment, cancellation) =>
                {
                    var result = _equipmentInternalService.GetByIdAsync(equipment.Id).GetAwaiter().GetResult();
                    return result is not null;
                }).WithMessage("O equipamento informado não existe.");
        }

        public ValidationResult ValidateBeforeInsert(EquipmentMaintenance entity)
        {
            CommonValidation(entity);

            RuleFor(x => x.Id)
                .Empty().WithMessage("O {PropertyName} não deve ser informado.");

            return Validate(entity);
        }

        public ValidationResult ValidateBeforeUpdate(EquipmentMaintenance entity)
            => throw new NotImplementedException();
    }
}
