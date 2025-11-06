using Orcamentaria.Lib.Domain.Entities;

namespace Orcamentaria.EquipamentService.Domain.Models
{
    public class EquipamentType : TenantEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; } = true;
    }
}
