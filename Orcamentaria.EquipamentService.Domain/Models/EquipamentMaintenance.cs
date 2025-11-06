namespace Orcamentaria.EquipamentService.Domain.Models
{
    public class EquipamentMaintenance
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public long EquipamentId { get; set; }
        public Equipament Equipament { get; set; }
        public long CompanyId { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
    }
}
