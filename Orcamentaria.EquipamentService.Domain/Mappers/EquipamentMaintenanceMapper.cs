using AutoMapper;
using Orcamentaria.EquipamentService.Domain.DTOs.Equipament;
using Orcamentaria.EquipamentService.Domain.Models;

namespace Orcamentaria.EquipamentService.Domain.Mappers
{
    public class EquipamentMaintenanceMapper : Profile
    {
        public EquipamentMaintenanceMapper() 
        {
            CreateMap<EquipamentMaintenanceResponseDTO, EquipamentMaintenance>()
                .ForMember(s => s.Id, opt => opt.MapFrom(d => d.Id))
                .ForMember(s => s.Description, opt => opt.MapFrom(d => d.Description))
                .ForMember(s => s.Equipament, opt => opt.MapFrom(d => d.Equipament))
                .ForMember(s => s.CreatedAt, opt => opt.MapFrom(d => d.CreatedAt))
                .ForMember(s => s.CreatedBy, opt => opt.MapFrom(d => d.CreatedBy))
                .ReverseMap();

            CreateMap<EquipamentMaintenance, EquipamentMaintenanceInsertDTO>()
                .ForMember(s => s.Description, opt => opt.MapFrom(d => d.Description))
                .ForMember(s => s.EquipamentId, opt => opt.MapFrom(d => d.EquipamentId))
                .ReverseMap();
        }
    }
}
