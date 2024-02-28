using AutoMapper;
using StyleVaulAPI.Dto.Models.Response;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Mapper.Models
{
    public class ModelsResponseProfile : Profile
    {
        public ModelsResponseProfile()
        {
            CreateMap<Model, ModelResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, src => src.MapFrom(s => s.Name))
                .ForMember(dest => dest.ResponsibleId, src => src.MapFrom(s => s.ResponsibleId))
                .ForMember(dest => dest.CollectionId, src => src.MapFrom(s => s.CollectionId))
                .ForMember(dest => dest.CollectionName, src => src.MapFrom(s => s.Collection.Name))
                .ForMember(dest => dest.RealCost, src => src.MapFrom(s => s.RealCost))
                .ForMember(dest => dest.Type, src => src.MapFrom(s => s.Type))
                .ForMember(dest => dest.Embroidery, src => src.MapFrom(s => s.Embroidery))
                .ForMember(dest => dest.Print, src => src.MapFrom(s => s.Print))
                .ReverseMap();
        }
    }
}
