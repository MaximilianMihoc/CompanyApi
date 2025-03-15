using Company.Api.ApplicationServices;
using Company.Api.Models;
using Company.Api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Company.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IRetrieveCompanyApplicationService retrieveCompanyApplicationService;
        private readonly ILogger<CompanyController> logger;

        public CompanyController(
            IRetrieveCompanyApplicationService retrieveCompanyApplicationService,
            ILogger<CompanyController> logger)
        {
            this.retrieveCompanyApplicationService = retrieveCompanyApplicationService;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CompanyResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrieveAllCompanies()
        {
            var responseBuilder = await retrieveCompanyApplicationService.RetrieveAllCompanies();
            return responseBuilder.Build(this, nameof(RetrieveAllCompanies), logger);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CompanyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrieveCompanyById(Guid id)
        {
            var responseBuilder = await retrieveCompanyApplicationService.RetrieveCompanyById(id);
            return responseBuilder.Build(this, nameof(RetrieveCompanyById), logger);
        }
    }
}
