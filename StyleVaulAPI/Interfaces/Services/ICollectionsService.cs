using StyleVaulAPI.Dto.Collections.Request;
using StyleVaulAPI.Dto.Collections.Response;

namespace StyleVaulAPI.Interfaces.Services
{
    public interface ICollectionsService
    {
        Task<List<CollectionsResponse>> GetAllAsync(int companyId);
        Task<List<CollectionsResponse>> GetAllValidAsync(int companyId);
        Task<int> CreateAsync(PostCollectionsDto dto, int companyId);
        Task<PutCollectionsDto> UpdateAsync(PutCollectionsDto dto, int companyId);
        Task<bool> DeleteAsync(int id, int companyId);
        Task<CollectionsResponse> GetByIdAsync(int id, int companyId);
        Task<List<CollectionsExpensiveBudgetsResponse>> GetExpensiveBudgetsAsync(int companyId, int qty);
        Task<List<CollectionsBudgetsVsCosts>> GetBudgetsVsCosts(int companyId);
    }
}
