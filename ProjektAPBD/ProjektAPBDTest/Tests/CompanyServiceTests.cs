using Xunit;
using ProjektAPBD.Services;
using ProjektAPBD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjektAPBD.Exceptions;
using ProjektAPBD.RequestModels;
using ProjektAPBDTest;

public class CompanyServiceTests : IDisposable
{
    private readonly DbContextOptions<DatabaseContextTest> _options;
    private DatabaseContextTest _context;
    private CompanyService _service;

    public CompanyServiceTests()
    {
        _options = new DbContextOptionsBuilder<DatabaseContextTest>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
    }

    private void InitializeDatabase()
    {
        _context = new DatabaseContextTest(_options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        
        _context.Companies.Add(new Company { CompanyId = 1, CompanyName = "TestCompany", CompanyAddress = "123 Test St", CompanyEmail = "testcompany@gmail.com", CompanyPhone = "123456789", CompanyKRS = "12345678901234" });
        _context.Companies.Add(new Company { CompanyId = 2, CompanyName = "TestCompany2", CompanyAddress = "456 Test St", CompanyEmail = "testcompany2@gmail.com", CompanyPhone = "987654321", CompanyKRS = "98765432101234" });

        _context.SaveChanges();

        _service = new CompanyService(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task PostCompany_WithValidData_ReturnsCompanyId()
    {
        // Arrange
        InitializeDatabase();
        var requestModel = new PostCompanyRequestModel
        {
            CompanyName = "NewCompany",
            CompanyAddress = "789 Test St",
            CompanyEmail = "newcompany@gmail.com",
            CompanyPhone = "456123789",
            CompanyKRS = "45612378901234"
        };

        // Act
        var result = await _service.PostCompany(requestModel);

        // Assert
        Assert.Equal(3, result);
    }

    [Fact]
    public async Task PostCompany_WithExistingKRS_ThrowsArgumentException()
    {
        // Arrange
        InitializeDatabase();
        var requestModel = new PostCompanyRequestModel
        {
            CompanyName = "NewCompany",
            CompanyAddress = "789 Test St",
            CompanyEmail = "newcompany@gmail.com",
            CompanyPhone = "456123789",
            CompanyKRS = "12345678901234"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.PostCompany(requestModel));
    }

    [Fact]
    public async Task UpdateCompany_WithValidData_ReturnsCompanyId()
    {
        // Arrange
        InitializeDatabase();
        var requestModel = new UpdateCompanyRequestModel
        {
            IdCompany = 1,
            CompanyName = "UpdatedCompany",
            CompanyAddress = "789 Test St",
            CompanyEmail = "updatedcompany@gmail.com",
            CompanyPhone = "456123789",
        };

        // Act
        var result = await _service.UpdateCompany(requestModel);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task UpdateCompany_WithNonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        InitializeDatabase();
        var requestModel = new UpdateCompanyRequestModel
        {
            IdCompany = 3,
            CompanyName = "UpdatedCompany",
            CompanyAddress = "789 Test St",
            CompanyEmail = "updatedcompany@gmail.com",
            CompanyPhone = "456123789",
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateCompany(requestModel));
    }
}