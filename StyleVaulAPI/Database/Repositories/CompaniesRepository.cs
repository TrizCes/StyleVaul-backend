using StyleVaulAPI.Dto.Companies.Request;
using StyleVaulAPI.Interfaces.Repositories;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Database.Repositories
{
    public class CompaniesRepository : ICompaniesRepository
    {
        public readonly StyleVaulDbContext _dbContext;

        public CompaniesRepository(StyleVaulDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CheckUser(int Id, string email)
        {
            return _dbContext.Companies
                .ToList()
                .Any(
                    c =>
                        c.Id != Id
                        && c.Email.Equals(email)
                );
        }

        public async Task<bool> CreateAsync(Company company)
        {
            await _dbContext.Companies.AddAsync(company);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Company?> UpdateAsync(int id, PutCompanies company)
        {
            var dbCompany = _dbContext.Companies.Find(id);

            if (dbCompany == null)
                return null;

            _dbContext.Entry(dbCompany).CurrentValues.SetValues(company);

            await _dbContext.SaveChangesAsync();
            return dbCompany;
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            var company = await GetByIdAsync(Id);
            if (company != null)
            {
                _dbContext.Companies.Remove(company);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Company> GetByIdAsync(int Id)
        {
            return await _dbContext.Companies.FindAsync(Id);
        }

        public async Task<bool> CheckEmailAsync(int id, string email)
        {
            return _dbContext.Companies
                .ToList()
                .Any(
                    c =>
                        c.Id != id
                        && c.Email.Equals(email)
                );
        }

        public async Task<bool> CheckCnpjAsync(int id, string cnpj)
        {
            return _dbContext.Companies
                .ToList()
                .Any(
                    c =>
                        c.Id != id
                        && c.Cnpj.Equals(cnpj)
                );
        }
    }
}