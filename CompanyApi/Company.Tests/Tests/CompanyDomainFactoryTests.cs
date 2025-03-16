using Company.Api.DomainServices;
using Company.Tests.Data.Builders.Models;
using Moq;

namespace Company.Tests.Tests
{
    public class CompanyDomainFactoryTests
    {
        private CompanyDomainFactory sut;
        private Mock<IRetrieveCompanyDomainService> mockCompanyDomainService;

        public CompanyDomainFactoryTests()
        {
            mockCompanyDomainService = new Mock<IRetrieveCompanyDomainService>();
            sut = new CompanyDomainFactory(mockCompanyDomainService.Object);
        }

        [Fact]
        public void BuildCompanyDomain_WhenCompanyIsNull_ReturnsFailure()
        {
            var result = sut.BuildCompanyDomain(null);
            Assert.False(result.IsSuccess);
            Assert.Equal("Company submission cannot be null.", result.Error);
        }

        [Fact]
        public void BuildCompanyDomain_WhenIsinIsInvalid_ReturnsFailure()
        {
            var company = new CompanySubmissionBuilder().WithIsin("123").Build();
            var result = sut.BuildCompanyDomain(company);
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid ISIN format.", result.Error);
        }

        [Fact]
        public void BuildCompanyDomain_WhenCompanyAlreadyExists_ReturnsFailure()
        {
            var company = new CompanySubmissionBuilder().WithIsin("AA123").Build();
            mockCompanyDomainService
                .Setup(x => x.RetrieveCompanybyIsin(company.Isin))
                .ReturnsAsync(new CompanyResponseBuilder().Build());

            var result = sut.BuildCompanyDomain(company);
            Assert.False(result.IsSuccess);
            Assert.Equal("A company with ISIN AA123 already exists.", result.Error);
        }

        [Fact]
        public void BuildCompanyDomain_WhenCompanyIsValid_ReturnsSuccess()
        {
            var exchangeId = Guid.NewGuid();
            var company = new CompanySubmissionBuilder()
                .WithIsin("AA123")
                .WithName("Company Name")
                .WithExchangeId(exchangeId)
                .WithTicker("Ticker")
                .WithWebsite("Website")
                .Build();

            var result = sut.BuildCompanyDomain(company);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            var companyDomain = result.Value;
            Assert.Equal(company.Name, companyDomain.Name);
            Assert.Equal(company.ExchangeId, companyDomain.ExchangeId);
            Assert.Equal(company.Ticker, companyDomain.Ticker);
            Assert.Equal(company.Isin, companyDomain.Isin);
            Assert.Equal(company.Website, companyDomain.Website);
        }
    }
}
