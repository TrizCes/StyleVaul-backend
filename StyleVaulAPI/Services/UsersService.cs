using Microsoft.IdentityModel.Tokens;
using StyleVaulAPI.Dto.Users.Request;
using StyleVaulAPI.Dto.Users.Response;
using StyleVaulAPI.Interfaces.Repositories;
using StyleVaulAPI.Interfaces.Services;
using StyleVaulAPI.Models;
using StyleVaulAPI.Models.Enums;
using StyleVaulAPI.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StyleVaulAPI.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _repository;
        private readonly ICompaniesRepository _companyRepository;
        private readonly IConfiguration _configuration;

        public UsersService(
            IUsersRepository repository,
            ICompaniesRepository companyRepository,
            IConfiguration configuration
        )
        {
            _repository = repository;
            _companyRepository = companyRepository;
            _configuration = configuration;
        }

        public async Task<UsersResponse> CreateAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
                throw new BadRequestException("Nome não pode ser nulo");

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new BadRequestException("Email não pode ser nulo");

            if (!user.Email.EmailIsValid())
                throw new BadRequestException("O campo Email deve ser um E-mail válido");

            if (await _repository.CheckEmailAsync(user.Id, user.Email))
                throw new ConflictException("Email já cadastrado");

            if (await _companyRepository.CheckUser(user.CompanyId, user.Email))
                throw new ConflictException("Usuário já está vinculado a outra empresa");

            user.Password = "12345678";

            await _repository.CreateAsync(user);

        }

        public async Task<bool> DeleteAsync(int id, int changeId)
        {
            if (await _repository.GetByIdAsync(id) == null)
                throw new NotFoundException("Usuário não encontrado");

            if (await _repository.CheckUserCollections(id))
                throw new ConflictException("Existe pelo menos uma coleção vinculada a este usuário");

            if (await _repository.CheckUserModels(id))
                throw new ConflictException("Existe pelo menos um modelo vinculado a este usuário");

            if (await _repository.CheckUserRoleAsync(id))
                throw new BadRequestException("Este usuário não pode ser excluído");

            if (await _repository.CheckChangeUserIdAsync(id, changeId))
                throw new UnauthorizedException("Você não tem permissão para excluir este usuário.");

            return await _repository.DeleteAsync(id);
        }

        public async Task<List<UsersResponse>> GetAllAsync(int companyId)
        {
            return < List < UsersResponse >> (await _repository.GetAllAsync(companyId));
        }

        public async Task<UsersResponse> GetByIdAsync(int id)
        {
            return < UsersResponse > (await _repository.GetByIdAsync(id));
        }
        public async Task<bool> GetEmailAsync(string email)
        {
            var user = await _repository.GetByEmailAsync(email);

            if (user == null)
                return false;

            return true;
        }

        public async Task<PutUsers> UpdateAsync(int id, PutUsers user, UsersResponse changer)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
                throw new BadRequestException("Nome não pode ser nulo");

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new BadRequestException("Email não pode ser nulo");

            if (!user.Email.EmailIsValid())
                throw new BadRequestException("O campo Email deve ser um E-mail válido");

            var dbUser = await _repository.GetByIdAsync(id);
            if (dbUser == null)
                throw new NotFoundException("Usuário não encontrado");

            if (changer.Id == id && dbUser.Role != user.Role)
                throw new BadRequestException("O gerente não pode alterar seu próprio papel");

            if (dbUser.Role == RoleEnum.Admin && user.Role != RoleEnum.Admin)
                throw new BadRequestException("O gerente principal não pode ter o papel alterado");

            await _repository.UpdateAsync(id, user);
            return user;
        }

        public async Task<string> GenerateJwtToken(AuthenticateRequest request)
        {
            if (!request.IsValid)
                throw new BadRequestException("Usuário ou Senha inválidos");

            var user = await _repository.GetUserAsync(request.Email!);

            if (user == null)
                throw new NotFoundException("E-mail não encontrado");

            if (request.Password != user.Password)
                throw new UnauthorizedException("Senha incorreta");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("App:secret")!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim("id", user.Id.ToString(), ClaimValueTypes.Integer32),
                        new Claim("role", Convert.ToInt32(user.Role).ToString(), ClaimValueTypes.Integer32)
                    }
                ),
                Expires = DateTime.UtcNow.AddMinutes(
                    _configuration.GetValue<int>("App:secret-timeout")!
                ),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int? ValidateJwtToken(string? token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("App:secret")!);
            try
            {
                tokenHandler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    },
                    out SecurityToken validatedToken
                );

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                return accountId;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> PatchPaswordAsync(
            int id,
            PasswordConfirmationRequest passwordConfirmation
        )
        {
            var matchesPassword =
                passwordConfirmation.Password == passwordConfirmation.PasswordConfirmation;
            if (!matchesPassword)
                throw new BadRequestException("Senhas diferentes");

            if (passwordConfirmation.Password.Length < 8)
                throw new BadRequestException("Senha deve ter no mínimo 8 dígitos");

            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                throw new ConflictException("Erro ao buscar usuario");

            if (user.Password == passwordConfirmation.Password)
                throw new BadRequestException("A senha não pode ser igual a anterior");

            user.Password = passwordConfirmation.Password;

            return await _repository.PatchAsync(user);
        }
    }
}
