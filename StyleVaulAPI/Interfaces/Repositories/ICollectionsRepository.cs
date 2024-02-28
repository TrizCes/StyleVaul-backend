using StyleVaulAPI.Models;

namespace StyleVaulAPI.Interfaces.Repositories
{
    public interface ICollectionsRepository
    {
        Task<bool> CreateAsync(Collection collection);
        Task<bool> UpdateAsync(Collection collection);
        Task<bool> DeleteAsync(int id);
        Task<Collection?> GetByIdAsync(int id, int companyId);
        Task<List<Collection>> GetAllAsync(int companyId);
        Task<bool> CheckNameAsync(int Id, string Name, int companyId);
        Task<List<Collection>> GetExpensiveBudgetsAsync(int companyId, int qty);
        Task<bool> CheckCollectionExistsAsync(int id, int companyId);
    }
}
