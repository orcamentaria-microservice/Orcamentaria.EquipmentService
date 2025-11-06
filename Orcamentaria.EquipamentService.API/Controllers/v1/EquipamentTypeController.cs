using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orcamentaria.EquipamentService.Domain.DTOs.Equipament;
using Orcamentaria.EquipamentService.Domain.Services;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;

namespace Orcamentaria.EquipamentService.API.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EquipamentTypeController : Controller
    {
        private readonly IEquipamentTypeService _service;

        public EquipamentTypeController(IEquipamentTypeService service)
        {
            _service = service;
        }

        [Authorize(Roles = "MASTER,EQUIPAMENT:READ")]
        [HttpPost("Get", Name = "EquipamentTypeGet")]
        public async Task<Response<IEnumerable<EquipamentTypeResponseDTO>>?> GetAsync([FromBody] GridParams gridParams)
        {
            try
            {
                return await _service.GetAsync(gridParams);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "MASTER,EQUIPAMENT:INSERT")]
        [HttpPost("Insert", Name = "EquipamentTypeInsert")]
        public async Task<Response<EquipamentTypeResponseDTO>> InsertAsync([FromBody] EquipamentTypeInsertDTO dto)
        {
            try
            {
                return await _service.InsertAsync(dto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "MASTER,EQUIPAMENT:UPDATE")]
        [HttpPut("Update/{id}", Name = "EquipamentTypeUpdate")]
        public async Task<Response<EquipamentTypeResponseDTO>> UpdateAsync(long id, [FromBody] EquipamentTypeUpdateDTO dto)
        {
            try
            {
                return await _service.UpdateAsync(id, dto);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
