using AutoMapper;
using Orcamentaria.EquipmentService.Domain.DTOs.Equipment;
using Orcamentaria.EquipmentService.Domain.Models;

namespace Orcamentaria.EquipmentService.Domain.Mappers
{
    public class EquipmentTypeMapper : Profile
    {
        public EquipmentTypeMapper() 
        {
            CreateMap<EquipmentTypeResponseDTO, EquipmentType>()
                .ForMember(s => s.Id, opt => opt.MapFrom(d => d.Id))
                .ForMember(s => s.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.Active, opt => opt.MapFrom(d => d.Active))
                .ForMember(s => s.CreatedAt, opt => opt.MapFrom(d => d.CreatedAt))
                .ForMember(s => s.CreatedBy, opt => opt.MapFrom(d => d.CreatedBy))
                .ForMember(s => s.UpdatedAt, opt => opt.MapFrom(d => d.UpdatedAt))
                .ForMember(s => s.UpdatedBy, opt => opt.MapFrom(d => d.UpdatedBy))
                .ReverseMap();

            CreateMap<EquipmentType, EquipmentTypeInsertDTO>()
                .ForMember(s => s.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.Active, opt => opt.MapFrom(d => d.Active))
                .ReverseMap();

            CreateMap<EquipmentType, EquipmentTypeUpdateDTO>()
                .ForMember(s => s.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.Active, opt => opt.MapFrom(d => d.Active))
                .ReverseMap();
        }
    }
}
