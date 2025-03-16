using Company.Api.ApplicationServices;
using Company.Api.DomainServices;
using Company.Tests.Data.Builders.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Company.Tests.Tests
{
    public class RetrieveCompanyApplicationServiceTests
    {
        private RetrieveCompanyApplicationService sut;
        private Mock<IRetrieveCompanyDomainService> mockCompanyDomainService;

        public RetrieveCompanyApplicationServiceTests()
        {
            mockCompanyDomainService = new Mock<IRetrieveCompanyDomainService>();
            var mockLogger = new Mock<ILogger<RetrieveCompanyApplicationService>>();

            sut = new RetrieveCompanyApplicationService(mockCompanyDomainService.Object, mockLogger.Object);
        }

        [Fact]
        public async Task RetrieveAllCompanies_WhenNoCompaniesExists_ReturnsEmptyList()
        {
            // Arrange
            mockCompanyDomainService
                .Setup(x => x.RetrieveAllCompanies())
                .ReturnsAsync([]);

            // Act
            var companiesResponse = await sut.RetrieveAllCompanies();

            // Assert
            Assert.True(companiesResponse.IsSuccess);
            Assert.NotNull(companiesResponse);
            Assert.NotNull(companiesResponse.Data);
            Assert.Empty(companiesResponse.Data);
        }

        [Fact]
        public async Task RetrieveAllCompanies_WhenCompaniesDoExists_ReturnsList()
        {
            // Arrange
            var companyA = new CompanyResponseBuilder().WithName("Company A").Build();
            var companyB = new CompanyResponseBuilder().WithName("Company B").Build();

            mockCompanyDomainService
                .Setup(x => x.RetrieveAllCompanies())
                .ReturnsAsync([companyA, companyB]);

            // Act
            var companiesResponse = await sut.RetrieveAllCompanies();

            // Assert
            Assert.True(companiesResponse.IsSuccess);
            Assert.NotNull(companiesResponse);
            Assert.NotNull(companiesResponse.Data);
            Assert.Collection(companiesResponse.Data.OrderBy(c => c.Name),
                c => Assert.Equal("Company A", c.Name),
                c => Assert.Equal("Company B", c.Name));
        }

        [Fact]
        public async Task RetrieveCompanyById_WhenNoCompaniesExistsAndEmptyGuidIsPassed_ReturnsFailure()
        {
            // Arrange
            var companyA = new CompanyResponseBuilder().WithName("Company A").Build();
            mockCompanyDomainService
                .Setup(x => x.RetrieveCompanyById(It.IsAny<Guid>()))
                .ReturnsAsync(companyA);

            // Act
            var companiesResponse = await sut.RetrieveCompanyById(Guid.Empty);

            // Assert
            Assert.False(companiesResponse.IsSuccess);
            Assert.NotNull(companiesResponse);
            Assert.Null(companiesResponse.Data);
            Assert.Equal("Invalid company Id provided.", companiesResponse.Message);
        }

        [Fact]
        public async Task RetrieveCompanyById_WhenNoCompaniesExists_ReturnsFailure()
        {
            // Act
            var companiesResponse = await sut.RetrieveCompanyById(Guid.NewGuid());

            // Assert
            Assert.False(companiesResponse.IsSuccess);
            Assert.NotNull(companiesResponse);
            Assert.Null(companiesResponse.Data);
            Assert.Equal("Company not found.", companiesResponse.Message);
        }

        [Fact]
        public async Task RetrieveCompanyById_WhenCompanyExists_ReturnsCompany()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var companyA = new CompanyResponseBuilder()
                .WithId(companyId)
                .WithName("Company A")
                .Build();

            mockCompanyDomainService
                .Setup(x => x.RetrieveCompanyById(It.IsAny<Guid>()))
                .ReturnsAsync(companyA);

            // Act
            var companiesResponse = await sut.RetrieveCompanyById(companyId);

            // Assert
            Assert.True(companiesResponse.IsSuccess);
            Assert.NotNull(companiesResponse);
            Assert.NotNull(companiesResponse.Data);
            var company = companiesResponse.Data;
            Assert.Equal(companyId, company.Id);
            Assert.Equal("Company A", company.Name);
            Assert.Equal("Isin", company.Isin);
        }

        [Fact]
        public async Task RetrieveCompanyByIsin_WhenNoCompaniesExists_ReturnsFailure()
        {
            // Act
            var companiesResponse = await sut.RetrieveCompanyByIsin("AB123");

            // Assert
            Assert.False(companiesResponse.IsSuccess);
            Assert.NotNull(companiesResponse);
            Assert.Null(companiesResponse.Data);
            Assert.Equal("Company not found.", companiesResponse.Message);
        }

        [Fact]
        public async Task RetrieveCompanyByIsin_WhenCompanyExists_ReturnsCompany()
        {
            // Arrange
            var isin = "US0378331005";
            var companyA = new CompanyResponseBuilder()
                .WithName("Apple Inc.")
                .WithTicker("AAPL")
                .WithIsin(isin)
                .Build();

            mockCompanyDomainService
                .Setup(x => x.RetrieveCompanybyIsin(It.IsAny<string>()))
                .ReturnsAsync(companyA);

            // Act
            var companiesResponse = await sut.RetrieveCompanyByIsin(isin);

            // Assert
            Assert.True(companiesResponse.IsSuccess);
            Assert.NotNull(companiesResponse);
            Assert.NotNull(companiesResponse.Data);

            var company = companiesResponse.Data;
            Assert.Equal("Apple Inc.", company.Name);
            Assert.Equal("AAPL", company.Ticker);
            Assert.Equal(isin, company.Isin);
        }

    }
}
