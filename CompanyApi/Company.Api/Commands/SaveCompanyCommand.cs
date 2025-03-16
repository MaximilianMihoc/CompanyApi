using Company.Api.Data;
using Company.Api.Data.Entities;
using Company.Api.Domains;

namespace Company.Api.Commands
{
    public interface ISaveCompanyCommand
    {
        Task CreateCompany(CompanyDomain companyDomain);
        Task UpdateCompany(CompanyDomain companyDomain);
    }
    public class SaveCompanyCommand : ISaveCompanyCommand
    {
        private readonly CompanyDbContext dbContext;

        public SaveCompanyCommand(CompanyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateCompany(CompanyDomain companyDomain)
        {
            var company = new CompanyEntity
            {
                Id = companyDomain.Id,
                Name = companyDomain.Name,
                ExchangeId = companyDomain.ExchangeId,
                Ticker = companyDomain.Ticker,
                Isin = companyDomain.Isin,
                Website = companyDomain.Website
            };

            await dbContext.Companies.AddAsync(company);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateCompany(CompanyDomain companyDomain)
        {
            var company = new CompanyEntity
            {
                Id = companyDomain.Id,
                Name = companyDomain.Name,
                ExchangeId = companyDomain.ExchangeId,
                Ticker = companyDomain.Ticker,
                Isin = companyDomain.Isin,
                Website = companyDomain.Website
            };

            dbContext.Companies.Update(company);
            await dbContext.SaveChangesAsync();
        }
    }
}
