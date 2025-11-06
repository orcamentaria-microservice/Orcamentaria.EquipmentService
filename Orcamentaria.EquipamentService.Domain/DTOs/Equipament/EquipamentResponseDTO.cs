using Orcamentaria.EquipamentService.Domain.Enums;
using Orcamentaria.EquipamentService.Domain.Models;

namespace Orcamentaria.EquipamentService.Domain.DTOs.Equipament
{
    public class EquipamentResponseDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Manufacturer { get; set; }
        public MaintenancePeriodEnum MaintenancePeriod { get; set; }
        public EquipamentType Type { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }
}
