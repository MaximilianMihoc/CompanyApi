using Company.Api.Data.Entities;

namespace Company.Api.Submissions
{
    public class UserRegistrationSubmission
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole? Role { get; set; }
    }
}
