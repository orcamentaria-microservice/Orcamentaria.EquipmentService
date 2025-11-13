using AutoMapper;
using Orcamentaria.EquipamentService.Domain.DTOs.Equipament;
using Orcamentaria.EquipamentService.Domain.Models;

namespace Orcamentaria.EquipamentService.Domain.Mappers
{
    public class EquipamentMapper : Profile
    {
        public EquipamentMapper(IMapper mapper) 
        {
            CreateMap<Equipament, EquipamentResponseDTO>()
                .ForMember(s => s.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.Description, opt => opt.MapFrom(d => d.Description))
                .ForMember(s => s.Manufacturer, opt => opt.MapFrom(d => d.Manufacturer))
                .ForMember(s => s.MaintenancePeriod, opt => opt.MapFrom(d => d.MaintenancePeriod))
                .ForMember(s => s.Type, opt => opt.MapFrom(d => mapper.Map<EquipamentType, EquipamentTypeResponseDTO>(d.Type)))
                .ForMember(s => s.Active, opt => opt.MapFrom(d => d.Active))
                .ForMember(s => s.CreatedAt, opt => opt.MapFrom(d => d.CreatedAt))
                .ForMember(s => s.CreatedBy, opt => opt.MapFrom(d => d.CreatedBy))
                .ForMember(s => s.UpdatedAt, opt => opt.MapFrom(d => d.UpdatedAt))
                .ForMember(s => s.UpdatedBy, opt => opt.MapFrom(d => d.UpdatedBy))
                .ReverseMap();

            CreateMap<Equipament, EquipamentInsertDTO>()
                .ForMember(s => s.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.Description, opt => opt.MapFrom(d => d.Description))
                .ForMember(s => s.Manufacturer, opt => opt.MapFrom(d => d.Manufacturer))
                .ForMember(s => s.TypeId, opt => opt.MapFrom(d => d.TypeId))
                .ForMember(s => s.MaintenancePeriod, opt => opt.MapFrom(d => d.MaintenancePeriod))
                .ForMember(s => s.Active, opt => opt.MapFrom(d => d.Active))
                .ReverseMap();

            CreateMap<Equipament, EquipamentUpdateDTO>()
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
