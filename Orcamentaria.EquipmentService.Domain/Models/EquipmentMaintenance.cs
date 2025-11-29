namespace Orcamentaria.EquipmentService.Domain.Models
{
    public class EquipmentMaintenance
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public long EquipmentId { get; set; }
        public Equipment Equipment { get; set; }
        public long CompanyId { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
    }
}
