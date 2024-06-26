﻿using ProjektAPBD.Services;
using ProjektAPBD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjektAPBDTest;

public class IncomeServiceTests : IDisposable
{
    private readonly DbContextOptions<DatabaseContextTest> _options;
    private DatabaseContextTest _context;
    private IncomeService _service;

    public IncomeServiceTests()
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
        
        var company = new Company { CompanyId = 1, CompanyName = "TestCompany", CompanyAddress = "123 Test St", CompanyEmail = "testcompany@gmail.com", CompanyPhone = "123456789", CompanyKRS = "12345678901234" };
        _context.Companies.Add(company);

        var person = new Person { PersonId = 1, PersonFirstName = "Jan", PersonLastName = "Kowalski", PersonAddress = "Test address", PersonEmail = "jankowalski@gmail.com", PersonPhone = "098765432", PersonPesel = "12345678901", PersonSoftDelete = false };
        _context.Persons.Add(person);

        var customer = new Customer { CustomerId = 1, PersonId = 1, CompanyId = null };
        _context.Customers.Add(customer);
        
        var customer2 = new Customer { CustomerId = 2, PersonId = null, CompanyId = 1 };
        _context.Customers.Add(customer2);

        var software = new Software { SoftwareId = 1, SoftwareName = "Test", SoftwareDescription = "This is a test software.", SoftwareCurrentVersion = "1.2", SoftwareCategory = "Finance" };
        _context.Software.Add(software);

        var contract = new Contract { IdContract = 1, IdCustomer = 1, IdSoftware = 1, Signed = true, TotalPrice = 1000 };
        _context.Contracts.Add(contract);

        var paymentContract = new PaymentContract { PaymentId = 1, ContractId = 1, PaymentDate = DateTime.Now, Amount = 1000, PaymentInformation = "Test Payment" };
        _context.PaymentsContracts.Add(paymentContract);

        var subscription = new Subscription { SubscriptionId = 1, CustomerId = 1, SoftwareId = 1, SubscriptionName = "Test Subscription", RenewalDate = DateTime.Now.AddMonths(1), PricePerMonth = 100, FirstPaymentPrice = 100, ActiveStatus = true, ActivationDate = DateTime.Now };
        _context.Subscriptions.Add(subscription);

        var paymentSubscription = new PaymentSubscription { PaymentId = 1, SubscriptionId = 1, PaymentDate = DateTime.Now, Amount = 100 };
        _context.PaymentsSubscriptions.Add(paymentSubscription);

        _context.SaveChanges();

        _service = new IncomeService(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task GetIncomeForWholeCompanyCurrent_WithNullCurrencyCode_ReturnsTotalIncome()
    {
        // Arrange
        InitializeDatabase();

        // Act
        var result = await _service.GetIncomeForWholeCompanyCurrent(null);

        // Assert
        Assert.Equal(1100, result);
    }

    [Fact]
    public async Task GetIncomeForWholeCompanyCurrent_WithInvalidCurrencyCode_ThrowsArgumentException()
    {
        // Arrange
        InitializeDatabase();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.GetIncomeForWholeCompanyCurrent("invalid"));
    }

    [Fact]
    public async Task GetIncomeForSoftwareCurrentCurrent_WithInvalidSoftwareId_ThrowsArgumentException()
    {
        // Arrange
        InitializeDatabase();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.GetIncomeForSoftwareCurrentCurrent(-1, null));
    }
    [Fact]
    public async Task GetIncomeForWholeCompanyExcepted_WithNullCurrencyCode_ReturnsTotalIncome()
    {
        // Arrange
        InitializeDatabase();

        // Act
        var result = await _service.GetIncomeForWholeCompanyExcepted(null);

        // Assert
        Assert.Equal(1200, result);
    }

    [Fact]
    public async Task GetIncomeForWholeCompanyExcepted_WithInvalidCurrencyCode_ThrowsArgumentException()
    {
        // Arrange
        InitializeDatabase();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.GetIncomeForWholeCompanyExcepted("invalid"));
    }

    [Fact]
    public async Task GetIncomeForSoftwareCurrentExcepted_WithInvalidSoftwareId_ThrowsArgumentException()
    {
        // Arrange
        InitializeDatabase();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.GetIncomeForSoftwareCurrentExcepted(-1, null));
    }

    [Fact]
    public async Task GetIncomeForSoftwareCurrentExcepted_WithValidSoftwareIdAndNullCurrencyCode_ReturnsTotalIncome()
    {
        // Arrange
        InitializeDatabase();

        // Act
        var result = await _service.GetIncomeForSoftwareCurrentExcepted(1, null);

        // Assert
        Assert.Equal(1200, result);
    }

    [Fact]
    public async Task GetIncomeForSoftwareCurrentExcepted_WithValidSoftwareIdAndInvalidCurrencyCode_ThrowsArgumentException()
    {
        // Arrange
        InitializeDatabase();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.GetIncomeForSoftwareCurrentExcepted(1, "invalid"));
    }
}
