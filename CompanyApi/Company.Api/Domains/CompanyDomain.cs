namespace Company.Api.Domains
{
    public class CompanyDomain
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public Guid ExchangeId { get; init; }
        public string Ticker { get; init; }
        public string Isin { get; init; }
        public string? Website { get; init; }

        protected CompanyDomain(Guid id, string name, Guid exchangeId, string ticker, string isin, string? website)
        {
            Id = id;
            Name = name;
            ExchangeId = exchangeId;
            Ticker = ticker;
            Isin = isin;
            Website = website;
        }

        public static CompanyDomain Create(Guid id, string name, Guid exchangeId, string ticker, string isin, string? website)
        {
            return new CompanyDomain(id, name, exchangeId, ticker, isin, website);
        }
    }
}
