using FluentValidation;
using FluentValidation.Results;
using Orcamentaria.EquipamentService.Domain.Models;
using Orcamentaria.EquipamentService.Domain.Repositories;
using Orcamentaria.EquipamentService.Domain.Services;
using Orcamentaria.Lib.Domain.Validators;

namespace Orcamentaria.PersonService.Application.Validators
{
    public class EquipamentMaintenanceValidator : AbstractValidator<EquipamentMaintenance>, IValidatorEntity<EquipamentMaintenance>
    {
        private readonly IEquipamentTypeRepository _repository;
        private readonly IEquipamentService _equipamentService;

        public EquipamentMaintenanceValidator(
            IEquipamentTypeRepository repository,
            IEquipamentService equipamentService)
        {
            _repository = repository;
            _equipamentService = equipamentService;
        }

        public void CommonValidation(EquipamentMaintenance entity)
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .MaximumLength(256).WithMessage("O tamanho máximo do {PropertyName} é de {MaxLength} caracteres.");
            RuleFor(x => x.EquipamentId)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .MustAsync(async (equipamentId, cancellation) =>
                {
                    var equipament = await _equipamentService.GetByIdAsync(equipamentId);
                    return equipament is not null;
                }).WithMessage("O equipamento informado não existe.");
        }

        public ValidationResult ValidateBeforeInsert(EquipamentMaintenance entity)
        {
            RuleFor(x => x.Id)
                .Empty().WithMessage("O {PropertyName} não deve ser informado.");

            return Validate(entity);
        }

        public ValidationResult ValidateBeforeUpdate(EquipamentMaintenance entity)
            => throw new NotImplementedException();
    }
}
