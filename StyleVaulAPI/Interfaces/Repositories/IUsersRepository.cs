using StyleVaulAPI.Dto.Users.Request;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        Task<bool> CreateAsync(User user);
        Task<User?> UpdateAsync(int id, PutUsers user);
        Task<bool> PatchAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<User?> GetByIdAsync(int id);
        Task<List<User>> GetAllAsync(int companyId);
        Task<bool> CheckEmailAsync(int id, string email);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetUserAsync(string Email);
        Task<bool> CheckUserExistsAsync(int responsibleId, int companyId);
        Task<bool> CheckUserCollections(int id);
        Task<bool> CheckUserModels(int id);
        Task<bool> CheckUserRoleAsync(int id);
        Task<bool> CheckChangeUserIdAsync(int id, int changeId);
    }
}
