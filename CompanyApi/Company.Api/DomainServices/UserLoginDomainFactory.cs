using Company.Api.Domains;
using Company.Api.Submissions;
using Company.Api.Utils;
using Microsoft.AspNetCore.Identity;

namespace Company.Api.DomainServices
{
    public interface IUserLoginDomainFactory
    {
        Task<Result<UserDomain>> LoginUser(UserLoginSubmission request);
    }

    public class UserLoginDomainFactory : IUserLoginDomainFactory
    {
        private readonly IRetrieveUserDomainService retrieveUserDomainService;

        public UserLoginDomainFactory(
            IRetrieveUserDomainService retrieveUserDomainService)
        {
            this.retrieveUserDomainService = retrieveUserDomainService;
        }

        public async Task<Result<UserDomain>> LoginUser(UserLoginSubmission request)
        {
            var user = await retrieveUserDomainService.RetrieveUserByUserName(request.Username);

            if (user is null)
                return Result<UserDomain>.Failure("Invalid username or password.");

            if (new PasswordHasher<UserLoginSubmission>().VerifyHashedPassword(request, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
                return Result<UserDomain>.Failure("Invalid username or password.");

            return Result<UserDomain>.Success(user);
        }
    }
}
