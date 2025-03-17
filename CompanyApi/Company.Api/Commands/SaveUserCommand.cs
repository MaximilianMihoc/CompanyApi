using Company.Api.Data.Entities;
using Company.Api.Data;
using Company.Api.Domains;

namespace Company.Api.Commands
{
    public interface ISaveUserCommand
    {
        Task CreateUser(UserDomain userDomain);
        Task UpdateUserTokens(UserTokenDomain userDomain);
    }

    public class SaveUserCommand : ISaveUserCommand
    {
        private readonly CompanyDbContext dbContext;

        public SaveUserCommand(CompanyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateUser(UserDomain userDomain)
        {
            var user = new UserEntity
            {
                Id = userDomain.Id,
                Username = userDomain.Username,
                PasswordHash = userDomain.PasswordHash,
                Name = userDomain.Name,
                Email = userDomain.Email,
                Role = userDomain.Role
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserTokens(UserTokenDomain userDomain)
        {
            var user = new UserEntity
            {
                Id = userDomain.Id, 
                RefreshToken = userDomain.RefreshToken,
                RefreshTokenExpiryTime = userDomain.RefreshTokenExpiryTime
            };

            dbContext.Users.Attach(user); // Attach without fetching
            dbContext.Entry(user).Property(x => x.RefreshToken).IsModified = true;
            dbContext.Entry(user).Property(x => x.RefreshTokenExpiryTime).IsModified = true;
            
            await dbContext.SaveChangesAsync();
        }
    }
}
