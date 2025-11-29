namespace Orcamentaria.EquipmentService.Domain.DTOs.Equipment
{
    public class EquipmentMaintenanceResponseDTO
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public EquipmentResponseDTO Equipment { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
    }
}
