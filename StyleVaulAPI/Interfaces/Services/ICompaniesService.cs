using StyleVaulAPI.Dto.Companies.Request;
using StyleVaulAPI.Dto.Companies.Response;

namespace StyleVaulAPI.Interfaces.Services
{
    public interface ICompaniesService
    {
        Task<CompaniesResponse> CreateAsync(PostCompanies company);
        Task<CompaniesResponse> UpdateAsync(int id, PutCompanies company);
        Task<bool> DeleteAsync(int id);
        Task<CompaniesResponse> GetByIdAsync(int id);
    }
}
