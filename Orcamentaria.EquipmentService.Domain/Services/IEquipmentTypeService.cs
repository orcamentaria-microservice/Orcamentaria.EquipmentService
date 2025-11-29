using Orcamentaria.EquipmentService.Domain.DTOs.Equipment;
using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;

namespace Orcamentaria.EquipmentService.Domain.Services
{
    public interface IEquipmentTypeService
    {
        Task<Response<IEnumerable<EquipmentTypeResponseDTO>>?> GetAsync(GridParams gridParams);
        Task<Response<EquipmentTypeResponseDTO>> InsertAsync(EquipmentTypeInsertDTO dto);
        Task<Response<EquipmentTypeResponseDTO>> UpdateAsync(long id, EquipmentTypeUpdateDTO dto);
    }
}
