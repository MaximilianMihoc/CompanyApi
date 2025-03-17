using Company.Api.ApplicationServices;
using Company.Api.Models;
using Company.Api.Submissions;
using Company.Api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Company.Api.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserAuthenticationApplicationService userAuthenticationApplicationService;
        private readonly ILogger<SaveCompanyApplicationService> logger;

        public AuthenticationController(
            IUserAuthenticationApplicationService userAuthenticationApplicationService,
            ILogger<SaveCompanyApplicationService> logger)
        {
            this.userAuthenticationApplicationService = userAuthenticationApplicationService;
            this.logger = logger;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUser(UserRegistrationSubmission request)
        {
            var responseBuilder = await userAuthenticationApplicationService.RegisterUser(request);
            return responseBuilder.Build(this, nameof(AddUser), logger);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginUser(UserLoginSubmission request)
        {
            var responseBuilder = await userAuthenticationApplicationService.LoginUser(request);
            return responseBuilder.Build(this, nameof(AddUser), logger);
        }
    }
}
