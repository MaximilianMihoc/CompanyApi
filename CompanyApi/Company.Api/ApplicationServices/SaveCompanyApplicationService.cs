using Company.Api.Commands;
using Company.Api.DomainServices;
using Company.Api.Models;
using Company.Api.Utils;
using System.Net;

namespace Company.Api.ApplicationServices
{
    public interface ISaveCompanyApplicationService
    {
        Task<ResponseBuilder<CreatedResponse>> CreateCompany(CompanySubmission company);
        Task<ResponseBuilder> UpdateCompany(CompanySubmission company);
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

        public async Task<ResponseBuilder<CreatedResponse>> CreateCompany(CompanySubmission company)
        {
            var domainResult = await companyDomainFactory.BuildCompanyDomain(company);

            if (!domainResult.IsSuccess)
            {
                logger.LogWarning("Company creation failed: {Error}", domainResult.Error);
                return new ResponseBuilder<CreatedResponse>()
                    .WithError(domainResult.Error, HttpStatusCode.BadRequest);
            }

            await saveCompanyCommand.CreateCompany(domainResult.Value);

            return new ResponseBuilder<CreatedResponse>()
                .WithSuccess(new CreatedResponse { Id = domainResult.Value.Id }, HttpStatusCode.Created);
        }

        public async Task<ResponseBuilder> UpdateCompany(CompanySubmission company)
        {
            var domainResult = await companyDomainFactory.BuildCompanyDomainForUpdate(company);
            if (!domainResult.IsSuccess)
            {
                logger.LogWarning("Company update failed: {Error}", domainResult.Error);
                return new ResponseBuilder()
                    .WithError(domainResult.Error, HttpStatusCode.BadRequest);
            }

            await saveCompanyCommand.UpdateCompany(domainResult.Value);

            return new ResponseBuilder().WithSuccess(HttpStatusCode.NoContent);

        }
    }
}
