using Company.Api.Data.Entities;

namespace Company.Tests.Data.Builders.Entities
{
    internal class CompanyEntityBuilder : BuilderBase<CompanyEntity>
    {
        private Guid id = Guid.NewGuid();
        private string name = "Company Name";
        private string ticker = "Ticker";
        private string isin = "Isin";
        private string? website;

        private Guid exchangeId = Guid.NewGuid();
        private ExchangeEntity? exchange;

        public CompanyEntityBuilder WithId(Guid id)
        {
            this.id = id;
            return this;
        }

        public CompanyEntityBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public CompanyEntityBuilder WithTicker(string ticker)
        {
            this.ticker = ticker;
            return this;
        }

        public CompanyEntityBuilder WithIsin(string isin)
        {
            this.isin = isin;
            return this;
        }

        public CompanyEntityBuilder WithWebsite(string website)
        {
            this.website = website;
            return this;
        }

        public CompanyEntityBuilder WithExchange(Func<ExchangeEntityBuilder, ExchangeEntityBuilder> builderFunc)
        {
            var builder = new ExchangeEntityBuilder();
            exchange = builderFunc.Invoke(builder);
            exchangeId = exchange.Id;
            return this;
        }

        public override CompanyEntity Build()
        {
            return new()
            {
                Id = id,
                Name = name,
                ExchangeId = exchangeId,
                Exchange = exchange,
                Ticker = ticker,
                Isin = isin,
                Website = website
            };
        }
    }
}
