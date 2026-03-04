using AutoMapper;
using Orcamentaria.EquipmentService.Domain.DTOs.Equipment;
using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.EquipmentService.Domain.Repositories;
using Orcamentaria.EquipmentService.Domain.Services;
using Orcamentaria.EquipmentService.Domain.Services.Internal;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Exceptions;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.Lib.Domain.Validators;

namespace Orcamentaria.EquipmentService.Application.Services
{
    public class EquipmentTypeService : IEquipmentTypeService, IEquipmentTypeInternalService
    {
        private readonly IEquipmentTypeRepository<EquipmentType> _repository;
        private readonly IValidatorEntity<EquipmentType> _validator;
        private readonly IMapper _mapper;

        public EquipmentTypeService(
            IEquipmentTypeRepository<EquipmentType> repository,
            IValidatorEntity<EquipmentType> validator,
            IMapper mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<EquipmentType?> GetByIdAsync(long id)
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

        public async Task<Response<IEnumerable<EquipmentTypeResponseDTO>>?> GetAsync(GridParams gridParams)
        {
            try
            {
                var (data, pagination) = await _repository.GetAsync(gridParams);

                if (!data.Any())
                    throw new InfoException($"Nenhum dado foi encontrado.", ErrorCodeEnum.NotFound);

                return new Response<IEnumerable<EquipmentTypeResponseDTO>>(
                        data.Select(x => _mapper.Map<EquipmentType, EquipmentTypeResponseDTO>(x)), pagination);
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

        public async Task<Response<EquipmentTypeResponseDTO>> InsertAsync(EquipmentTypeInsertDTO dto)
        {
            try
            {
                var equipmentType = _mapper.Map<EquipmentTypeInsertDTO, EquipmentType>(dto);

                equipmentType.Name = equipmentType.Name.ToUpper();

                var result = _validator.ValidateBeforeInsert(equipmentType);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.InsertAsync(equipmentType);

                return new Response<EquipmentTypeResponseDTO>(
                    _mapper.Map<EquipmentType, EquipmentTypeResponseDTO>(entity));
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

        public async Task<Response<EquipmentTypeResponseDTO>> UpdateAsync(long id, EquipmentTypeUpdateDTO dto)
        {
            try
            {
                var equipmentType = _mapper.Map<EquipmentTypeUpdateDTO, EquipmentType>(dto);

                equipmentType.Id = id;
                equipmentType.Name = equipmentType.Name.ToUpper();

                var result = _validator.ValidateBeforeUpdate(equipmentType);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.UpdateAsync(id, equipmentType);

                return new Response<EquipmentTypeResponseDTO>(
                    _mapper.Map<EquipmentType, EquipmentTypeResponseDTO>(entity));
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
