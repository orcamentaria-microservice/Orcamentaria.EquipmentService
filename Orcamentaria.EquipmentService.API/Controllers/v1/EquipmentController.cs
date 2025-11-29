using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orcamentaria.EquipmentService.Domain.DTOs.Equipment;
using Orcamentaria.EquipmentService.Domain.Services;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;

namespace Orcamentaria.EquipmentService.API.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EquipmentController : Controller
    {
        private readonly IEquipmentService _service;

        public EquipmentController(IEquipmentService service)
        {
            _service = service;
        }

        [Authorize(Roles = "MASTER,EQUIPMENT:READ")]
        [HttpPost("Get", Name = "EquipmentGet")]
        public async Task<Response<IEnumerable<EquipmentResponseDTO>>?> GetAsync([FromBody] GridParams gridParams)
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

        [Authorize(Roles = "MASTER,EQUIPMENT:INSERT")]
        [HttpPost("Insert", Name = "EquipmentInsert")]
        public async Task<Response<EquipmentResponseDTO>> InsertAsync([FromBody] EquipmentInsertDTO dto)
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

        [Authorize(Roles = "MASTER,EQUIPMENT:UPDATE")]
        [HttpPut("Update/{id}", Name = "EquipmentUpdate")]
        public async Task<Response<EquipmentResponseDTO>> UpdateAsync(long id, [FromBody] EquipmentUpdateDTO dto)
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
