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
        Task<ResponseBuilder<TokenResponse>> RefreshToken(RefreshTokenSubmission request);
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
            await saveUserCommand.UpdateUserTokens(domainResult.Value);

            var token = authenticationTokenDomainService.CreateToken(domainResult.Value);
            return new ResponseBuilder<TokenResponse>()
                .WithSuccess(new TokenResponse { AccessToken = token, RefreshToken = domainResult.Value.RefreshToken }, HttpStatusCode.Created);
        }

        public async Task<ResponseBuilder<TokenResponse>> RefreshToken(RefreshTokenSubmission request)
        {
            var domainResult = await userLoginDomainFactory.RefreshTokens(request);
            if (!domainResult.IsSuccess)
            {
                return new ResponseBuilder<TokenResponse>()
                    .WithError(domainResult.Error, HttpStatusCode.Unauthorized);
            }
            await saveUserCommand.UpdateUserTokens(domainResult.Value);

            var token = authenticationTokenDomainService.CreateToken(domainResult.Value);
            return new ResponseBuilder<TokenResponse>()
                .WithSuccess(new TokenResponse { AccessToken = token, RefreshToken = domainResult.Value.RefreshToken }, HttpStatusCode.Created);
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
