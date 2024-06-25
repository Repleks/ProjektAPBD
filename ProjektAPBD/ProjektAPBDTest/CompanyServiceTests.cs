using Xunit;
using Moq;
using System.Threading.Tasks;
using ProjektAPBD.Services;
using ProjektAPBD.Contexts;
using ProjektAPBD.Models;
using ProjektAPBD.RequestModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjektAPBD.Exceptions;

public class CompanyServiceTests
{
    private readonly Mock<DbSet<Company>> _mockSet;
    private readonly Mock<DatabaseContext> _mockContext;
    private readonly CompanyService _service;

    public CompanyServiceTests()
    {
        _mockSet = new Mock<DbSet<Company>>();
        _mockContext = new Mock<DatabaseContext>();
        _mockContext.Setup(c => c.Companies).Returns(_mockSet.Object);
        _service = new CompanyService(_mockContext.Object);
    }

    [Fact]
    public async Task PostCompany_WithValidData_ReturnsCompanyId()
    {
        // Arrange
        var requestModel = new PostCompanyRequestModel
        {
            CompanyName = "Test Company",
            CompanyAddress = "Test Address",
            CompanyEmail = "test@test.com",
            CompanyPhone = "123456789",
            CompanyKRS = "1234567890"
        };

        // Act
        var result = await _service.PostCompany(requestModel);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task PostCompany_WithExistingKRS_ThrowsArgumentException()
    {
        // Arrange
        var requestModel = new PostCompanyRequestModel
        {
            CompanyName = "Test Company",
            CompanyAddress = "Test Address",
            CompanyEmail = "test@test.com",
            CompanyPhone = "123456789",
            CompanyKRS = "1234567890"
        };

        var companies = new List<Company>
        {
            new Company { CompanyKRS = "1234567890" }
        }.AsQueryable();

        _mockSet.As<IQueryable<Company>>().Setup(m => m.Provider).Returns(companies.Provider);
        _mockSet.As<IQueryable<Company>>().Setup(m => m.Expression).Returns(companies.Expression);
        _mockSet.As<IQueryable<Company>>().Setup(m => m.ElementType).Returns(companies.ElementType);
        _mockSet.As<IQueryable<Company>>().Setup(m => m.GetEnumerator()).Returns(companies.GetEnumerator());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.PostCompany(requestModel));
    }

    [Fact]
    public async Task UpdateCompany_WithValidData_ReturnsCompanyId()
    {
        // Arrange
        var requestModel = new UpdateCompanyRequestModel
        {
            IdCompany = 1,
            CompanyName = "Updated Company",
            CompanyAddress = "Updated Address",
            CompanyEmail = "updated@test.com",
            CompanyPhone = "987654321",
        };

        var companies = new List<Company>
        {
            new Company { CompanyId = 1, CompanyName = "Test Company", CompanyAddress = "Test Address", CompanyEmail = "test@test.com", CompanyPhone = "123456789", CompanyKRS = "1234567890" }
        }.AsQueryable();

        _mockSet.As<IQueryable<Company>>().Setup(m => m.Provider).Returns(companies.Provider);
        _mockSet.As<IQueryable<Company>>().Setup(m => m.Expression).Returns(companies.Expression);
        _mockSet.As<IQueryable<Company>>().Setup(m => m.ElementType).Returns(companies.ElementType);
        _mockSet.As<IQueryable<Company>>().Setup(m => m.GetEnumerator()).Returns(companies.GetEnumerator());

        // Act
        var result = await _service.UpdateCompany(requestModel);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task UpdateCompany_WithNonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        var requestModel = new UpdateCompanyRequestModel
        {
            IdCompany = 2,
            CompanyName = "Updated Company",
            CompanyAddress = "Updated Address",
            CompanyEmail = "updated@test.com",
            CompanyPhone = "987654321",
        };

        var companies = new List<Company>
        {
            new Company { CompanyId = 1, CompanyName = "Test Company", CompanyAddress = "Test Address", CompanyEmail = "test@test.com", CompanyPhone = "123456789", CompanyKRS = "1234567890" }
        }.AsQueryable();

        _mockSet.As<IQueryable<Company>>().Setup(m => m.Provider).Returns(companies.Provider);
        _mockSet.As<IQueryable<Company>>().Setup(m => m.Expression).Returns(companies.Expression);
        _mockSet.As<IQueryable<Company>>().Setup(m => m.ElementType).Returns(companies.ElementType);
        _mockSet.As<IQueryable<Company>>().Setup(m => m.GetEnumerator()).Returns(companies.GetEnumerator());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateCompany(requestModel));
    }
}