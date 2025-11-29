using Orcamentaria.EquipmentService.Domain.DTOs.Equipment;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;

namespace Orcamentaria.EquipmentService.Domain.Services
{
    public interface IEquipmentMaintenanceService
    {
        Task<Response<IEnumerable<EquipmentMaintenanceResponseDTO>>> GetAsync(GridParams gridParams);
        Task<Response<EquipmentMaintenanceResponseDTO>> InsertAsync(EquipmentMaintenanceInsertDTO dto);
    }
}
