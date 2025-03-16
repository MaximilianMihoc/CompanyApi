namespace Company.Api.Data.Entities
{
    public class CompanyEntity
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Ticker { get; set; }
        public required string Isin { get; set; }
        public string? Website { get; set; }
        public required Guid ExchangeId { get; set; }
        public ExchangeEntity? Exchange { get; set; }
    }
}
