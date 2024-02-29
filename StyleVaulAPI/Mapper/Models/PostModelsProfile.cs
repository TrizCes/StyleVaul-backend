using AutoMapper;
using StyleVaulAPI.Dto.Models.Request;
using StyleVaulAPI.Extensions;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Mapper.Models
{
    public class PostModelsProfile : Profile
    {
        public PostModelsProfile()
        {
            CreateMap<Model, PostModels>()
                .ForMember(dest => dest.Name, src => src.MapFrom(s => s.Name))
                .ForMember(dest => dest.ResponsibleId, src => src.MapFrom(s => s.ResponsibleId))
                .ForMember(dest => dest.CollectionId, src => src.MapFrom(s => s.CollectionId))
                .ForMember(dest => dest.RealCost, src => src.MapFrom(s => s.RealCost))
                .ForMember(dest => dest.Type, src => src.MapFrom(s => s.Type.GetEnumDescription()))
                .ForMember(dest => dest.Embroidery, src => src.MapFrom(s => s.Embroidery))
                .ForMember(dest => dest.Print, src => src.MapFrom(s => s.Print))
                .ReverseMap();
        }
    }
}
