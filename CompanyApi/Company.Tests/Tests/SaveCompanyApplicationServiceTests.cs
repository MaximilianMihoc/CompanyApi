using Company.Api.ApplicationServices;
using Company.Api.Commands;
using Company.Api.Domains;
using Company.Api.DomainServices;
using Company.Api.Utils;
using Company.Tests.Data.Builders.Domains;
using Company.Tests.Data.Builders.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Company.Tests.Tests
{
    public class SaveCompanyApplicationServiceTests
    {
        private SaveCompanyApplicationService sut;
        private Mock<ICompanyDomainFactory> mockCompanyDomainFactory;
        private Mock<ISaveCompanyCommand> mockSaveCompanyCommand;

        public SaveCompanyApplicationServiceTests()
        {
            var mockLogger = new Mock<ILogger<SaveCompanyApplicationService>>();
            mockCompanyDomainFactory = new Mock<ICompanyDomainFactory>();
            mockSaveCompanyCommand = new Mock<ISaveCompanyCommand>();

            sut = new SaveCompanyApplicationService(mockCompanyDomainFactory.Object, mockSaveCompanyCommand.Object, mockLogger.Object);
        }

        [Fact]
        public async Task CreateCompany_WhenCompanySubmissionIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            var companySubmission = new CompanySubmissionBuilder().Build();
            var domainResult =  Result<CompanyDomain>.Failure("Company submission cannot be null.");

            mockCompanyDomainFactory
                .Setup(x => x.BuildCompanyDomain(companySubmission))
                .ReturnsAsync(domainResult);

            // Act
            var response = await sut.CreateCompany(companySubmission);

            // Assert
            Assert.False(response.IsSuccess);
            Assert.Equal("Company submission cannot be null.", response.Message);
        }

        [Fact]
        public async Task CreateCompany_WhenCompanySubmissionIsValid_ReturnsCreated()
        {
            // Arrange
            var companySubmission = new CompanySubmissionBuilder().Build();
            var companyDomain = new CompanyDomainBuilder().Build();
            var domainResult = Result<CompanyDomain>.Success(companyDomain);
            mockCompanyDomainFactory
                .Setup(x => x.BuildCompanyDomain(companySubmission))
                .ReturnsAsync(domainResult);

            // Act
            var response = await sut.CreateCompany(companySubmission);

            // Assert
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Data);
            Assert.Equal(companyDomain.Id, response.Data.Id);
        }

        [Fact]
        public async Task UpdateCompany_WhenCompanySubmissionIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            var companySubmission = new CompanySubmissionBuilder().Build();
            var domainResult = Result<CompanyDomain>.Failure("Company submission cannot be null.");

            mockCompanyDomainFactory
                .Setup(x => x.BuildCompanyDomainForUpdate(companySubmission))
                .ReturnsAsync(domainResult);

            // Act
            var response = await sut.UpdateCompany(companySubmission);

            // Assert
            Assert.False(response.IsSuccess);
            Assert.Equal("Company submission cannot be null.", response.Message);
        }

        [Fact]
        public async Task UpdateCompany_WhenCompanySubmissionIsValid_ReturnsCreated()
        {
            // Arrange
            var companySubmission = new CompanySubmissionBuilder().Build();
            var companyDomain = new CompanyDomainBuilder().Build();
            var domainResult = Result<CompanyDomain>.Success(companyDomain);
            mockCompanyDomainFactory
                .Setup(x => x.BuildCompanyDomainForUpdate(companySubmission))
                .ReturnsAsync(domainResult);

            // Act
            var response = await sut.UpdateCompany(companySubmission);

            // Assert
            Assert.True(response.IsSuccess);
        }
    }
}
