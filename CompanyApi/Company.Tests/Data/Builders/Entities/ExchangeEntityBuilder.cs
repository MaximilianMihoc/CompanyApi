using Company.Api.Data.Entities;

namespace Company.Tests.Data.Builders.Entities
{
    internal class ExchangeEntityBuilder : BuilderBase<ExchangeEntity>
    {
        private Guid id = Guid.NewGuid();
        private string name = "NYSE";

        public ExchangeEntityBuilder WithId(Guid id)
        {
            this.id = id;
            return this;
        }

        public ExchangeEntityBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public override ExchangeEntity Build()
        {
            return new()
            {
                Id = id,
                Name = name
            };
        }
    }
}
