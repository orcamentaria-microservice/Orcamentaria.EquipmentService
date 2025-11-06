using AutoMapper;
using Orcamentaria.EquipamentService.Domain.DTOs.Equipament;
using Orcamentaria.EquipamentService.Domain.Models;
using Orcamentaria.EquipamentService.Domain.Repositories;
using Orcamentaria.EquipamentService.Domain.Services;
using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Exceptions;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.Lib.Domain.Validators;

namespace Orcamentaria.EquipamentService.Application.Services
{
    public class EquipamentTypeService : IEquipamentTypeService
    {
        private readonly IEquipamentTypeRepository _repository;
        private readonly IValidatorEntity<EquipamentType> _validator;
        private readonly IMapper _mapper;

        public EquipamentTypeService(
            IEquipamentTypeRepository repository,
            IValidatorEntity<EquipamentType> validator,
            IMapper mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<EquipamentType?> GetByIdAsync(long id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (DefaultException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Response<IEnumerable<EquipamentTypeResponseDTO>>?> GetAsync(GridParams gridParams)
        {
            try
            {
                var (data, pagination) = await _repository.GetAsync(gridParams);

                if (!data.Any())
                    throw new InfoException($"Nenhum dado foi encontrado.", ErrorCodeEnum.NotFound);

                return new Response<IEnumerable<EquipamentTypeResponseDTO>>(
                        data.Select(x => _mapper.Map<EquipamentType, EquipamentTypeResponseDTO>(x)), pagination);
            }
            catch (DefaultException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Response<EquipamentTypeResponseDTO>> InsertAsync(EquipamentTypeInsertDTO dto)
        {
            try
            {
                var equipamentType = _mapper.Map<EquipamentTypeInsertDTO, EquipamentType>(dto);

                equipamentType.Name = equipamentType.Name.ToUpper();

                var result = _validator.ValidateBeforeInsert(equipamentType);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.InsertAsync(equipamentType);

                return new Response<EquipamentTypeResponseDTO>(
                    _mapper.Map<EquipamentType, EquipamentTypeResponseDTO>(entity));
            }
            catch (DefaultException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Response<EquipamentTypeResponseDTO>> UpdateAsync(long id, EquipamentTypeUpdateDTO dto)
        {
            try
            {
                var equipamentType = _mapper.Map<EquipamentTypeUpdateDTO, EquipamentType>(dto);

                equipamentType.Id = id;
                equipamentType.Name = equipamentType.Name.ToUpper();

                var result = _validator.ValidateBeforeUpdate(equipamentType);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.UpdateAsync(id, equipamentType);

                return new Response<EquipamentTypeResponseDTO>(
                    _mapper.Map<EquipamentType, EquipamentTypeResponseDTO>(entity));
            }
            catch (DefaultException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
