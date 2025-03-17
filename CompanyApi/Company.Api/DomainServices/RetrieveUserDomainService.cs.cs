using Company.Api.Data;
using Company.Api.Domains;
using Microsoft.EntityFrameworkCore;

namespace Company.Api.DomainServices
{
    public interface IRetrieveUserDomainService
    {
        Task<bool> DoesUserNameExist(string username);
        Task<UserDomain?> RetrieveUserByUserName(string username);
        Task<UserWithTokenDomain?> RetrieveUserTokensById(Guid id);
    }

    public class RetrieveUserDomainService : IRetrieveUserDomainService
    {
        private readonly CompanyDbContext dbContext;

        public RetrieveUserDomainService(CompanyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> DoesUserNameExist(string username)
        {
            return await dbContext.Users
                .Where(user => user.Username == username)
                .AnyAsync();
        }

        public async Task<UserWithTokenDomain?> RetrieveUserTokensById(Guid id)
        {
            return await dbContext.Users
                .Where(user => user.Id == id && user.RefreshToken != null && user.RefreshTokenExpiryTime != null)
                .Select(user =>
                    UserWithTokenDomain.Create(user.Id, user.Username, user.PasswordHash, user.Name, user.Email, user.Role,
                        user.RefreshToken, user.RefreshTokenExpiryTime.Value))
                .FirstOrDefaultAsync();
        }

        public async Task<UserDomain?> RetrieveUserByUserName(string username)
        {
            return await dbContext.Users
                .Where(user => user.Username == username)
                .Select(user => 
                    UserDomain.Create(user.Id, user.Username, user.PasswordHash, user.Name, user.Email, user.Role))
                .FirstOrDefaultAsync();
        }
    }
}
