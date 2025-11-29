using AutoMapper;
using Orcamentaria.EquipmentService.Domain.DTOs.Equipment;
using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.EquipmentService.Domain.Repositories;
using Orcamentaria.EquipmentService.Domain.Services;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Exceptions;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.Lib.Domain.Validators;

namespace Orcamentaria.EquipmentService.Application.Services
{
    public class EquipmentMaintenanceService : IEquipmentMaintenanceService
    {
        private readonly IEquipmentMaintenanceRepository _repository;
        private readonly IValidatorEntity<EquipmentMaintenance> _validator;
        private readonly IMapper _mapper;

        public EquipmentMaintenanceService(
            IEquipmentMaintenanceRepository repository,
            IValidatorEntity<EquipmentMaintenance> validator,
            IMapper mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<EquipmentMaintenanceResponseDTO>>> GetAsync(GridParams gridParams)
        {
            try 
            {
                var (data, pagination) = await _repository.GetAsync(gridParams,
                p => p.Equipment,
                p => p.Equipment.Type);

                if (!data.Any())
                    throw new InfoException($"Nenhum dado foi encontrado.", ErrorCodeEnum.NotFound);

                return new Response<IEnumerable<EquipmentMaintenanceResponseDTO>>(
                        data.Select(x => _mapper.Map<EquipmentMaintenance, EquipmentMaintenanceResponseDTO>(x)), pagination);
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

        public async Task<Response<EquipmentMaintenanceResponseDTO>> InsertAsync(EquipmentMaintenanceInsertDTO dto)
        {
            try 
            {
                var equipmentMaintenance = _mapper.Map<EquipmentMaintenanceInsertDTO, EquipmentMaintenance>(dto);

                var result = _validator.ValidateBeforeInsert(equipmentMaintenance);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.InsertAsync(equipmentMaintenance);

                return new Response<EquipmentMaintenanceResponseDTO>(
                    _mapper.Map<EquipmentMaintenance, EquipmentMaintenanceResponseDTO>(entity));
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
