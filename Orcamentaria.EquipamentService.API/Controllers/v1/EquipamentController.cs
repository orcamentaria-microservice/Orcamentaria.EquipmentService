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
    public class EquipamentController : Controller
    {
        private readonly IEquipamentService _service;

        public EquipamentController(IEquipamentService service)
        {
            _service = service;
        }

        [Authorize(Roles = "MASTER,EQUIPAMENT:READ")]
        [HttpPost("Get", Name = "EquipamentGet")]
        public async Task<Response<IEnumerable<EquipamentResponseDTO>>?> GetAsync([FromBody] GridParams gridParams)
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
        [HttpPost("Insert", Name = "EquipamentInsert")]
        public async Task<Response<EquipamentResponseDTO>> InsertAsync([FromBody] EquipamentInsertDTO dto)
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
        [HttpPut("Update/{id}", Name = "EquipamentUpdate")]
        public async Task<Response<EquipamentResponseDTO>> UpdateAsync(long id, [FromBody] EquipamentUpdateDTO dto)
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
