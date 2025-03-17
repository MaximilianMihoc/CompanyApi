using Company.Api.Data.Entities;
using Company.Api.Domains;
using Company.Api.Submissions;
using Company.Api.Utils;
using Microsoft.AspNetCore.Identity;

namespace Company.Api.DomainServices
{
    public interface IUserRegistrationDomainFactory
    {
        Task<Result<UserDomain>> RegisterUser(UserRegistrationSubmission request);
    }

    public class UserRegistrationDomainFactory : IUserRegistrationDomainFactory
    {
        private readonly IRetrieveUserDomainService retrieveUserDomainService;

        public UserRegistrationDomainFactory(IRetrieveUserDomainService retrieveUserDomainService)
        {
            this.retrieveUserDomainService = retrieveUserDomainService;
        }

        public async Task<Result<UserDomain>> RegisterUser(UserRegistrationSubmission request)
        {
            var doesUserNameExist = await retrieveUserDomainService.DoesUserNameExist(request.Username);
            if (doesUserNameExist)
                return Result<UserDomain>.Failure("Username already exists.");

            var hashedPassword = new PasswordHasher<UserRegistrationSubmission>()
                .HashPassword(request, request.Password);

            var userDomain = UserDomain.Create(Guid.NewGuid(), request.Username, hashedPassword, request.Name, request.Email, request.Role ?? UserRole.User);
            return Result<UserDomain>.Success(userDomain);
        }
    }
}
