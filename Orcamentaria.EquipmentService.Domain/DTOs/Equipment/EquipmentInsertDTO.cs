using Orcamentaria.EquipmentService.Domain.Enums;

namespace Orcamentaria.EquipmentService.Domain.DTOs.Equipment
{
    public class EquipmentInsertDTO
    {
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Manufacturer { get; set; }
        public long TypeId { get; set; }
        public MaintenancePeriodEnum MaintenancePeriod { get; set; }
        public bool Active { get; set; }
    }
}
