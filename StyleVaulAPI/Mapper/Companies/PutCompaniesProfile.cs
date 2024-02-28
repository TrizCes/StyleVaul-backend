using AutoMapper;
using StyleVaulAPI.Dto.Companies.Request;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Mapper.Companies
{
    public class PutCompaniesProfile : Profile
    {
        public PutCompaniesProfile()
        {
            CreateMap<PutCompanies, Company>().ReverseMap();
        }
    }
}