using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjektAPBD.Exceptions;
using ProjektAPBD.Models;
using ProjektAPBD.RequestModels.PaymentSubcriptionRequestModels;
using ProjektAPBD.Services;

namespace ProjektAPBDTest.Tests;

public class PaymentSubscriptionServiceTests : IDisposable
{
    private readonly DbContextOptions<DatabaseContextTest> _options;
    private DatabaseContextTest _context;
    private PaymentSubscriptionService _service;

    public PaymentSubscriptionServiceTests()
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
        
        var subscription = new Subscription { SubscriptionId = 1, CustomerId = 1, SoftwareId = 1, SubscriptionName = "Test Subscription", RenewalDate = DateTime.Now.AddMonths(1), PricePerMonth = 100, FirstPaymentPrice = 100, ActiveStatus = true, ActivationDate = DateTime.Now };
        _context.Subscriptions.Add(subscription);
        
        var subscription2 = new Subscription { SubscriptionId = 2, CustomerId = 1, SoftwareId = 1, SubscriptionName = "Test Subscription", RenewalDate = DateTime.Now.AddMonths(1), PricePerMonth = 100, FirstPaymentPrice = 100, ActiveStatus = false, ActivationDate = DateTime.Now };
        _context.Subscriptions.Add(subscription2);
        
        var paymentSubscription = new PaymentSubscription { PaymentId = 1, SubscriptionId = 1, PaymentDate = DateTime.Now, Amount = 100 };
        _context.PaymentsSubscriptions.Add(paymentSubscription);

        _context.SaveChanges();

        _service = new PaymentSubscriptionService(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
    
    [Fact]
    public async Task PostPaymentSubscription_WithValidData_ReturnsPaymentId()
    {
        // Arrange
        InitializeDatabase();
        var requestModel = new PostPaymentSubscriptionRequestModel
        {
            SubscriptionId = 1,
            PaymentDate = DateTime.Now.AddMonths(1),
            Amount = 100
        };
    
        // Act
        var result = await _service.PostPaymentSubscription(requestModel);
    
        // Assert
        Assert.Equal(2, result);
    }
    
    [Fact]
    public async Task PostPaymentSubscription_WithInvalidSubscriptionId_ThrowsNotFoundException()
    {
        // Arrange
        InitializeDatabase();
        var requestModel = new PostPaymentSubscriptionRequestModel
        {
            SubscriptionId = 999, 
            PaymentDate = DateTime.Now,
            Amount = 100
        };
    
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.PostPaymentSubscription(requestModel));
    }
    
    [Fact]
    public async Task PostPaymentSubscription_WithIncorrectAmount_ThrowsArgumentException()
    {
        // Arrange
        InitializeDatabase();
        var requestModel = new PostPaymentSubscriptionRequestModel
        {
            SubscriptionId = 1,
            PaymentDate = DateTime.Now,
            Amount = 999 
        };
    
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.PostPaymentSubscription(requestModel));
    }
    
    [Fact]
    public async Task PostPaymentSubscription_WithInactiveSubscription_ThrowsArgumentException()
    {
        // Arrange
        InitializeDatabase();
        var requestModel = new PostPaymentSubscriptionRequestModel
        {
            SubscriptionId = 2, 
            PaymentDate = DateTime.Now,
            Amount = 100
        };
    
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.PostPaymentSubscription(requestModel));
    }
    
    [Fact]
    public async Task PostPaymentSubscription_WithPaymentDateAfterRenewalDate_ThrowsArgumentException()
    {
        // Arrange
        InitializeDatabase();
        var requestModel = new PostPaymentSubscriptionRequestModel
        {
            SubscriptionId = 1,
            PaymentDate = DateTime.Now.AddDays(2),
            Amount = 100
        };
    
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.PostPaymentSubscription(requestModel));
    }
    
    [Fact]
    public async Task GetAllPaymentSubscriptions_ReturnsAllPaymentSubscriptions()
    {
        // Arrange
        InitializeDatabase();
    
        // Act
        var result = await _service.GetAllPaymentSubscriptions();
    
        // Assert
        Assert.Equal(1, result.Count);
    }
}