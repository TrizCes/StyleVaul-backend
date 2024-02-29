using Microsoft.EntityFrameworkCore;
using StyleVaulAPI.Interfaces.Repositories;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Database.Repositories
{
    public class ModelsRepository : IModelsRepository
    {
        private readonly StyleVaulDbContext _dbContext;

        public ModelsRepository(StyleVaulDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(Model model)
        {
            await _dbContext.Models.AddAsync(model);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Model model)
        {
            _dbContext.Models.Update(model);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id, int companyId)
        {
            var model = await GetByIdAsync(id, companyId);
            if (model == null) return false;

            _dbContext.Models.Remove(model);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<Model>> GetAllAsync(int companyId)
        {
            return await _dbContext.Models.Include(m => m.Collection).Where(m => m.CompanyId == companyId).ToListAsync();
        }

        public async Task<List<Model>> GetAllForCostsAsync(int companyId)
        {
            return await _dbContext.Models
                .Where(model => model.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<Model?> GetByIdAsync(int id, int companyId)
        {
            return await _dbContext.Models.FirstOrDefaultAsync(m => m.Id == id && m.CompanyId == companyId);
        }

        public async Task<List<Model>> GetAllByCollectionIdAsync(int collectionId)
        {
            return await _dbContext.Models
                .Where(model => model.CollectionId == collectionId)
                .ToListAsync();
        }

        public Task<bool> CheckCollectionAsync(int companyId, int collectionId)
        {
            return _dbContext.Collections.AnyAsync(c => c.Id == collectionId && c.CompanyId == companyId && c.Status != Models.Enums.StatusEnum.Archived);
        }

        public Task<bool> CheckModelCompanyAsync(int companyId, int collectionId)
        {
            return _dbContext.Models.AnyAsync(c => c.CollectionId == collectionId && c.CompanyId == companyId);
        }

        public Task<bool> CheckModelExistsByCollectionAsync(int collectionId)
        {
            return _dbContext.Models.AnyAsync(m => m.CollectionId == collectionId);
        }

        public Task<bool> CheckResponsibleAsync(int companyId, int responsibleId)
        {
            return _dbContext.Users.AnyAsync(u => u.Id == responsibleId && u.CompanyId == companyId);
        }

        public async Task<bool> CheckModelNameAsync(int companyId, string modelName)
        {
            return await _dbContext.Models.AnyAsync(m => m.Name == modelName && m.CompanyId == companyId);
        }
    }
}
