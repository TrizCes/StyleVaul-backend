using AutoMapper;
using StyleVaulAPI.Dto.Companies.Request;
using StyleVaulAPI.Dto.Companies.Response;
using StyleVaulAPI.Exceptions;
using StyleVaulAPI.Interfaces.Repositories;
using StyleVaulAPI.Interfaces.Services;
using StyleVaulAPI.Models.Enums;
using StyleVaulAPI.Models;
using StyleVaulAPI.Extensions;

namespace StyleVaulAPI.Services
{
    public class CompaniesService : ICompaniesService
    {
        private readonly ICompaniesRepository _companyRepository;
        private readonly IUsersRepository _userRepository;
        private readonly IMapper _mapper;

        public CompaniesService(
            ICompaniesRepository companyRepository,
            IUsersRepository userRepository,
            IMapper mapper)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<CompaniesResponse> CreateAsync(PostCompanies company)
        {
            if (string.IsNullOrWhiteSpace(company.Name))
                throw new BadRequestException("O campo Empresa não pode ser nulo");

            if (string.IsNullOrWhiteSpace(company.Cnpj))
                throw new BadRequestException("O campo CNPJ não pode ser nulo");

            if (string.IsNullOrWhiteSpace(company.Manager))
                throw new BadRequestException("O campo Nome do Gerente não pode ser nulo");

            if (string.IsNullOrWhiteSpace(company.Email))
                throw new BadRequestException("O campo Email não pode ser nulo");

            if (!company.Email.EmailIsValid())
                throw new BadRequestException("O campo Email deve ser um E-mail válido");

            if (await _companyRepository.CheckEmailAsync(0, company.Email))
                throw new ConflictException("Email já cadastrado");

            if (await _companyRepository.CheckCnpjAsync(0, company.Cnpj))
                throw new ConflictException("CNPJ já cadastrado");

            var entity = _mapper.Map<Company>(company);
            await _companyRepository.CreateAsync(entity);
            var user = new User
            {
                CompanyId = entity.Id,
                Email = company.Email,
                Password = company.Password,
                Role = RoleEnum.Admin,
                Name = company.Manager
            };
            await _userRepository.CreateAsync(user);

            // TODO: implementar junto com CompanySetup

            //var setup = new CompanySetup
            //{
            //    Id = entity.Id,
            //    Logo = string.Empty,
            //    Tema = ThemeEnum.Light
            //};
            //await _setupRepository.CreateAsync(setup);
            return _mapper.Map<CompaniesResponse>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = GetByIdAsync(id);
            if (result == null)
                throw new NotFoundException("Código não existente na base de dados");

            return await _companyRepository.DeleteAsync(id);
        }

        public async Task<CompaniesResponse> GetByIdAsync(int id)
        {
            return _mapper.Map<CompaniesResponse>(await _companyRepository.GetByIdAsync(id));
        }

        public async Task<CompaniesResponse> UpdateAsync(int id, PutCompanies company)
        {
            var exists = await GetByIdAsync(id);
            if (exists == null)
                throw new NotFoundException("Empresa não encontrada");

            if (string.IsNullOrWhiteSpace(company.Name))
                throw new BadRequestException("O campo Empresa não pode ser nulo");

            if (string.IsNullOrWhiteSpace(company.Cnpj))
                throw new BadRequestException("O campo CNPJ não pode ser nulo");

            if (string.IsNullOrWhiteSpace(company.Manager))
                throw new BadRequestException("O campo Nome do Gerente não pode ser nulo");

            if (string.IsNullOrWhiteSpace(company.Email))
                throw new BadRequestException("O campo Email não pode ser nulo");

            if (!company.Email.EmailIsValid())
                throw new BadRequestException("O campo Email deve ser um E-mail válido");

            if (await _companyRepository.CheckEmailAsync(id, company.Email))
                throw new ConflictException("Email já cadastrado");

            return _mapper.Map<CompaniesResponse>(
                await _companyRepository.UpdateAsync(id, company)
            );
        }
    }
}

