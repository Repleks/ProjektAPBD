using Xunit;
using Moq;
using System.Threading.Tasks;
using ProjektAPBD.Services;
using ProjektAPBD.Contexts;
using ProjektAPBD.Models;
using ProjektAPBD.RequestModels.PaymentContractRequestModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjektAPBD.Exceptions;

public class PaymentContractServiceTests
{
    private readonly Mock<DatabaseContext> _mockContext;
    private readonly PaymentContractService _service;

    public PaymentContractServiceTests()
    {
        _mockContext = new Mock<DatabaseContext>();
        _service = new PaymentContractService(_mockContext.Object);
    }

    [Fact]
    public async Task PostPaymentContract_WithValidData_ReturnsPaymentContractId()
    {
        var requestModel = new PostPaymentContractRequestModel
        {
            ContractId = 1,
            PaymentDate = DateTime.Now,
            Amount = 1000,
            PaymentInformation = "Test Payment"
        };

        var result = await _service.PostPaymentContract(requestModel);

        Assert.Equal(1, result);
    }

    [Fact]
    public async Task PostPaymentContract_WithInvalidContract_ThrowsNotFoundException()
    {
        var requestModel = new PostPaymentContractRequestModel
        {
            ContractId = 2,
            PaymentDate = DateTime.Now,
            Amount = 1000,
            PaymentInformation = "Test Payment"
        };

        await Assert.ThrowsAsync<NotFoundException>(() => _service.PostPaymentContract(requestModel));
    }

    [Fact]
    public async Task PostPaymentContract_WithExceedingAmount_ThrowsArgumentException()
    {
        var requestModel = new PostPaymentContractRequestModel
        {
            ContractId = 1,
            PaymentDate = DateTime.Now,
            Amount = 2000,
            PaymentInformation = "Test Payment"
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.PostPaymentContract(requestModel));
    }

    [Fact]
    public async Task PostPaymentContract_WithLatePayment_ThrowsTooLateException()
    {
        var requestModel = new PostPaymentContractRequestModel
        {
            ContractId = 1,
            PaymentDate = DateTime.Now.AddDays(1),
            Amount = 1000,
            PaymentInformation = "Test Payment"
        };

        await Assert.ThrowsAsync<TooLateException>(() => _service.PostPaymentContract(requestModel));
    }

    [Fact]
    public async Task GetAllPaymentsContracts_ReturnsAllPaymentContracts()
    {
        var result = await _service.GetAllPaymentsContracts();

        Assert.Equal(3, result.Count);
    }
}