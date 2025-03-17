using Company.Api.Commands;
using Company.Api.Domains;
using Company.Api.DomainServices;
using Company.Api.Models;
using Company.Api.Submissions;
using Company.Api.Utils;
using System.Net;

namespace Company.Api.ApplicationServices
{
    public interface IUserAuthenticationApplicationService
    {
        Task<ResponseBuilder<CreatedResponse>> RegisterUser(UserRegistrationSubmission request);
        Task<ResponseBuilder<TokenResponse>> LoginUser(UserLoginSubmission request);
    }

    public class UserAuthenticationApplicationService : IUserAuthenticationApplicationService
    {
        private readonly IUserRegistrationDomainFactory userRegistrationDomainFactory;
        private readonly ISaveUserCommand saveUserCommand;
        private readonly IUserLoginDomainFactory userLoginDomainFactory;
        private readonly IAuthenticationTokenDomainService authenticationTokenDomainService;

        public UserAuthenticationApplicationService(
            IUserRegistrationDomainFactory userRegistrationDomainFactory,
            ISaveUserCommand saveUserCommand,
            IUserLoginDomainFactory userLoginDomainFactory,
            IAuthenticationTokenDomainService authenticationTokenDomainService)
        {
            this.userRegistrationDomainFactory = userRegistrationDomainFactory;
            this.saveUserCommand = saveUserCommand;
            this.userLoginDomainFactory = userLoginDomainFactory;
            this.authenticationTokenDomainService = authenticationTokenDomainService;
        }

        public async Task<ResponseBuilder<TokenResponse>> LoginUser(UserLoginSubmission request)
        {
            var domainResult = await userLoginDomainFactory.LoginUser(request);
            if (!domainResult.IsSuccess)
            {
                return new ResponseBuilder<TokenResponse>()
                    .WithError(domainResult.Error, HttpStatusCode.BadRequest);
            }

            var token = authenticationTokenDomainService.CreateToken(domainResult.Value);
            var refreshToken = authenticationTokenDomainService.GenerateRefreshToken();

            var tokenDomain = UserTokenDomain.Create(domainResult.Value.Id, refreshToken, DateTime.UtcNow.AddDays(1));
            await saveUserCommand.UpdateUserTokens(tokenDomain);

            return new ResponseBuilder<TokenResponse>()
                .WithSuccess(new TokenResponse { AccessToken = token, RefreshToken = refreshToken }, HttpStatusCode.Created);
        }

        public async Task<ResponseBuilder<CreatedResponse>> RegisterUser(UserRegistrationSubmission request)
        {
            var domainResult = await userRegistrationDomainFactory.RegisterUser(request);
            if (!domainResult.IsSuccess)
            {
                return new ResponseBuilder<CreatedResponse>()
                    .WithError(domainResult.Error, HttpStatusCode.BadRequest);
            }

            await saveUserCommand.CreateUser(domainResult.Value);

            return new ResponseBuilder<CreatedResponse>()
                .WithSuccess(new CreatedResponse { Id = domainResult.Value.Id }, HttpStatusCode.Created);
        }
    }
}
