using Company.Api.Domains;
using Company.Api.Models;
using Company.Api.Utils;

namespace Company.Api.DomainServices
{
    public interface ICompanyDomainFactory
    {
        Task<Result<CompanyDomain>> BuildCompanyDomain(CompanySubmission company);
        Task<Result<CompanyDomain>> BuildCompanyDomainForUpdate(CompanySubmission company);
    }

    public class CompanyDomainFactory : ICompanyDomainFactory
    {
        private readonly IRetrieveCompanyDomainService retrieveCompanyDomainService;

        public CompanyDomainFactory(IRetrieveCompanyDomainService retrieveCompanyDomainService)
        {
            this.retrieveCompanyDomainService = retrieveCompanyDomainService;
        }

        public async Task<Result<CompanyDomain>> BuildCompanyDomain(CompanySubmission company)
        {
            if (company == null)
                return Result<CompanyDomain>.Failure("Company submission cannot be null.");

            if(!IsValidIsin(company.Isin))
                return Result<CompanyDomain>.Failure("Invalid ISIN format.");

            var isIsinUsed = await retrieveCompanyDomainService.IsIsinUsed(company.Isin);
            if (isIsinUsed)
                return Result<CompanyDomain>.Failure($"A company with ISIN {company.Isin} already exists.");

            var companyDomain = CompanyDomain.Create(Guid.NewGuid(), company.Name, company.ExchangeId, company.Ticker, company.Isin, company.Website);
            return Result<CompanyDomain>.Success(companyDomain);
        }

        public async Task<Result<CompanyDomain>> BuildCompanyDomainForUpdate(CompanySubmission company)
        {
            if (company == null)
                return Result<CompanyDomain>.Failure("Company submission cannot be null.");

            var existingCompany = retrieveCompanyDomainService.RetrieveCompanyById(company.Id).Result;
            if (existingCompany is null)
                return Result<CompanyDomain>.Failure($"A company with Id {company.Id} does not exist.");

            if (existingCompany.Isin != company.Isin && !IsValidIsin(company.Isin))
                return Result<CompanyDomain>.Failure("Invalid ISIN format.");

            if (existingCompany.Isin != company.Isin)
            { 
                var isIsinUsed = await retrieveCompanyDomainService.IsIsinUsed(company.Isin, existingCompany.Id);
                if (isIsinUsed)
                    return Result<CompanyDomain>.Failure($"A company with ISIN {company.Isin} already exists.");
            }
            var companyDomain = CompanyDomain.Create(existingCompany.Id, company.Name, company.ExchangeId, company.Ticker, company.Isin, company.Website);
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
