using Company.Api.ApplicationServices;
using Company.Api.Models;
using Company.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Api.Controllers
{
    [Route("api/lookup")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private readonly IExchangeLookupApplicationService exchangeLookupApplicationService;
        private readonly ILogger<LookupController> logger;

        public LookupController(
            IExchangeLookupApplicationService exchangeLookupApplicationService,
            ILogger<LookupController> logger)
        {
            this.exchangeLookupApplicationService = exchangeLookupApplicationService;
            this.logger = logger;
        }

        [HttpGet("exchange")]
        [Authorize]
        [ProducesResponseType(typeof(List<ExchangeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrieveAll()
        {
            var responseBuilder = await exchangeLookupApplicationService.GetAllExchanges();
            return responseBuilder.Build(this, nameof(RetrieveAll), logger);
        }

        [HttpGet("exchange/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ExchangeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrieveById(Guid id)
        {
            var responseBuilder = await exchangeLookupApplicationService.GetExchangeById(id);
            return responseBuilder.Build(this, nameof(RetrieveById), logger);
        }

        [HttpPost("exchange")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(ExchangeSubmission company)
        {
            var responseBuilder = await exchangeLookupApplicationService.CreateExchange(company);
            return responseBuilder.Build(this, nameof(Add), logger);
        }
    }
}
