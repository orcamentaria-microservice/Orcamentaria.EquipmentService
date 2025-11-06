namespace Orcamentaria.EquipamentService.Domain.DTOs.Equipament
{
    public class EquipamentMaintenanceResponseDTO
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public EquipamentResponseDTO Equipament { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
    }
}
