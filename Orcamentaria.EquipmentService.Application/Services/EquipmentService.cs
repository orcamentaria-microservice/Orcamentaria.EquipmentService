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
    public class EquipmentService : IEquipmentService, IEquipmentInternalService
    {
        private readonly IEquipmentRepository<Equipment> _repository;
        private readonly IValidatorEntity<Equipment> _validator;
        private readonly IMapper _mapper;

        public EquipmentService(
            IEquipmentRepository<Equipment> repository,
            IValidatorEntity<Equipment> validator,
            IMapper mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Equipment?> GetByIdAsync(long id)
        {
            try
            {
                return await _repository.GetByIdAsync(id, p => p.Type);
            }
            catch (DefaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnexpectedException(ex.Message, ex);
            }
        }

        public async Task<Response<IEnumerable<EquipmentResponseDTO>>?> GetAsync(GridParams gridParams)
        {
            try
            {                 
                var (data, pagination) = await _repository.GetAsync(gridParams, p => p.Type);

                if (!data.Any())
                    throw new InfoException($"Nenhum dado foi encontrado.", ErrorCodeEnum.NotFound);

                return new Response<IEnumerable<EquipmentResponseDTO>>(
                        data.Select(x => _mapper.Map<Equipment, EquipmentResponseDTO>(x)).ToList(), pagination);
            }
            catch (DefaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnexpectedException(ex.Message, ex);
            }
        }

        public async Task<Response<EquipmentResponseDTO>> InsertAsync(EquipmentInsertDTO dto)
        {
            try 
            {
                var equipment = _mapper.Map<EquipmentInsertDTO, Equipment>(dto);

                var result = _validator.ValidateBeforeInsert(equipment);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.InsertAsync(equipment);

                return new Response<EquipmentResponseDTO>(
                    _mapper.Map<Equipment, EquipmentResponseDTO>(entity));
            }
            catch (DefaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnexpectedException(ex.Message, ex);
            }
        }

        public async Task<Response<EquipmentResponseDTO>> UpdateAsync(long id, EquipmentUpdateDTO dto)
        {
            try
            {
                var equipment = _mapper.Map<EquipmentUpdateDTO, Equipment>(dto);

                equipment.Id = id;

                var result = _validator.ValidateBeforeUpdate(equipment);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.UpdateAsync(id, equipment);

                return new Response<EquipmentResponseDTO>(
                    _mapper.Map<Equipment, EquipmentResponseDTO>(entity));
            }
            catch (DefaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnexpectedException(ex.Message, ex);
            }
        }
    }
}
