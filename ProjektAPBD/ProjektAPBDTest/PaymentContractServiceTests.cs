using Microsoft.EntityFrameworkCore;
using ProjektAPBD.Contexts;
using ProjektAPBD.Exceptions;
using ProjektAPBD.Models;
using ProjektAPBD.RequestModels.PaymentContractRequestModels;
using ProjektAPBD.Services;
using ProjektAPBDTest;

public class PaymentContractServiceTests : IDisposable
{
    private readonly DbContextOptions<DatabaseContext> _options;
    private DatabaseContextTest _context;
    private PaymentContractService _service;

    public PaymentContractServiceTests()
    {
        _options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    private void InitializeDatabase()
    {
        _context = new DatabaseContextTest(_options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        
        _context.Contracts.Add(new Contract { IdContract = 1, Signed = true, TotalPrice = 1000 });
        _context.Contracts.Add(new Contract { IdContract = 2, Signed = true, TotalPrice = 2000 });
        _context.Contracts.Add(new Contract { IdContract = 3, Signed = false, TotalPrice = 3000, DateTo = DateTime.Now.AddDays(5)});

        _context.SaveChanges();

        _service = new PaymentContractService(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task PostPaymentContract_WithValidData_ReturnsPaymentContractId()
    {
        // Arrange
        InitializeDatabase();
        var requestModel = new PostPaymentContractRequestModel
        {
            ContractId = 3,
            PaymentDate = DateTime.Now,
            Amount = 1000,
            PaymentInformation = "Test Payment"
        };

        // Act
        var result = await _service.PostPaymentContract(requestModel);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task PostPaymentContract_WithInvalidContract_ThrowsNotFoundException()
    {
        // Arrange
        InitializeDatabase();
        var requestModel = new PostPaymentContractRequestModel
        {
            ContractId = 4,
            PaymentDate = DateTime.Now,
            Amount = 1000,
            PaymentInformation = "Test Payment"
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.PostPaymentContract(requestModel));
    }

    [Fact]
    public async Task PostPaymentContract_WithExceedingAmount_ThrowsArgumentException()
    {
        // Arrange
        InitializeDatabase();
        var requestModel = new PostPaymentContractRequestModel
        {
            ContractId = 1,
            PaymentDate = DateTime.Now,
            Amount = 2000,
            PaymentInformation = "Test Payment"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.PostPaymentContract(requestModel));
    }

    [Fact]
    public async Task GetAllPaymentsContracts_ReturnsAllPaymentContracts()
    {
        // Arrange
        InitializeDatabase();

        _context.PaymentsContracts.Add(new PaymentContract { PaymentId = 1, ContractId = 1, PaymentDate = DateTime.Now, Amount = 1000, PaymentInformation = "Test Payment 1" });
        _context.PaymentsContracts.Add(new PaymentContract { PaymentId = 2, ContractId = 2, PaymentDate = DateTime.Now, Amount = 2000, PaymentInformation = "Test Payment 2" });
        _context.PaymentsContracts.Add(new PaymentContract { PaymentId = 3, ContractId = 3, PaymentDate = DateTime.Now, Amount = 3000, PaymentInformation = "Test Payment 3" });

        _context.SaveChanges();

        // Act
        var result = await _service.GetAllPaymentsContracts();

        // Assert
        Assert.Equal(3, result.Count);
    }
}
