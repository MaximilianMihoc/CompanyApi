using Company.Api.Data;
using Company.Api.Models;
using Company.Api.Projections;
using Microsoft.EntityFrameworkCore;

namespace Company.Api.Queries
{
    public interface ICompanyQuery
    {
        Task<IEnumerable<CompanyResponse>> GetAllCompanies(ICompanyProjection projection);
        Task<CompanyResponse?> GetCompanyById(Guid id, ICompanyProjection projection);
        Task<CompanyResponse?> GetCompanyByIsin(string isin, ICompanyProjection projection);
        Task<bool> IsIsinUsed(string isin, Guid? excludeCompanyId = null);
    }

    public class CompanyQuery : ICompanyQuery
    {
        private readonly CompanyDbContext dbContext;

        public CompanyQuery(CompanyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<CompanyResponse>> GetAllCompanies(ICompanyProjection projection)
        {
            return await dbContext.Companies
                .Select(projection.Project())
                .ToListAsync();
        }

        public async Task<CompanyResponse?> GetCompanyById(Guid id, ICompanyProjection projection)
        {
            return await dbContext.Companies
                .Where(company => company.Id == id)
                .Select(projection.Project())
                .FirstOrDefaultAsync();
        }

        public async Task<CompanyResponse?> GetCompanyByIsin(string isin, ICompanyProjection projection)
        {
            return await dbContext.Companies
                .Where(company => company.Isin == isin)
                .Select(projection.Project())
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsIsinUsed(string isin, Guid? excludeCompanyId = null)
        {
            return await dbContext.Companies
                .Where(company => company.Isin == isin)
                .Where(company => !excludeCompanyId.HasValue || company.Id != excludeCompanyId.Value)
                .AnyAsync();
        }
    }
}
