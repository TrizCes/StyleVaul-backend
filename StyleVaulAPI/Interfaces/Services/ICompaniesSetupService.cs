using StyleVaulAPI.Dto.CompaniesSetup.Request;
using StyleVaulAPI.Dto.CompaniesSetup.Response;

namespace StyleVaulAPI.Interfaces.Services
{
    public interface ICompaniesSetupService
    {
        Task<bool?> CreateAsync(CompaniesSetupRequest companySetup, int companyId);
        Task<bool> UpdateCompanySetup(CompaniesSetupRequest companySetup, int companyId);
        Task<ThemeResponse> GetColorMode(int companyId);
        Task<LogoResponse> GetImgCompany(int companyId);
    }
}
