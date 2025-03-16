using System.Text.Json.Serialization;

namespace Company.Api.Models
{
    public class CompanySubmission
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required Guid ExchangeId { get; set; }
        public required string Ticker { get; set; }
        public required string Isin { get; set; }
        public string? Website { get; set; }
    }
}
