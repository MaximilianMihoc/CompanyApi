using Company.Api.DomainServices;
using Company.Tests.Data.Builders.Models;
using Moq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        public async Task BuildCompanyDomain_WhenCompanyIsNull_ReturnsFailure()
        {
            var result = await sut.BuildCompanyDomain(null);
            Assert.False(result.IsSuccess);
            Assert.Equal("Company submission cannot be null.", result.Error);
        }

        [Fact]
        public async Task BuildCompanyDomain_WhenIsinIsInvalid_ReturnsFailure()
        {
            var company = new CompanySubmissionBuilder().WithIsin("123").Build();
            var result = await sut.BuildCompanyDomain(company);
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid ISIN format.", result.Error);
        }

        [Fact]
        public async Task BuildCompanyDomain_WhenCompanyAlreadyExists_ReturnsFailure()
        {
            var company = new CompanySubmissionBuilder().WithIsin("AA123").Build();
            mockCompanyDomainService
                .Setup(x => x.IsIsinUsed(company.Isin, null))
                .ReturnsAsync(true);

            var result = await sut.BuildCompanyDomain(company);
            Assert.False(result.IsSuccess);
            Assert.Equal("A company with ISIN AA123 already exists.", result.Error);
        }

        [Fact]
        public async Task BuildCompanyDomain_WhenCompanyIsValid_ReturnsSuccess()
        {
            var exchangeId = Guid.NewGuid();
            var company = new CompanySubmissionBuilder()
                .WithIsin("AA123")
                .WithName("Company Name")
                .WithExchangeId(exchangeId)
                .WithTicker("Ticker")
                .WithWebsite("Website")
                .Build();

            var result = await sut.BuildCompanyDomain(company);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            var companyDomain = result.Value;
            Assert.Equal(company.Name, companyDomain.Name);
            Assert.Equal(company.ExchangeId, companyDomain.ExchangeId);
            Assert.Equal(company.Ticker, companyDomain.Ticker);
            Assert.Equal(company.Isin, companyDomain.Isin);
            Assert.Equal(company.Website, companyDomain.Website);
        }

        [Fact]
        public async Task BuildCompanyDomainForUpdate_WhenCompanyIsNull_ReturnsFailure()
        {
            var result = await sut.BuildCompanyDomainForUpdate(null);
            Assert.False(result.IsSuccess);
            Assert.Equal("Company submission cannot be null.", result.Error);
        }

        [Fact]
        public async Task BuildCompanyDomainForUpdate_WhenCompanyDoesNotExist_ReturnsFailure()
        {
            var company = new CompanySubmissionBuilder().WithIsin("123").Build();
            var result = await sut.BuildCompanyDomainForUpdate(company);
            Assert.False(result.IsSuccess);
            Assert.Equal($"A company with Id {company.Id} does not exist.", result.Error);
        }

        [Fact]
        public async Task BuildCompanyDomainForUpdate_WhenIsinIsInvalid_ReturnsFailure()
        {
            var companyId = Guid.NewGuid();

            var companySubmission = new CompanySubmissionBuilder()
                .WithId(companyId)
                .WithIsin("123")
                .Build();

            var companyA = new CompanyResponseBuilder()
                .WithId(companyId)
                .WithName("Company A")
                .WithIsin("AB123")
                .Build();

            mockCompanyDomainService
                .Setup(x => x.RetrieveCompanyById(companyId))
                .ReturnsAsync(companyA);

            var result = await sut.BuildCompanyDomainForUpdate(companySubmission);

            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid ISIN format.", result.Error);
        }

        [Fact]
        public async Task BuildCompanyDomainForUpdate_WhenIsinIsAlreadyUsed_ReturnsFailure()
        {
            var companyId = Guid.NewGuid();

            var companySubmission = new CompanySubmissionBuilder()
                .WithId(companyId)
                .WithIsin("AB123")
                .Build();

            var companyA = new CompanyResponseBuilder()
                .WithId(companyId)
                .WithName("Company A")
                .WithIsin("AB12351")
                .Build();

            mockCompanyDomainService
                .Setup(x => x.RetrieveCompanyById(companyId))
                .ReturnsAsync(companyA);

            mockCompanyDomainService
                .Setup(x => x.IsIsinUsed(companySubmission.Isin, companyId))
                .ReturnsAsync(true);

            var result = await sut.BuildCompanyDomainForUpdate(companySubmission);

            Assert.False(result.IsSuccess);
            Assert.Equal($"A company with ISIN {companySubmission.Isin} already exists.", result.Error);
        }

        [Fact]
        public async Task BuildCompanyDomainForUpdate_WhenCompanySubmissionIsValid_ReturnsSuccess()
        {
            var companyId = Guid.NewGuid();
            var exchangeId = Guid.NewGuid();
            var companySubmission = new CompanySubmissionBuilder()
                .WithId(companyId)
                .WithIsin("AA123")
                .WithName("Company Name")
                .WithExchangeId(exchangeId)
                .WithTicker("Ticker")
                .WithWebsite("Website")
                .Build();

            var companyA = new CompanyResponseBuilder()
                .WithId(companyId)
                .WithName("Company A")
                .WithIsin("AB12351")
                .WithTicker("Ticker")
                .WithWebsite("Website")
                .Build();

            mockCompanyDomainService
                .Setup(x => x.RetrieveCompanyById(companyId))
                .ReturnsAsync(companyA);

            mockCompanyDomainService
                .Setup(x => x.IsIsinUsed(companySubmission.Isin, companyId))
                .ReturnsAsync(false);

            var result = await sut.BuildCompanyDomainForUpdate(companySubmission);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            var companyDomain = result.Value;
            Assert.Equal(companySubmission.Name, companyDomain.Name);
            Assert.Equal(companySubmission.Ticker, companyDomain.Ticker);
            Assert.Equal(companySubmission.Isin, companyDomain.Isin);
            Assert.Equal(companySubmission.Website, companyDomain.Website);
        }
    }
}
