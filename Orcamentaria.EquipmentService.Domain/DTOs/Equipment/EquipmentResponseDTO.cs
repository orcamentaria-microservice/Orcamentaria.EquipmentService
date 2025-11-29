using Orcamentaria.EquipmentService.Domain.Enums;
using Orcamentaria.EquipmentService.Domain.Models;

namespace Orcamentaria.EquipmentService.Domain.DTOs.Equipment
{
    public class EquipmentResponseDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Manufacturer { get; set; }
        public MaintenancePeriodEnum MaintenancePeriod { get; set; }
        public EquipmentTypeResponseDTO Type { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }
}
