using Orcamentaria.EquipamentService.Domain.Enums;
using Orcamentaria.Lib.Domain.Entities;

namespace Orcamentaria.EquipamentService.Domain.Models
{
    public class Equipament : TenantEntity
    {
        public long Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Manufacturer { get; set; }
        public long TypeId { get; set; }
        public EquipamentType Type { get; set; }
        public bool Active { get; set; }
        public MaintenancePeriodEnum MaintenancePeriod { get; set; }
    }
}
