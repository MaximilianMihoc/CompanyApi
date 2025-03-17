using Company.Api.Domains;
using Company.Api.Submissions;
using Company.Api.Utils;
using Microsoft.AspNetCore.Identity;

namespace Company.Api.DomainServices
{
    public interface IUserLoginDomainFactory
    {
        Task<Result<UserWithTokenDomain>> LoginUser(UserLoginSubmission request);
        Task<Result<UserWithTokenDomain>> RefreshTokens(RefreshTokenSubmission request);
    }

    public class UserLoginDomainFactory : IUserLoginDomainFactory
    {
        private readonly IRetrieveUserDomainService retrieveUserDomainService;
        private readonly IAuthenticationTokenDomainService authenticationTokenDomainService;

        public UserLoginDomainFactory(
            IRetrieveUserDomainService retrieveUserDomainService,
            IAuthenticationTokenDomainService authenticationTokenDomainService)
        {
            this.retrieveUserDomainService = retrieveUserDomainService;
            this.authenticationTokenDomainService = authenticationTokenDomainService;
        }

        public async Task<Result<UserWithTokenDomain>> LoginUser(UserLoginSubmission request)
        {
            var user = await retrieveUserDomainService.RetrieveUserByUserName(request.Username);

            if (user is null)
                return Result<UserWithTokenDomain>.Failure("Invalid username or password.");

            if (new PasswordHasher<UserLoginSubmission>().VerifyHashedPassword(request, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
                return Result<UserWithTokenDomain>.Failure("Invalid username or password.");
            
            var refreshToken = authenticationTokenDomainService.GenerateRefreshToken();

            var tokenDomain = UserWithTokenDomain.Create(user.Id, user.Username, user.PasswordHash, user.Name, user.Email, user.Role,
                refreshToken, DateTime.UtcNow.AddDays(1));

            return Result<UserWithTokenDomain>.Success(tokenDomain);
        }

        public async Task<Result<UserWithTokenDomain>> RefreshTokens(RefreshTokenSubmission request)
        {
            var user = await retrieveUserDomainService.RetrieveUserTokensById(request.UserId);

            if (user is null)
                return Result<UserWithTokenDomain>.Failure("Invalid refresh token.");

            if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return Result<UserWithTokenDomain>.Failure("Invalid refresh token.");

            var refreshToken = authenticationTokenDomainService.GenerateRefreshToken();

            var tokenDomain = UserWithTokenDomain.Create(user.Id, user.Username, user.PasswordHash, user.Name, user.Email, user.Role,
                refreshToken, DateTime.UtcNow.AddDays(1));

            return Result<UserWithTokenDomain>.Success(tokenDomain);
        }
    }
}
