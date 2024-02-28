using AutoMapper;
using StyleVaulAPI.Dto.Companies.Request;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Mapper.Companies
{
    public class PostCompaniesProfile : Profile
    {
        public PostCompaniesProfile()
        {
            CreateMap<PostCompanies, Company>().ReverseMap();
        }
    }
}