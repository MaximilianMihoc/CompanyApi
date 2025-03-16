using Company.Api.Domains;
using Company.Api.Models;
using Company.Api.Utils;

namespace Company.Api.DomainServices
{
    public interface ICompanyDomainFactory
    {
        Result<CompanyDomain> BuildCompanyDomain(CompanySubmission company);
    }

    public class CompanyDomainFactory : ICompanyDomainFactory
    {
        private readonly IRetrieveCompanyDomainService retrieveCompanyDomainService;

        public CompanyDomainFactory(IRetrieveCompanyDomainService retrieveCompanyDomainService)
        {
            this.retrieveCompanyDomainService = retrieveCompanyDomainService;
        }

        public Result<CompanyDomain> BuildCompanyDomain(CompanySubmission company)
        {
            if (company == null)
                return Result<CompanyDomain>.Failure("Company submission cannot be null.");

            if(!IsValidIsin(company.Isin))
                return Result<CompanyDomain>.Failure("Invalid ISIN format.");

            var existingCar = retrieveCompanyDomainService.RetrieveCompanybyIsin(company.Isin).Result;
            if (existingCar is not null)
                return Result<CompanyDomain>.Failure($"A company with ISIN {company.Isin} already exists.");

            var companyDomain = CompanyDomain.Create(Guid.NewGuid(), company.Name, company.ExchangeId, company.Ticker, company.Isin, company.Website);
            return Result<CompanyDomain>.Success(companyDomain);
        }

        private bool IsValidIsin(string isin)
        {
            if (string.IsNullOrWhiteSpace(isin) || isin.Length < 2)
                return false;

            return char.IsLetter(isin[0]) && char.IsLetter(isin[1]);
        }
    }
}
