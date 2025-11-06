using AutoMapper;
using Orcamentaria.EquipamentService.Domain.DTOs.Equipament;
using Orcamentaria.EquipamentService.Domain.Models;
using Orcamentaria.EquipamentService.Domain.Repositories;
using Orcamentaria.EquipamentService.Domain.Services;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Exceptions;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.Lib.Domain.Validators;
using System;

namespace Orcamentaria.EquipamentService.Application.Services
{
    public class EquipamentService : IEquipamentService
    {
        private readonly IEquipamentRepository _repository;
        private readonly IValidatorEntity<Equipament> _validator;
        private readonly IMapper _mapper;

        public EquipamentService(
            IEquipamentRepository repository,
            IValidatorEntity<Equipament> validator,
            IMapper mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Equipament?> GetByIdAsync(long id)
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

        public async Task<Response<IEnumerable<EquipamentResponseDTO>>?> GetAsync(GridParams gridParams)
        {
            try
            {                 
                var (data, pagination) = await _repository.GetAsync(gridParams, p => p.Type);

                if (!data.Any())
                    throw new InfoException($"Nenhum dado foi encontrado.", ErrorCodeEnum.NotFound);

                return new Response<IEnumerable<EquipamentResponseDTO>>(
                        data.Select(x => _mapper.Map<Equipament, EquipamentResponseDTO>(x)), pagination);
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

        public async Task<Response<EquipamentResponseDTO>> InsertAsync(EquipamentInsertDTO dto)
        {
            try 
            {
                var equipament = _mapper.Map<EquipamentInsertDTO, Equipament>(dto);

                var result = _validator.ValidateBeforeInsert(equipament);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.InsertAsync(equipament);

                return new Response<EquipamentResponseDTO>(
                    _mapper.Map<Equipament, EquipamentResponseDTO>(entity));
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

        public async Task<Response<EquipamentResponseDTO>> UpdateAsync(long id, EquipamentUpdateDTO dto)
        {
            try
            {
                var equipament = _mapper.Map<EquipamentUpdateDTO, Equipament>(dto);

                equipament.Id = id;

                var result = _validator.ValidateBeforeUpdate(equipament);

                if (!result.IsValid)
                    throw new ValidationException(result);

                var entity = await _repository.UpdateAsync(id, equipament);

                return new Response<EquipamentResponseDTO>(
                    _mapper.Map<Equipament, EquipamentResponseDTO>(entity));
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
