using Orcamentaria.EquipmentService.Domain.DTOs.Equipment;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;

namespace Orcamentaria.EquipmentService.Domain.Services
{
    public interface IEquipmentService
    {
        Task<Response<IEnumerable<EquipmentResponseDTO>>?> GetAsync(GridParams gridParams);
        Task<Response<EquipmentResponseDTO>> InsertAsync(EquipmentInsertDTO dto);
        Task<Response<EquipmentResponseDTO>> UpdateAsync(long id, EquipmentUpdateDTO dto);
    }
}
