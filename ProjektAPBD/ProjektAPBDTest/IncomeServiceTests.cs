using Xunit;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net;
using ProjektAPBD.Contexts;
using ProjektAPBD.Models;
using ProjektAPBD.Services;

public class IncomeServiceTests
{
    private readonly Mock<DatabaseContext> _mockContext;
    private readonly IncomeService _service;

    public IncomeServiceTests()
    {
        _mockContext = new Mock<DatabaseContext>();
        _service = new IncomeService(_mockContext.Object);
    }

    [Fact]
    public async Task GetIncomeForWholeCompanyCurrent_WithNullCurrencyCode_ReturnsTotalIncome()
    {
        // Arrange
        var contracts = GetTestContracts();
        _mockContext.Setup(db => db.Contracts).Returns(contracts.Object);

        // Act
        var result = await _service.GetIncomeForWholeCompanyCurrent(null);

        // Assert
        Assert.Equal(3000, result);
    }

    [Fact]
    public async Task GetIncomeForWholeCompanyCurrent_WithInvalidCurrencyCode_ThrowsArgumentException()
    {
        // Arrange
        var contracts = GetTestContracts();
        _mockContext.Setup(db => db.Contracts).Returns(contracts.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.GetIncomeForWholeCompanyCurrent("invalid"));
    }

    [Fact]
    public async Task GetIncomeForSoftwareCurrentCurrent_WithInvalidSoftwareId_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.GetIncomeForSoftwareCurrentCurrent(-1, null));
    }

    private Mock<DbSet<Contract>> GetTestContracts()
    {
        var data = new List<Contract>
        {
            new Contract { Signed = true, TotalPrice = 1000 },
            new Contract { Signed = true, TotalPrice = 2000 },
            new Contract { Signed = false, TotalPrice = 3000 }
        }.AsQueryable();

        var mockSet = new Mock<DbSet<Contract>>();
        mockSet.As<IQueryable<Contract>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<Contract>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<Contract>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<Contract>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        return mockSet;
    }
}