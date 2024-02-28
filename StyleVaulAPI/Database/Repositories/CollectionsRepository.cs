using StyleVaul.Interfaces.Repositories;
using StyleVaul.Models;

namespace StyleVaulAPI.Database.Repositories
{
    public class CollectionsRepository : ICollectionsRepository
    {
        public readonly StyleVaulDbContext _dbContext;

        public CollectionsRepository(StyleVaulDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(Collection collection)
        {
            await _dbContext.Collections.AddAsync(collection);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Collection collection)
        {
            _dbContext.Collections.Update(collection);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var collection = await _dbContext.Collections.FindAsync(id);
            if (collection != null)
            {
                _dbContext.Collections.Remove(collection);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Collection?> GetByIdAsync(int id, int companyId)
        {
            return await _dbContext.Collections
                .Include(c => c.Models)
                .FirstOrDefaultAsync(collection
                    => collection.Id == id
                    && collection.CompanyId == companyId);
        }

        public async Task<List<Collection>> GetAllAsync(int companyId)
        {
            return await _dbContext.Collections
                .Include(c => c.Company)
                .Include(c => c.Responsible)
                .Where(collection => collection.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<bool> CheckNameAsync(int collectionId, string collectionName, int companyId)
        {
            return await _dbContext.Collections
                .AnyAsync(c => c.Id != collectionId
                               && c.Name.Equals(collectionName)
                               && c.CompanyId == companyId);
        }

        public async Task<List<Collection>> GetExpensiveBudgetsAsync(int companyId, int qty)
        {
            var allCollections = await GetAllAsync(companyId);
            var validCollections = allCollections.Where(c => c.Status != Models.Enums.StatusEnum.Archived).ToList();

            validCollections.Sort(
                (collection1, collection2) => (collection2.Budget).CompareTo(collection1.Budget)
            );

            var expensiveCollections = validCollections.Take(qty).ToList();

            return expensiveCollections;
        }

        public Task<bool> CheckCollectionExistsAsync(int id, int companyId)
        {
            return _dbContext.Collections.AnyAsync(c => c.Id == id && c.CompanyId == companyId);
        }
    }
}