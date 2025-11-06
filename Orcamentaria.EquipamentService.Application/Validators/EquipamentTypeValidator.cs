using FluentValidation;
using FluentValidation.Results;
using Orcamentaria.EquipamentService.Domain.Models;
using Orcamentaria.EquipamentService.Domain.Repositories;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Validators;

namespace Orcamentaria.PersonService.Application.Validators
{
    public class EquipamentTypeValidator : AbstractValidator<EquipamentType>, IValidatorEntity<EquipamentType>
    {
        private readonly IEquipamentTypeRepository _repository;

        public EquipamentTypeValidator(IEquipamentTypeRepository repository)
        {
            _repository = repository;
        }

        public void CommonValidation(EquipamentType entity)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
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

        public ValidationResult ValidateBeforeInsert(EquipamentType entity)
        {
            CommonValidation(entity);

            RuleFor(x => x.Id)
                .Empty().WithMessage("O {PropertyName} não deve ser informado.");

            return Validate(entity);
        }

        public ValidationResult ValidateBeforeUpdate(EquipamentType entity)
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
