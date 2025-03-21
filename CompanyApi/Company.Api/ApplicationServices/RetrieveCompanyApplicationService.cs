﻿using Company.Api.DomainServices;
using Company.Api.Models;
using Company.Api.Utils;
using System.Net;

namespace Company.Api.ApplicationServices
{
    public interface IRetrieveCompanyApplicationService
    {
        Task<ResponseBuilder<List<CompanyResponse>>> RetrieveAllCompanies();
        Task<ResponseBuilder<CompanyResponse?>> RetrieveCompanyById(Guid id);
        Task<ResponseBuilder<CompanyResponse?>> RetrieveCompanyByIsin(string isin);
    }

    public class RetrieveCompanyApplicationService : IRetrieveCompanyApplicationService
    {
        private readonly IRetrieveCompanyDomainService retrieveCompanyDomainService;
        private readonly ILogger<RetrieveCompanyApplicationService> logger;

        public RetrieveCompanyApplicationService(
            IRetrieveCompanyDomainService retrieveCompanyDomainService,
            ILogger<RetrieveCompanyApplicationService> logger)
        {
            this.retrieveCompanyDomainService = retrieveCompanyDomainService;
            this.logger = logger;
        }

        public async Task<ResponseBuilder<List<CompanyResponse>>> RetrieveAllCompanies()
        {
            var companies = (await retrieveCompanyDomainService.RetrieveAllCompanies()).ToList();
            return new ResponseBuilder<List<CompanyResponse>>().WithSuccess(companies, HttpStatusCode.OK);
        }

        public async Task<ResponseBuilder<CompanyResponse?>> RetrieveCompanyById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new ResponseBuilder<CompanyResponse?>()
                    .WithError("Invalid company Id provided.", HttpStatusCode.BadRequest);
            }

            var company = await retrieveCompanyDomainService.RetrieveCompanyById(id);
            if (company == null)
            {
                logger.LogWarning("Company with ID {Id} not found.", id);
                return new ResponseBuilder<CompanyResponse?>()
                    .WithError($"Company not found.", HttpStatusCode.NotFound);
            }

            return new ResponseBuilder<CompanyResponse?>().WithSuccess(company, HttpStatusCode.OK);
        }

        public async Task<ResponseBuilder<CompanyResponse?>> RetrieveCompanyByIsin(string isin)
        {
            if (string.IsNullOrEmpty(isin))
            {
                return new ResponseBuilder<CompanyResponse?>()
                    .WithError("Invalid company ISIN provided.", HttpStatusCode.BadRequest);
            }

            var company = await retrieveCompanyDomainService.RetrieveCompanybyIsin(isin);
            if (company == null)
            {
                logger.LogWarning("Company with ISIN {Isin} not found.", isin);
                return new ResponseBuilder<CompanyResponse?>()
                    .WithError($"Company not found.", HttpStatusCode.NotFound);
            }

            return new ResponseBuilder<CompanyResponse?>().WithSuccess(company, HttpStatusCode.OK);
        }
    }
}
