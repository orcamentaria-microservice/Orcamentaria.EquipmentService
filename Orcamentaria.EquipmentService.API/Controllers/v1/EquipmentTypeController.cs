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
    public class EquipmentTypeController : Controller
    {
        private readonly IEquipmentTypeService _service;

        public EquipmentTypeController(IEquipmentTypeService service)
        {
            _service = service;
        }

        [Authorize(Roles = "MASTER,EQUIPMENT:READ")]
        [HttpPost("Get", Name = "EquipmentTypeGet")]
        public async Task<Response<IEnumerable<EquipmentTypeResponseDTO>>?> GetAsync([FromBody] GridParams gridParams)
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
        [HttpPost("Insert", Name = "EquipmentTypeInsert")]
        public async Task<Response<EquipmentTypeResponseDTO>> InsertAsync([FromBody] EquipmentTypeInsertDTO dto)
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
        [HttpPut("Update/{id}", Name = "EquipmentTypeUpdate")]
        public async Task<Response<EquipmentTypeResponseDTO>> UpdateAsync(long id, [FromBody] EquipmentTypeUpdateDTO dto)
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
