using AutoMapper;
using StyleVaulAPI.Dto.Collections.Request;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Mapper.Collections
{
    public class PostCollectionsProfile : Profile
    {
        public PostCollectionsProfile()
        {
            CreateMap<PostCollectionsDto, Collection>()
                .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
                .ForMember(dest => dest.ResponsibleId, opt => opt.MapFrom(src => src.ResponsibleId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Budget, opt => opt.MapFrom(src => src.Budget))
                .ForMember(dest => dest.InclusionAt, opt => opt.Ignore())
                .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src => src.ReleaseYear))
                .ForMember(dest => dest.Collors, opt => opt.MapFrom(src => src.Collors))
                .ForMember(dest => dest.Season, opt => opt.MapFrom(src => src.Season))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ReverseMap();
        }
    }
}
