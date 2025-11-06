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
    public class EquipamentMaintenanceService : IEquipamentMaintenanceService
    {
        private readonly IEquipamentMaintenanceRepository _repository;
        private readonly IValidatorEntity<EquipamentMaintenance> _validator;
        private readonly IMapper _mapper;

        public EquipamentMaintenanceService(
            IEquipamentMaintenanceRepository repository,
            IValidatorEntity<EquipamentMaintenance> validator,
            IUserAuthContext userAuthContext,
            IMapper mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<EquipamentMaintenanceResponseDTO>>> GetAsync(GridParams gridParams)
        {
            try 
            {
                var (data, pagination) = await _repository.GetAsync(gridParams,
                p => p.Equipament,
                p => p.Equipament.Type);

                if (!data.Any())
                    throw new InfoException($"Nenhum dado foi encontrado.", ErrorCodeEnum.NotFound);

                return new Response<IEnumerable<EquipamentMaintenanceResponseDTO>>(
                        data.Select(x => _mapper.Map<EquipamentMaintenance, EquipamentMaintenanceResponseDTO>(x)), pagination);
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

        public async Task<Response<EquipamentMaintenanceResponseDTO>> InsertAsync(EquipamentMaintenanceInsertDTO dto)
        {
            try 
            {
                var equipamentMaintenance = _mapper.Map<EquipamentMaintenanceInsertDTO, EquipamentMaintenance>(dto);

                var result = _validator.ValidateBeforeInsert(equipamentMaintenance);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.InsertAsync(equipamentMaintenance);

                return new Response<EquipamentMaintenanceResponseDTO>(
                    _mapper.Map<EquipamentMaintenance, EquipamentMaintenanceResponseDTO>(entity));
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
