using Microsoft.EntityFrameworkCore;
using StyleVaulAPI.Dto.CompaniesSetup.Request;
using StyleVaulAPI.Dto.CompaniesSetup.Response;
using StyleVaulAPI.Interfaces.Repositories;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Database.Repositories
{
    public class CompaniesSetupRepository : ICompaniesSetupRepository
    {
        private readonly StyleVaulDbContext _dbContext;

        public CompaniesSetupRepository(StyleVaulDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(CompanySetup companySetup)
        {
            try
            {
                _dbContext.CompaniesSetups.AddAsync(companySetup);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<ThemeResponse> GetColorModeAsync(int companyId)
        {
            var setup = await GetSetupByCompanyIdAsync(companyId);
            ThemeResponse theme = new ThemeResponse(setup.Theme);
            return theme;
        }

        public async Task<LogoResponse> GetImgCompanyAsync(int companyId)
        {
            var setup = await GetSetupByCompanyIdAsync(companyId);
            LogoResponse logo = new LogoResponse(setup.Logo);
            return logo;
        }

        public async Task<CompanySetup> GetSetupByCompanyIdAsync(int companyId)
        {
            return await _dbContext.CompaniesSetups
                .Where(companySetup => companySetup.CompanySetupId == companyId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(CompaniesSetupRequest companySetupRequest, int companyId)
        {
            var setup = await GetSetupByCompanyIdAsync(companyId);
            try
            {
                if (setup == null)
                {
                    var newSetup = new CompanySetup();
                    newSetup.Theme = companySetupRequest.Theme;
                    newSetup.Logo = companySetupRequest.Logo;

                    await _dbContext.AddAsync(newSetup);
                    return await _dbContext.SaveChangesAsync() > 0;
                }

                setup.Theme = companySetupRequest.Theme;
                setup.Logo = string.IsNullOrEmpty(companySetupRequest.Logo) ? setup.Logo : companySetupRequest.Logo;


                _dbContext.CompaniesSetups.Update(setup);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }

        }
    }
}