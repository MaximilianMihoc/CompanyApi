using Company.Api.Models;

namespace Company.Tests.Data.Builders.Models
{
    internal class CompanySubmissionBuilder : BuilderBase<CompanySubmission>
    {
        private Guid id = Guid.NewGuid();
        private string name = "Company Name";
        private Guid exchangeId = Guid.NewGuid();
        private string ticker = "Ticker";
        private string isin = "Isin";
        private string? website;

        public CompanySubmissionBuilder WithId(Guid id)
        {
            this.id = id;
            return this;
        }

        public CompanySubmissionBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public CompanySubmissionBuilder WithExchangeId(Guid exchangeId)
        {
            this.exchangeId = exchangeId;
            return this;
        }

        public CompanySubmissionBuilder WithTicker(string ticker)
        {
            this.ticker = ticker;
            return this;
        }

        public CompanySubmissionBuilder WithIsin(string isin)
        {
            this.isin = isin;
            return this;
        }

        public CompanySubmissionBuilder WithWebsite(string website)
        {
            this.website = website;
            return this;
        }

        public override CompanySubmission Build()
        {
            return new()
            {
                Id = id,
                Name = name,
                ExchangeId = exchangeId,
                Ticker = ticker,
                Isin = isin,
                Website = website
            };
        }
    }
}
