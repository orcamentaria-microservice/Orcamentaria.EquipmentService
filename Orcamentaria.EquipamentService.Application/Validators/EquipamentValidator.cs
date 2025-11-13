using FluentValidation;
using FluentValidation.Results;
using Orcamentaria.EquipamentService.Domain.Enums;
using Orcamentaria.EquipamentService.Domain.Models;
using Orcamentaria.EquipamentService.Domain.Repositories;
using Orcamentaria.EquipamentService.Domain.Services;
using Orcamentaria.Lib.Domain.Validators;

namespace Orcamentaria.EquipamentService.Application.Validators
{
    public class EquipamentValidator : AbstractValidator<Equipament>, IValidatorEntity<Equipament>
    {
        private readonly IEquipamentRepository _repository;
        private readonly IEquipamentTypeService _equipamentTypeService;

        public EquipamentValidator(
            IEquipamentRepository repository,
            IEquipamentTypeService equipamentTypeService)
        {
            _repository = repository;
            _equipamentTypeService = equipamentTypeService;
        }

        public void CommonValidation(Equipament entity) 
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
                    var type = _equipamentTypeService.GetByIdAsync(x.TypeId).GetAwaiter().GetResult();
                    return type is not null;
                }).WithMessage("O tipo informado não existe.");
            RuleFor(x => x.MaintenancePeriod)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .Must(x => Enum.IsDefined(typeof(MaintenancePeriodEnum), x)).WithMessage("O {PropertyName} é inválido.");
        }

        public ValidationResult ValidateBeforeInsert(Equipament entity)
        {
            CommonValidation(entity);

            RuleFor(x => x.Id)
                .Empty().WithMessage("O {PropertyName} não deve ser informado.");

            return Validate(entity);
        }

        public ValidationResult ValidateBeforeUpdate(Equipament entity)
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
