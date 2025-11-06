using AutoMapper;
using Orcamentaria.EquipamentService.Domain.DTOs.Equipament;
using Orcamentaria.EquipamentService.Domain.Models;

namespace Orcamentaria.EquipamentService.Domain.Mappers
{
    public class EquipamentTypeMapper : Profile
    {
        public EquipamentTypeMapper() 
        {
            CreateMap<EquipamentTypeResponseDTO, EquipamentType>()
                .ForMember(s => s.Id, opt => opt.MapFrom(d => d.Id))
                .ForMember(s => s.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.Active, opt => opt.MapFrom(d => d.Active))
                .ForMember(s => s.CreatedAt, opt => opt.MapFrom(d => d.CreatedAt))
                .ForMember(s => s.CreatedBy, opt => opt.MapFrom(d => d.CreatedBy))
                .ForMember(s => s.UpdatedAt, opt => opt.MapFrom(d => d.UpdatedAt))
                .ForMember(s => s.UpdatedBy, opt => opt.MapFrom(d => d.UpdatedBy))
                .ReverseMap();

            CreateMap<EquipamentType, EquipamentTypeInsertDTO>()
                .ForMember(s => s.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.Active, opt => opt.MapFrom(d => d.Active))
                .ReverseMap();

            CreateMap<EquipamentType, EquipamentTypeUpdateDTO>()
                .ForMember(s => s.Name, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.Active, opt => opt.MapFrom(d => d.Active))
                .ReverseMap();
        }
    }
}
