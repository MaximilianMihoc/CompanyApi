using Company.Api.Models;
using Company.Api.Projections;
using Company.Api.Queries;

namespace Company.Api.DomainServices
{
    public interface IRetrieveCompanyDomainService
    {
        Task<IEnumerable<CompanyResponse>> RetrieveAllCompanies();
        Task<CompanyResponse?> RetrieveCompanyById(Guid id);
        Task<CompanyResponse?> RetrieveCompanybyIsin(string isin);
        Task<bool> IsIsinUsed(string isin, Guid? excludeCompanyId = null);
    }

    public class RetrieveCompanyDomainService : IRetrieveCompanyDomainService
    {
        private readonly ICompanyQuery companyQuery;
        private readonly ICompanyProjection companyProjection;

        public RetrieveCompanyDomainService(
            ICompanyQuery companyQuery,
            ICompanyProjection companyProjection)
        {
            this.companyQuery = companyQuery;
            this.companyProjection = companyProjection;
        }

        public async Task<bool> IsIsinUsed(string isin, Guid? excludeCompanyId = null)
        {
            return await companyQuery.IsIsinUsed(isin, excludeCompanyId);
        }

        public async Task<IEnumerable<CompanyResponse>> RetrieveAllCompanies()
        {
            return await companyQuery.GetAllCompanies(companyProjection);
        }

        public async Task<CompanyResponse?> RetrieveCompanyById(Guid id)
        {
            return await companyQuery.GetCompanyById(id, companyProjection);
        }

        public async Task<CompanyResponse?> RetrieveCompanybyIsin(string isin)
        {
            return await companyQuery.GetCompanyByIsin(isin, companyProjection);
        }
    }
}
