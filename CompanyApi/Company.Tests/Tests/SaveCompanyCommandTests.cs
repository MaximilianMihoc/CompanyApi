using Company.Api.Commands;
using Company.Api.Data;
using Company.Api.Data.Entities;
using Company.Tests.Data.Builders.Domains;
using Company.Tests.Data.Builders.Entities;
using Microsoft.EntityFrameworkCore;

namespace Company.Tests.Tests
{
    public class SaveCompanyCommandTests : IClassFixture<SaveCompanyCommandFixture>
    {
        private readonly SaveCompanyCommandFixture fixture;
        private readonly SaveCompanyCommand sut;

        public SaveCompanyCommandTests(SaveCompanyCommandFixture fixture)
        {
            this.fixture = fixture;
            sut = new SaveCompanyCommand(fixture.Context);
        }

        [Fact]
        public async Task CreateCompany_ShouldCreateCompany()
        {
            var companyDomain = new CompanyDomainBuilder()
                .WithId(Guid.NewGuid())
                .WithName("Apple Inc")
                .WithExchangeId(fixture.NasdaqId)
                .WithTicker("AAPL")
                .WithIsin("US0378331005")
                .WithWebsite("https://www.apple.com/")
                .Build();

            await sut.CreateCompany(companyDomain);
            var company = await fixture.Context.Companies.FindAsync(companyDomain.Id);
            Assert.NotNull(company);
            Assert.Equal(companyDomain.Id, company.Id);
            Assert.Equal(companyDomain.Name, company.Name);
            Assert.Equal(companyDomain.ExchangeId, company.ExchangeId);
            Assert.Equal(companyDomain.Ticker, company.Ticker);
            Assert.Equal(companyDomain.Isin, company.Isin);
            Assert.Equal(companyDomain.Website, company.Website);
        }

        [Fact]
        public async Task UpdateCompany_ShouldUpdateCompany()
        {
            var companyDomain = new CompanyDomainBuilder()
                .WithId(fixture.PorscheAutomobilId)
                .WithName("Porsche AG")
                .WithExchangeId(fixture.NasdaqId)
                .WithTicker("PAH3")
                .WithIsin("DE000PAH0038")
                .WithWebsite("https://www.porsche.com/")
                .Build();

            await sut.UpdateCompany(companyDomain);
            var company = await fixture.Context.Companies.FindAsync(fixture.PorscheAutomobilId);
            Assert.NotNull(company);
            Assert.Equal(companyDomain.Id, company.Id);
            Assert.Equal(companyDomain.Name, company.Name);
            Assert.Equal(companyDomain.ExchangeId, company.ExchangeId);
            Assert.Equal(companyDomain.Ticker, company.Ticker);
            Assert.Equal(companyDomain.Isin, company.Isin);
            Assert.Equal(companyDomain.Website, company.Website);
        }
    }

    public class SaveCompanyCommandFixture
    {
        public readonly CompanyDbContext Context;
        public Guid PorscheAutomobilId = Guid.NewGuid();
        public Guid NasdaqId = Guid.NewGuid();

        private CompanyEntity porscheAutomobil => new CompanyEntityBuilder()
            .WithId(PorscheAutomobilId)
            .WithName("Porsche Automobil")
            .WithTicker("PAH3")
            .WithIsin("DE000PAH0038")
            .WithWebsite("https://www.porsche.com/")
            .WithExchange(builder => builder.WithName("Deutsche Börse"))
            .Build();

        public SaveCompanyCommandFixture()
        {
            var options = new DbContextOptionsBuilder<CompanyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            Context = new CompanyDbContext(options);

            SeedData().Wait();
        }

        private async Task SeedData()
        {
            await Context.Exchanges.AddAsync(new ExchangeEntity { Id = NasdaqId, Name = "Nasdaq" });

            await Context.AddAsync(porscheAutomobil);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();
        }
    }
}
