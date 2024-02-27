using StyleVaulAPI.Dto.Users.Request;
using StyleVaulAPI.Dto.Users.Response;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Interfaces.Services
{
    public interface IUsersService
    {
        Task<UsersResponse> CreateAsync(User user);
        Task<bool> PatchPaswordAsync(int id, PasswordConfirmationRequest passwordConfirmation);
        Task<PutUsers> UpdateAsync(int id, PutUsers user, UsersResponse changer);
        Task<bool> DeleteAsync(int id, int changeUserId);
        Task<UsersResponse> GetByIdAsync(int id);
        Task<List<UsersResponse>> GetAllAsync(int Companyid);
        Task<string> GenerateJwtToken(AuthenticateRequest request);
        int? ValidateJwtToken(string? token);
        Task<bool> GetEmailAsync(string email);
    }
}
