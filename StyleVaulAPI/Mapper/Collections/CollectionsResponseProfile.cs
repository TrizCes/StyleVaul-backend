using AutoMapper;
using StyleVaulAPI.Dto.Collections.Response;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Mapper.Collections
{
    public class CollectionsResponseProfile :  Profile
        {
            public CollectionsResponseProfile()
            {
                CreateMap<Collection, CollectionsResponse>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                    .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => MapCompanyName(src)))
                    .ForMember(dest => dest.ResponsibleName, opt => opt.MapFrom(src => MapResponsibleName(src)))
                    .ForMember(dest => dest.ResponsibleId, opt => opt.MapFrom(src => src.ResponsibleId))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                    .ForMember(dest => dest.Budget, opt => opt.MapFrom(src => src.Budget))
                    .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => MapTotalCost(src)))
                    .ForMember(dest => dest.InclusionAt, opt => opt.MapFrom(src => src.InclusionAt))
                    .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src => src.ReleaseYear))
                    .ForMember(dest => dest.Collors, opt => opt.MapFrom(src => src.Collors))
                    .ForMember(dest => dest.Season, opt => opt.MapFrom(src => src.Season))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                    .ReverseMap();
            }

            private static decimal MapTotalCost(Collection collection)
            {
                return collection?.Models?.Sum(x => Convert.ToDecimal(x.RealCost)) ?? 0;
            }

            private static string MapResponsibleName(Collection collection)
            {
                return collection?.Responsible == null
                    ? string.Empty
                    : collection.Responsible.Name;
            }

            private static string MapCompanyName(Collection collection)
            {
                return collection?.Company == null
                    ? string.Empty
                    : collection.Company.Name;
            }
    }
}
