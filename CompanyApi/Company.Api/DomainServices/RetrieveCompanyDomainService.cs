using Company.Api.Models;

namespace Company.Api.DomainServices
{
    public interface IRetrieveCompanyDomainService
    {
        Task<IEnumerable<CompanyResponse>> RetrieveAllCompanies();
        Task<CompanyResponse?> RetrieveCompanyById(Guid id);
        Task<CompanyResponse?> RetrieveCompanybyIsin(string isin);
    }

    public class RetrieveCompanyDomainService : IRetrieveCompanyDomainService
    {
        public Task<IEnumerable<CompanyResponse>> RetrieveAllCompanies()
        {
            throw new NotImplementedException();
        }

        public Task<CompanyResponse?> RetrieveCompanyById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<CompanyResponse?> RetrieveCompanybyIsin(string isin)
        {
            throw new NotImplementedException();
        }
    }
}
