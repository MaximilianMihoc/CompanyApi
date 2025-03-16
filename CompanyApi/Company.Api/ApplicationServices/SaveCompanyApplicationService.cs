using Company.Api.Commands;
using Company.Api.DomainServices;
using Company.Api.Models;
using Company.Api.Utils;
using System.Net;

namespace Company.Api.ApplicationServices
{
    public interface ISaveCompanyApplicationService
    {
        Task<ResponseBuilder<Guid>> CreateCompany(CompanySubmission company);
    }

    public class SaveCompanyApplicationService : ISaveCompanyApplicationService
    {
        private readonly ICompanyDomainFactory companyDomainFactory;
        private readonly ISaveCompanyCommand saveCompanyCommand;
        private readonly ILogger<SaveCompanyApplicationService> logger;

        public SaveCompanyApplicationService(
            ICompanyDomainFactory companyDomainFactory,
            ISaveCompanyCommand saveCompanyCommand,
            ILogger<SaveCompanyApplicationService> logger)
        {
            this.companyDomainFactory = companyDomainFactory;
            this.saveCompanyCommand = saveCompanyCommand;
            this.logger = logger;
        }

        public async Task<ResponseBuilder<Guid>> CreateCompany(CompanySubmission company)
        {
            var domainResult = companyDomainFactory.BuildCompanyDomain(company);

            if (!domainResult.IsSuccess)
            {
                logger.LogWarning("Company creation failed: {Error}", domainResult.Error);
                return new ResponseBuilder<Guid>()
                    .WithError(domainResult.Error, HttpStatusCode.BadRequest);
            }

            await saveCompanyCommand.CreateCompany(domainResult.Value);

            return new ResponseBuilder<Guid>().WithSuccess(domainResult.Value.Id, HttpStatusCode.Created);
        }
    }
}
