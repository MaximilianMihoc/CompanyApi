namespace Company.Api.Models
{
    public class CompanyResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Exchange { get; set; }
        public required string Ticker { get; set; }
        public required string Isin { get; set; }
        public string? Website { get; set; }
    }
}
