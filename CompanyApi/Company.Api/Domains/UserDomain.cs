using Company.Api.Data.Entities;

namespace Company.Api.Domains
{
    public class UserDomain
    {
        public Guid Id { get; init; }
        public string Username { get; init; } = string.Empty;
        public string PasswordHash { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public UserRole Role { get; init; } = UserRole.User;

        protected UserDomain(Guid id, string username, string passwordHash, string name, string email, UserRole role)
        {
            Id = id;
            Username = username;
            PasswordHash = passwordHash;
            Name = name;
            Email = email;
            Role = role;
        }

        public static UserDomain Create(Guid id, string username, string passwordHash, string name, string email, UserRole role)
        {
            return new UserDomain(id, username, passwordHash, name, email, role);
        }
    }

    public class UserWithTokenDomain : UserDomain
    {
        public string RefreshToken { get; init; }
        public DateTime RefreshTokenExpiryTime { get; init; }

        protected UserWithTokenDomain(Guid id, string username, string passwordHash, string name, string email, UserRole role,
            string refreshToken, DateTime refreshTokenExpiryTime) 
            : base(id, username, passwordHash, name, email, role)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
        }

        public static UserWithTokenDomain Create(Guid id, string username, string passwordHash, string name, string email, UserRole role,
            string refreshToken, DateTime refreshTokenExpiryTime)
        {
            return new UserWithTokenDomain(id, username, passwordHash, name, email, role, refreshToken, refreshTokenExpiryTime);
        }
    }
}
