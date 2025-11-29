using Orcamentaria.Lib.Domain.Entities;

namespace Orcamentaria.EquipmentService.Domain.Models
{
    public class EquipmentType : TenantEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; } = true;
    }
}
