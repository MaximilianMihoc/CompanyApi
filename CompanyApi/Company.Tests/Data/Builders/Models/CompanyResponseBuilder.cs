using Company.Api.Models;

namespace Company.Tests.Data.Builders.Models
{
    internal class CompanyResponseBuilder : BuilderBase<CompanyResponse>
    {
        private Guid id = Guid.NewGuid();
        private string name = "Company Name";
        private string exchange = "Exchange";
        private string ticker = "Ticker";
        private string isin = "Isin";
        private string? website;

        public CompanyResponseBuilder WithId(Guid id)
        {
            this.id = id;
            return this;
        }

        public CompanyResponseBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public CompanyResponseBuilder WithExchange(string exchange)
        {
            this.exchange = exchange;
            return this;
        }

        public CompanyResponseBuilder WithTicker(string ticker)
        {
            this.ticker = ticker;
            return this;
        }

        public CompanyResponseBuilder WithIsin(string isin)
        {
            this.isin = isin;
            return this;
        }

        public CompanyResponseBuilder WithWebsite(string website)
        {
            this.website = website;
            return this;
        }

        public override CompanyResponse Build()
        {
            return new()
            {
                Id = id,
                Name = name,
                Exchange = exchange,
                Ticker = ticker,
                Isin = isin,
                Website = website
            };
        }
    }
}
