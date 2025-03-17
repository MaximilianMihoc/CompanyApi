namespace Company.Api.Submissions
{
    public class RefreshTokenSubmission
    {
        public Guid UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
}
