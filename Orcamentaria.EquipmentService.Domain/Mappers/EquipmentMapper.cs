using AutoMapper;
using Orcamentaria.EquipmentService.Domain.DTOs.Equipment;
using Orcamentaria.EquipmentService.Domain.Models;

namespace Orcamentaria.EquipmentService.Domain.Mappers
{
    public class EquipmentMapper : Profile
    {
        public EquipmentMapper() 
        {
            CreateMap<Equipment, EquipmentResponseDTO>()
                .ForMember(s => s.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.Description, opt => opt.MapFrom(d => d.Description))
                .ForMember(s => s.Manufacturer, opt => opt.MapFrom(d => d.Manufacturer))
                .ForMember(s => s.MaintenancePeriod, opt => opt.MapFrom(d => d.MaintenancePeriod))
                .ForMember(s => s.Type, opt => opt.MapFrom(d => d.Type))
                .ForMember(s => s.Active, opt => opt.MapFrom(d => d.Active))
                .ForMember(s => s.CreatedAt, opt => opt.MapFrom(d => d.CreatedAt))
                .ForMember(s => s.CreatedBy, opt => opt.MapFrom(d => d.CreatedBy))
                .ForMember(s => s.UpdatedAt, opt => opt.MapFrom(d => d.UpdatedAt))
                .ForMember(s => s.UpdatedBy, opt => opt.MapFrom(d => d.UpdatedBy))
                .ReverseMap();

            CreateMap<Equipment, EquipmentInsertDTO>()
                .ForMember(s => s.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.Description, opt => opt.MapFrom(d => d.Description))
                .ForMember(s => s.Manufacturer, opt => opt.MapFrom(d => d.Manufacturer))
                .ForMember(s => s.TypeId, opt => opt.MapFrom(d => d.TypeId))
                .ForMember(s => s.MaintenancePeriod, opt => opt.MapFrom(d => d.MaintenancePeriod))
                .ForMember(s => s.Active, opt => opt.MapFrom(d => d.Active))
                .ReverseMap();

            CreateMap<Equipment, EquipmentUpdateDTO>()
                .ForMember(s => s.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.Description, opt => opt.MapFrom(d => d.Description))
                .ForMember(s => s.Manufacturer, opt => opt.MapFrom(d => d.Manufacturer))
                .ForMember(s => s.TypeId, opt => opt.MapFrom(d => d.TypeId))
                .ForMember(s => s.MaintenancePeriod, opt => opt.MapFrom(d => d.MaintenancePeriod))
                .ForMember(s => s.Active, opt => opt.MapFrom(d => d.Active))
                .ReverseMap();
        }
    }
}
