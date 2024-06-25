using ProjektAPBD.Services;
using ProjektAPBD.Contexts;
using ProjektAPBD.RequestModels.ContractRequestModels;
using Microsoft.EntityFrameworkCore;
using ProjektAPBD.Exceptions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjektAPBD.Models;
using ProjektAPBDTest;


public class ContractServiceTests : IDisposable
{
    private readonly DbContextOptions<DatabaseContextTest> _options;
    private DatabaseContextTest _context;
    private ContractService _service;

    public ContractServiceTests()
    {
        _options = new DbContextOptionsBuilder<DatabaseContextTest>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
    }

    private void InitializeDatabase()
    {
        _context = new DatabaseContextTest(_options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        // Add your test data here
        var company = new Company { CompanyName = "TestCompany", CompanyAddress = "123 Test St", CompanyEmail = "testcompany@gmail.com", CompanyPhone = "123456789", CompanyKRS = "12345678901234" };
        company.CompanyId = 1;
        _context.Companies.Add(company);

        var person = new Person { PersonId = 1, PersonFirstName = "Jan", PersonLastName = "Kowalski", PersonAddress = "Test address", PersonEmail = "jankowalski@gmail.com", PersonPhone = "098765432", PersonPesel = "12345678901", PersonSoftDelete = false };
        _context.Persons.Add(person);

        var customer = new Customer { CustomerId = 1, PersonId = 1, CompanyId = null };
        _context.Customers.Add(customer);

        var software1 = new Software { SoftwareId = 1, SoftwareName = "Test", SoftwareDescription = "This is a test software.", SoftwareCurrentVersion = "1.2", SoftwareCategory = "Finance" };
        _context.Software.Add(software1);
        
        var software2 = new Software { SoftwareId = 2, SoftwareName = "Test", SoftwareDescription = "This is the test software.", SoftwareCurrentVersion = "1.0", SoftwareCategory = "Bookkeaping" };
        _context.Software.Add(software2);

        var contract = new Contract { IdContract = 1, IdCustomer = 1, IdSoftware = 1, Signed = true, TotalPrice = 1000 };
        _context.Contracts.Add(contract);

        _context.SaveChanges();

        _service = new ContractService(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task PostContract_WithValidData_ReturnsContractId()
    {
        InitializeDatabase();
        var requestModel = new PostContractRequestModel
        {
            IdCustomer = 1,
            SoftwareId = 2,
            SoftwareVersion = "1.0",
            UpdateInformation = "Test",
            ContractDateFrom = DateTime.Now.AddDays(1),
            ContractDateTo = DateTime.Now.AddDays(10),
            Price = 1000,
            AdditionalSupportInYears = 1
        };

        var result = await _service.PostContract(requestModel);

        Assert.Equal(2, result);
    }

    [Fact]
    public async Task PostContract_WithInvalidCustomer_ThrowsNotFoundException()
    {
        InitializeDatabase();
        var requestModel = new PostContractRequestModel
        {
            IdCustomer = 3,
            SoftwareId = 1,
            SoftwareVersion = "1.0",
            UpdateInformation = "Test",
            ContractDateFrom = new DateTime(2025, 5, 1),
            ContractDateTo = new DateTime(2025, 5, 20),
            Price = 1000,
            AdditionalSupportInYears = 1
        };

        await Assert.ThrowsAsync<NotFoundException>(() => _service.PostContract(requestModel));
    }

    [Fact]
    public async Task PostContract_WithInvalidSoftware_ThrowsNotFoundException()
    {
        InitializeDatabase();
        var requestModel = new PostContractRequestModel
        {
            IdCustomer = 1,
            SoftwareId = 5,
            SoftwareVersion = "1.0",
            UpdateInformation = "Test",
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
        InitializeDatabase();
        var requestModel = new PostContractRequestModel
        {
            IdCustomer = 1,
            SoftwareId = 1,
            SoftwareVersion = "1.0",
            UpdateInformation = "Test",
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
        InitializeDatabase();
        var requestModel = new PostContractRequestModel
        {
            IdCustomer = 1,
            SoftwareId = 1,
            SoftwareVersion = "1.0",
            UpdateInformation = "Test",
            ContractDateFrom = new DateTime(2023, 5, 1),
            ContractDateTo = new DateTime(2023, 5, 20),
            Price = 1000,
            AdditionalSupportInYears = 1
        };

        await Assert.ThrowsAsync<AlreadyExistsException>(() => _service.PostContract(requestModel));
    }

    // [Fact]
    // public async Task PostContract_WithActiveSubscription_ThrowsAlreadyExistsException()
    // {
    //     InitializeDatabase();
    //     var requestModel = new PostContractRequestModel
    //     {
    //         IdCustomer = 1,
    //         SoftwareId = 1,
    //         ContractDateFrom = DateTime.Now.AddDays(1),
    //         ContractDateTo = DateTime.Now.AddDays(10),
    //         Price = 1000,
    //         AdditionalSupportInYears = 1
    //     };
    //
    //     await Assert.ThrowsAsync<AlreadyExistsException>(() => _service.PostContract(requestModel));
    // }

    [Fact]
    public async Task PostContract_WithInvalidAdditionalSupport_ThrowsArgumentException()
    {
        InitializeDatabase();
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
}