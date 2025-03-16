using Company.Api.Data;
using Company.Api.Data.Entities;
using Company.Api.DomainServices;
using Company.Api.Projections;
using Company.Api.Queries;
using Company.Tests.Data.Builders.Entities;
using Microsoft.EntityFrameworkCore;

namespace Company.Tests.Tests
{
    public class RetrieveCompanyDomainServiceTests : IClassFixture<RetrieveCompanyDomainServiceFixture>
    {
        private readonly RetrieveCompanyDomainServiceFixture fixture;
        private readonly RetrieveCompanyDomainService sut;

        public RetrieveCompanyDomainServiceTests(RetrieveCompanyDomainServiceFixture fixture)
        {
            this.fixture = fixture;
            var companyProjection = new CompanyProjection();
            var companyQuery = new CompanyQuery(fixture.Context);
            sut = new RetrieveCompanyDomainService(companyQuery, companyProjection);
        }

        [Fact]
        public async Task RetrieveCompanies_ShouldAllCompanies()
        {
            var companies = await sut.RetrieveAllCompanies();
            Assert.Collection(companies.OrderBy(c => c.Name),
                c =>
                {
                    Assert.Equal(fixture.PanasonicCorpId, c.Id);
                    Assert.Equal("Panasonic Corp", c.Name);
                    Assert.Equal("6752", c.Ticker);
                    Assert.Equal("JP3866800000", c.Isin);
                    Assert.Equal("http://www.panasonic.co.jp", c.Website);
                    Assert.Equal("Tokyo Stock Exchange", c.Exchange);
                },
                c =>
                {
                    Assert.Equal(fixture.PorscheAutomobilId, c.Id);
                    Assert.Equal("Porsche Automobil", c.Name);
                    Assert.Equal("PAH3", c.Ticker);
                    Assert.Equal("DE000PAH0038", c.Isin);
                    Assert.Equal("https://www.porsche.com/", c.Website);
                    Assert.Equal("Deutsche Börse", c.Exchange);
                });
        }

        [Fact]
        public async Task RetrieveCompanyById_ShouldReturnCompany()
        {
            var company = await sut.RetrieveCompanyById(fixture.PanasonicCorpId);
            Assert.NotNull(company);
            Assert.Equal(fixture.PanasonicCorpId, company.Id);
            Assert.Equal("Panasonic Corp", company.Name);
            Assert.Equal("6752", company.Ticker);
            Assert.Equal("JP3866800000", company.Isin);
            Assert.Equal("http://www.panasonic.co.jp", company.Website);
            Assert.Equal("Tokyo Stock Exchange", company.Exchange);
        }

        [Fact]
        public async Task RetrieveCompanyByIsin_ShouldReturnCompany()
        {
            var company = await sut.RetrieveCompanybyIsin("DE000PAH0038");
            Assert.NotNull(company);
            Assert.Equal(fixture.PorscheAutomobilId, company.Id);
            Assert.Equal("Porsche Automobil", company.Name);
            Assert.Equal("PAH3", company.Ticker);
            Assert.Equal("DE000PAH0038", company.Isin);
            Assert.Equal("https://www.porsche.com/", company.Website);
            Assert.Equal("Deutsche Börse", company.Exchange);
        }

        [Fact]
        public async Task RetrieveCompanyByIsin_WhenCompanyDoesNotExist_ReturnsNull()
        {
            var company = await sut.RetrieveCompanybyIsin("DE000PAH0039");
            Assert.Null(company);
        }

        [Fact]
        public async Task RetrieveCompanyById_WhenCompanyDoesNotExist_ReturnsNull()
        {
            var company = await sut.RetrieveCompanyById(Guid.NewGuid());
            Assert.Null(company);
        }
    }

    public class RetrieveCompanyDomainServiceFixture
    {
        public readonly CompanyDbContext Context;
        public Guid PanasonicCorpId = Guid.NewGuid();
        public Guid PorscheAutomobilId = Guid.NewGuid();

        private CompanyEntity panasonicCorp => new CompanyEntityBuilder()
            .WithId(PanasonicCorpId)
            .WithName("Panasonic Corp")
            .WithTicker("6752")
            .WithIsin("JP3866800000")
            .WithWebsite("http://www.panasonic.co.jp")
            .WithExchange(builder => builder.WithName("Tokyo Stock Exchange"))
            .Build();

        private CompanyEntity porscheAutomobil => new CompanyEntityBuilder()
            .WithId(PorscheAutomobilId)
            .WithName("Porsche Automobil")
            .WithTicker("PAH3")
            .WithIsin("DE000PAH0038")
            .WithWebsite("https://www.porsche.com/")
            .WithExchange(builder => builder.WithName("Deutsche Börse"))
            .Build();

        public RetrieveCompanyDomainServiceFixture()
        {
            var options = new DbContextOptionsBuilder<CompanyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            Context = new CompanyDbContext(options);

            SeedData().Wait();
        }

        private async Task SeedData()
        {
            await Context.AddRangeAsync(panasonicCorp, porscheAutomobil);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();
        }
    }
}
