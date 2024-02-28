using StyleVaulAPI.Dto.Users.Request;
using StyleVaulAPI.Interfaces.Repositories;
using StyleVaulAPI.Models.Enums;
using StyleVaulAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace StyleVaulAPI.Database.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        public readonly StyleVaulDbContext _dbContext;

        public UsersRepository(StyleVaulDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<User?> UpdateAsync(int id, PutUsers user)
        {
            var dbUser = _dbContext.Users.Find(id);
            if (dbUser == null)
                return null;

            _dbContext.Entry(dbUser).CurrentValues.SetValues(user);

            await _dbContext.SaveChangesAsync();
            return dbUser;
        }

        public async Task<bool> PatchAsync(User user)
        {
            _dbContext.Users.Update(user);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            var user = await GetByIdAsync(Id);

            if (user == null)
                return false;

            _dbContext.Users.Remove(user);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<User?> GetByIdAsync(int Id)
        {
            throw new NotImplementedException();
            //TODO:
            //return await _dbContext.Users.Include(u => u.Company).FirstOrDefaultAsync(u => u.Id == Id);
        }

        public async Task<List<User>> GetAllAsync(int companyId)
        {
            throw new NotImplementedException();
            //TODO:
            //return await _dbContext.Users.Include(u => u.Company).Where(u => u.CompanyId == companyId).ToListAsync();
        }

        public async Task<bool> CheckEmailAsync(int id, string email)
        {
            return _dbContext.Users
                .ToList()
                .Any(
            c =>
            c.Id != id
                        && c.Email.Equals(email)
                );
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

        public async Task<bool> CheckUserExistsAsync(int responsibleId, int companyId)
        {
            return await _dbContext.Users.AnyAsync(c => c.Id == responsibleId && c.CompanyId == companyId);
        }

        public async Task<bool> CheckUserCollections(int id)
        {
            throw new NotImplementedException();
            //TODO:
            //return await _dbContext.Collections.AnyAsync(c => c.ResponsibleId == id);
        }

        public async Task<bool> CheckUserModels(int id)
        {
            throw new NotImplementedException();
            //TODO:
            //return await _dbContext.Models.AnyAsync(m => m.ResponsibleId == id);
        }

        public Task<bool> CheckUserRoleAsync(int id)
        {
            var user = _dbContext.Users.Find(id);

            return Task.FromResult(user!.Role == RoleEnum.Admin);
        }

        public Task<bool> CheckChangeUserIdAsync(int id, int changeId)
        {
            var user = _dbContext.Users.Find(id);

            if (user == null)
                Task.FromResult(true);

            var changer = _dbContext.Users.Find(changeId);

            if (changer == null)
                Task.FromResult(true);

            if (changer!.Role != RoleEnum.Admin && changer.Role != RoleEnum.Manager)
                Task.FromResult(true);

            if (user!.Role == RoleEnum.Manager && changer.Role != RoleEnum.Admin)
                Task.FromResult(true);

            return Task.FromResult(false);
        }
    }
}