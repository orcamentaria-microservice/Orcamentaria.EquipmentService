using FluentValidation;
using FluentValidation.Results;
using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.EquipmentService.Domain.Repositories;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Validators;

namespace Orcamentaria.EquipmentService.Application.Validators
{
    public class EquipmentTypeValidator : AbstractValidator<EquipmentType>, IValidatorEntity<EquipmentType>
    {
        private readonly IEquipmentTypeRepository<EquipmentType> _repository;

        public EquipmentTypeValidator(IEquipmentTypeRepository<EquipmentType> repository)
        {
            _repository = repository;
        }

        public void CommonValidation(EquipmentType entity)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .NotNull().WithMessage("O {PropertyName} é obrigatório.")
                .MaximumLength(40).WithMessage("O tamanho máximo do {PropertyName} é de {MaxLength} caracteres.")
                .Must((x, cancellation) =>
                {
                    var gridParams = new GridParams
                    {
                        Filters = new List<FilterParam>
                        {
                            new FilterParam
                            {
                                Field = "Name",
                                Operator = "eq",
                                Value = x.Name,
                            }
                        }
                    };

                    var (data, _) = _repository.GetAsync(gridParams).GetAwaiter().GetResult();

                    if (!data.Any())
                        return true;

                    if (x.Id == 0)
                        return false;

                    if (data.FirstOrDefault(p => p.Id == x.Id) is null)
                        return false;

                    return true;
                })
                .WithMessage("Esse tipo já existe.");
        }

        public ValidationResult ValidateBeforeInsert(EquipmentType entity)
        {
            CommonValidation(entity);

            RuleFor(x => x.Id)
                .Empty().WithMessage("O {PropertyName} não deve ser informado.");

            return Validate(entity);
        }

        public ValidationResult ValidateBeforeUpdate(EquipmentType entity)
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
