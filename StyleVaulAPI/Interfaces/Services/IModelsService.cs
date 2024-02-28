using StyleVaulAPI.Dto.Models.Request;
using StyleVaulAPI.Dto.Models.Response;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Interfaces.Services
{
    public interface IModelsService
    {
        Task<int> CreateAsync(PostModels model, int companyId);
        Task<bool> UpdateAsync(PutModels modelDto, int id, int companyId);
        Task<bool> DeleteAsync(int id, int companyId);
        Task<Model> GetByIdAsync(int id, int companyId);
        Task<List<ModelResponse>> GetAllAsync(int companyId);
    }
}
