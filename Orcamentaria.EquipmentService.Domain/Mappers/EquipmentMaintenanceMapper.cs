using AutoMapper;
using Orcamentaria.EquipmentService.Domain.DTOs.Equipment;
using Orcamentaria.EquipmentService.Domain.Models;

namespace Orcamentaria.EquipmentService.Domain.Mappers
{
    public class EquipmentMaintenanceMapper : Profile
    {
        public EquipmentMaintenanceMapper() 
        {
            CreateMap<EquipmentMaintenanceResponseDTO, EquipmentMaintenance>()
                .ForMember(s => s.Id, opt => opt.MapFrom(d => d.Id))
                .ForMember(s => s.Description, opt => opt.MapFrom(d => d.Description))
                .ForMember(s => s.Equipment, opt => opt.MapFrom(d => d.Equipment))
                .ForMember(s => s.CreatedAt, opt => opt.MapFrom(d => d.CreatedAt))
                .ForMember(s => s.CreatedBy, opt => opt.MapFrom(d => d.CreatedBy))
                .ReverseMap();

            CreateMap<EquipmentMaintenance, EquipmentMaintenanceInsertDTO>()
                .ForMember(s => s.Description, opt => opt.MapFrom(d => d.Description))
                .ForMember(s => s.EquipmentId, opt => opt.MapFrom(d => d.EquipmentId))
                .ReverseMap();
        }
    }
}
