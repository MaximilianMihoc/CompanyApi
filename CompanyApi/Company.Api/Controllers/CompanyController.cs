using Company.Api.ApplicationServices;
using Company.Api.Models;
using Company.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Api.Controllers
{
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IRetrieveCompanyApplicationService retrieveCompanyApplicationService;
        private readonly ISaveCompanyApplicationService saveCompanyApplicationService;
        private readonly ILogger<CompanyController> logger;

        public CompanyController(
            IRetrieveCompanyApplicationService retrieveCompanyApplicationService,
            ISaveCompanyApplicationService saveCompanyApplicationService,
            ILogger<CompanyController> logger)
        {
            this.retrieveCompanyApplicationService = retrieveCompanyApplicationService;
            this.saveCompanyApplicationService = saveCompanyApplicationService;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(List<CompanyResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrieveAllCompanies()
        {
            var responseBuilder = await retrieveCompanyApplicationService.RetrieveAllCompanies();
            return responseBuilder.Build(this, nameof(RetrieveAllCompanies), logger);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(CompanyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrieveCompanyById(Guid id)
        {
            var responseBuilder = await retrieveCompanyApplicationService.RetrieveCompanyById(id);
            return responseBuilder.Build(this, nameof(RetrieveCompanyById), logger);
        }

        [HttpGet("isin/{isin}")]
        [Authorize]
        [ProducesResponseType(typeof(CompanyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrieveCompanyByIsin(string isin)
        {
            var responseBuilder = await retrieveCompanyApplicationService.RetrieveCompanyByIsin(isin);
            return responseBuilder.Build(this, nameof(RetrieveCompanyByIsin), logger);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCompany(CompanySubmission company)
        {
            var responseBuilder = await saveCompanyApplicationService.CreateCompany(company);
            return responseBuilder.Build(this, nameof(AddCompany), logger);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCompany(Guid id, CompanySubmission company)
        {
            company.Id = id;
            var responseBuilder = await saveCompanyApplicationService.UpdateCompany(company);
            return responseBuilder.Build(this, nameof(UpdateCompany), logger);
        }
    }
}
