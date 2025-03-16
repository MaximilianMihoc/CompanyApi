using Company.Api.Domains;

namespace Company.Tests.Data.Builders.Domains
{
    internal class CompanyDomainBuilder : BuilderBase<CompanyDomain>
    {
        private Guid id = Guid.NewGuid();
        private string name = "Company Name";
        private string ticker = "Ticker";
        private string isin = "Isin";
        private string? website;
        private Guid exchangeId = Guid.NewGuid();

        public CompanyDomainBuilder WithId(Guid id)
        {
            this.id = id;
            return this;
        }

        public CompanyDomainBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public CompanyDomainBuilder WithTicker(string ticker)
        {
            this.ticker = ticker;
            return this;
        }

        public CompanyDomainBuilder WithIsin(string isin)
        {
            this.isin = isin;
            return this;
        }

        public CompanyDomainBuilder WithWebsite(string website)
        {
            this.website = website;
            return this;
        }

        public CompanyDomainBuilder WithExchangeId(Guid exchangeId)
        {
            this.exchangeId = exchangeId;
            return this;
        }

        public override CompanyDomain Build()
        {
            return CompanyDomain.Create(id, name, exchangeId, ticker, isin, website);
        }
    }
}
