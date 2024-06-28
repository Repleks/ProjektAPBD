using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjektAPBD.Models;
using ProjektAPBD.RequestModels.SubscriptionRequestModels;
using ProjektAPBD.Services;

namespace ProjektAPBDTest.Tests;

public class SubscriptionServiceTests : IDisposable
{
    private readonly DbContextOptions<DatabaseContextTest> _options;
    private DatabaseContextTest _context;
    private SubcriptionService _service;

    public SubscriptionServiceTests()
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
        
        var customer = new Customer { CustomerId = 1, PersonId = null, CompanyId = 1 };
        _context.Customers.Add(customer);

        var software = new Software { SoftwareId = 1, SoftwareName = "Test", SoftwareDescription = "This is a test software.", SoftwareCurrentVersion = "1.2", SoftwareCategory = "Finance" };
        _context.Software.Add(software);

        var subscription = new Subscription { SubscriptionId = 1, CustomerId = 1, SoftwareId = 1, SubscriptionName = "Test Subscription", RenewalDate = DateTime.Now.AddMonths(1), PricePerMonth = 100, FirstPaymentPrice = 100, ActiveStatus = true, ActivationDate = DateTime.Now };
        _context.Subscriptions.Add(subscription);
        
        _context.SaveChanges();

        _service = new SubcriptionService(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task PostSubscription_WithValidData_ReturnsSubscriptionId()
    {
        // Arrange
        InitializeDatabase();
        var requestModel = new PostSubscriptionRequestModel
        {
            CustomerId = 1,
            SoftwareId = 1,
            SubscriptionName = "Test Subscription",
            RenewalDate = DateTime.Now.AddMonths(12),
            PricePerMonth = 100
        };

        // Act
        var result = await _service.PostSubcription(requestModel);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task PostSubscription_WithInvalidData_ThrowsArgumentException()
    {
        // Arrange
        InitializeDatabase();
        var requestModel = new PostSubscriptionRequestModel
        {
            CustomerId = 1,
            SoftwareId = 1,
            SubscriptionName = "Test Subscription",
            RenewalDate = DateTime.Now.AddYears(3),
            PricePerMonth = 100
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.PostSubcription(requestModel));
    }

    [Fact]
    public async Task GetAllSubscription_ReturnsAllSubscriptions()
    {
        // Arrange
        InitializeDatabase();

        // Act
        var result = await _service.GetAllSubcription();

        // Assert
        Assert.Equal(1, result.Count);
    }
    
}