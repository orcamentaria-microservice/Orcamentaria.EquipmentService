namespace Orcamentaria.EquipamentService.Domain.DTOs.Equipament
{
    public class EquipamentTypeResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }
}
