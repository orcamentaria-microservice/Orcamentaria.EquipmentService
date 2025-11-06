using Orcamentaria.EquipamentService.Domain.DTOs.Equipament;
using Orcamentaria.EquipamentService.Domain.Models;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;

namespace Orcamentaria.EquipamentService.Domain.Services
{
    public interface IEquipamentTypeService
    {
        Task<EquipamentType?> GetByIdAsync(long id);
        Task<Response<IEnumerable<EquipamentTypeResponseDTO>>?> GetAsync(GridParams gridParams);
        Task<Response<EquipamentTypeResponseDTO>> InsertAsync(EquipamentTypeInsertDTO dto);
        Task<Response<EquipamentTypeResponseDTO>> UpdateAsync(long id, EquipamentTypeUpdateDTO dto);
    }
}
