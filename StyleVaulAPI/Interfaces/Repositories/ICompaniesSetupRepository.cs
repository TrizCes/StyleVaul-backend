using StyleVaulAPI.Dto.CompaniesSetup.Request;
using StyleVaulAPI.Dto.CompaniesSetup.Response;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Interfaces.Repositories
{
    public interface ICompaniesSetupRepository
    {
        Task<bool> CreateAsync(CompanySetup companySetup);
        Task<bool> UpdateAsync(CompaniesSetupRequest companySetupRequest, int companyId);
        Task<ThemeResponse> GetColorModeAsync(int companyId);
        Task<LogoResponse> GetImgCompanyAsync(int companyId);
        Task<CompanySetup> GetSetupByCompanyIdAsync(int companyId);
    }
}
