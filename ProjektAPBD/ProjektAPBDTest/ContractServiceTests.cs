using Xunit;
using Moq;
using System.Threading.Tasks;
using ProjektAPBD.Services;
using ProjektAPBD.Contexts;
using ProjektAPBD.Models;
using ProjektAPBD.RequestModels.ContractRequestModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjektAPBD.Exceptions;

public class ContractServiceTests
{
    private readonly Mock<DatabaseContext> _mockContext;
    private readonly ContractService _service;

    public ContractServiceTests()
    {
        _mockContext = new Mock<DatabaseContext>();
        _service = new ContractService(_mockContext.Object);
    }

    [Fact]
    public async Task PostContract_WithValidData_ReturnsContractId()
    {
        var requestModel = new PostContractRequestModel
        {
            IdCustomer = 1,
            SoftwareId = 1,
            ContractDateFrom = DateTime.Now.AddDays(1),
            ContractDateTo = DateTime.Now.AddDays(10),
            Price = 1000,
            AdditionalSupportInYears = 1
        };

        var result = await _service.PostContract(requestModel);

        Assert.Equal(1, result);
    }

    [Fact]
    public async Task PostContract_WithInvalidCustomer_ThrowsNotFoundException()
    {
        var requestModel = new PostContractRequestModel
        {
            IdCustomer = 2,
            SoftwareId = 1,
            ContractDateFrom = DateTime.Now.AddDays(1),
            ContractDateTo = DateTime.Now.AddDays(10),
            Price = 1000,
            AdditionalSupportInYears = 1
        };

        await Assert.ThrowsAsync<NotFoundException>(() => _service.PostContract(requestModel));
    }

    [Fact]
    public async Task PostContract_WithInvalidSoftware_ThrowsNotFoundException()
    {
        var requestModel = new PostContractRequestModel
        {
            IdCustomer = 1,
            SoftwareId = 2,
            ContractDateFrom = DateTime.Now.AddDays(1),
            ContractDateTo = DateTime.Now.AddDays(10),
            Price = 1000,
            AdditionalSupportInYears = 1
        };

        await Assert.ThrowsAsync<NotFoundException>(() => _service.PostContract(requestModel));
    }

    [Fact]
    public async Task PostContract_WithInvalidContractDuration_ThrowsArgumentException()
    {
        var requestModel = new PostContractRequestModel
        {
            IdCustomer = 1,
            SoftwareId = 1,
            ContractDateFrom = DateTime.Now.AddDays(1),
            ContractDateTo = DateTime.Now.AddDays(40),
            Price = 1000,
            AdditionalSupportInYears = 1
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.PostContract(requestModel));
    }

    [Fact]
    public async Task PostContract_WithActiveContract_ThrowsAlreadyExistsException()
    {
        var requestModel = new PostContractRequestModel
        {
            IdCustomer = 1,
            SoftwareId = 1,
            ContractDateFrom = DateTime.Now.AddDays(1),
            ContractDateTo = DateTime.Now.AddDays(10),
            Price = 1000,
            AdditionalSupportInYears = 1
        };

        await Assert.ThrowsAsync<AlreadyExistsException>(() => _service.PostContract(requestModel));
    }

    [Fact]
    public async Task PostContract_WithActiveSubscription_ThrowsAlreadyExistsException()
    {
        var requestModel = new PostContractRequestModel
        {
            IdCustomer = 1,
            SoftwareId = 1,
            ContractDateFrom = DateTime.Now.AddDays(1),
            ContractDateTo = DateTime.Now.AddDays(10),
            Price = 1000,
            AdditionalSupportInYears = 1
        };

        await Assert.ThrowsAsync<AlreadyExistsException>(() => _service.PostContract(requestModel));
    }

    [Fact]
    public async Task PostContract_WithInvalidAdditionalSupport_ThrowsArgumentException()
    {
        var requestModel = new PostContractRequestModel
        {
            IdCustomer = 1,
            SoftwareId = 1,
            ContractDateFrom = DateTime.Now.AddDays(1),
            ContractDateTo = DateTime.Now.AddDays(10),
            Price = 1000,
            AdditionalSupportInYears = 4
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.PostContract(requestModel));
    }

    [Fact]
    public async Task GetAllContracts_ReturnsAllContracts()
    {
        var result = await _service.GetAllContracts();

        Assert.Equal(3, result.Count);
    }
}