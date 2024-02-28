using StyleVaulAPI.Models;

namespace StyleVaulAPI.Interfaces.Repositories
{
    public interface IModelsRepository
    {
        Task<bool> CreateAsync(Model model);
        Task<bool> UpdateAsync(Model model);
        Task<bool> DeleteAsync(int id, int companyId);
        Task<Model?> GetByIdAsync(int id, int companyId);
        Task<bool> CheckModelNameAsync(int companyId, string modelName);
        Task<List<Model>> GetAllAsync(int companyId);
        Task<List<Model>> GetAllForCostsAsync(int companyId);
        Task<List<Model>> GetAllByCollectionIdAsync(int collectionId);
        Task<bool> CheckCollectionAsync(int companyId, int collectionId);
        Task<bool> CheckResponsibleAsync(int companyId, int responsibleId);
        Task<bool> CheckModelCompanyAsync(int id, int companyId);
        Task<bool> CheckModelExistsByCollectionAsync(int collectionId);
    }
}
