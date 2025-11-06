using Orcamentaria.EquipamentService.Domain.DTOs.Equipament;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;

namespace Orcamentaria.EquipamentService.Domain.Services
{
    public interface IEquipamentMaintenanceService
    {
        Task<Response<IEnumerable<EquipamentMaintenanceResponseDTO>>> GetAsync(GridParams gridParams);
        Task<Response<EquipamentMaintenanceResponseDTO>> InsertAsync(EquipamentMaintenanceInsertDTO dto);
    }
}
