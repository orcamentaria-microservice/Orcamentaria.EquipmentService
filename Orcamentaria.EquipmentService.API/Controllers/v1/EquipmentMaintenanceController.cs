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
    public class EquipmentMaintenanceController : Controller
    {
        private readonly IEquipmentMaintenanceService _service;

        public EquipmentMaintenanceController(IEquipmentMaintenanceService service)
        {
            _service = service;
        }

        [Authorize(Roles = "MASTER,EQUIPMENT:READ")]
        [HttpPost("Get", Name = "EquipmentMaintenanceGet")]
        public async Task<Response<IEnumerable<EquipmentMaintenanceResponseDTO>>?> GetAsync([FromBody] GridParams gridParams)
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
        [HttpPost("Insert", Name = "EquipmentMaintenanceInsert")]
        public async Task<Response<EquipmentMaintenanceResponseDTO>> InsertAsync([FromBody] EquipmentMaintenanceInsertDTO dto)
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
    }
}
