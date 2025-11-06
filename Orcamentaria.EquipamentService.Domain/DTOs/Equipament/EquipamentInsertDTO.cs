using Orcamentaria.EquipamentService.Domain.Enums;

namespace Orcamentaria.EquipamentService.Domain.DTOs.Equipament
{
    public class EquipamentInsertDTO
    {
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Manufacturer { get; set; }
        public long TypeId { get; set; }
        public MaintenancePeriodEnum MaintenancePeriod { get; set; }
        public bool Active { get; set; }
    }
}
