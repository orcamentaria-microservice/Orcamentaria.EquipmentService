using Orcamentaria.EquipamentService.Domain.DTOs.Equipament;
using Orcamentaria.EquipamentService.Domain.Models;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;

namespace Orcamentaria.EquipamentService.Domain.Services
{
    public interface IEquipamentService
    {
        Task<Equipament?> GetByIdAsync(long id);
        Task<Response<IEnumerable<EquipamentResponseDTO>>?> GetAsync(GridParams gridParams);
        Task<Response<EquipamentResponseDTO>> InsertAsync(EquipamentInsertDTO dto);
        Task<Response<EquipamentResponseDTO>> UpdateAsync(long id, EquipamentUpdateDTO dto);
    }
}
