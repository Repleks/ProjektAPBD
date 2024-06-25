using Xunit;
using ProjektAPBD.Services;
using ProjektAPBD.Contexts;
using ProjektAPBD.Models;
using Microsoft.EntityFrameworkCore;
using ProjektAPBD.Exceptions;
using ProjektAPBD.RequestModels.PersonRequestModels;
using System.Linq;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjektAPBDTest;

public class PersonServiceTests : IDisposable
{
    private readonly DbContextOptions<DatabaseContextTest> _options;
    private DatabaseContextTest _context;
    private PersonService _service;

    public PersonServiceTests()
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
        
        _context.Persons.Add(new Person { PersonId = 1, PersonFirstName = "Jan", PersonLastName = "Kowalski", PersonAddress = "Test address", PersonEmail = "jankowalski@gmail.com", PersonPhone = "098765432", PersonPesel = "12345678901", PersonSoftDelete = false });

        _context.SaveChanges();

        _service = new PersonService(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task PostPerson_WithValidData_ReturnsPersonId()
    {
        // Arrange
        InitializeDatabase();
        var requestModel = new PostPersonRequestModel
        {
            PersonFirstName = "NewPerson",
            PersonLastName = "Test",
            PersonAddress = "789 Test St",
            PersonEmail = "newperson@gmail.com",
            PersonPhone = "456123789",
            PersonPesel = "45612378901"
        };

        // Act
        var result = await _service.PostPerson(requestModel);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task PostPerson_WithExistingPesel_ThrowsArgumentException()
    {
        // Arrange
        InitializeDatabase();
        var requestModel = new PostPersonRequestModel
        {
            PersonFirstName = "NewPerson",
            PersonLastName = "Test",
            PersonAddress = "789 Test St",
            PersonEmail = "newperson@gmail.com",
            PersonPhone = "456123789",
            PersonPesel = "12345678901"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.PostPerson(requestModel));
    }

    [Fact]
    public async Task UpdatePerson_WithValidData_ReturnsPersonId()
    {
        // Arrange
        InitializeDatabase();
        var requestModel = new UpdatePersonRequestModel
        {
            PersonId = 1,
            PersonFirstName = "UpdatedPerson",
            PersonLastName = "Test",
            PersonAddress = "789 Test St",
            PersonEmail = "updatedperson@gmail.com",
            PersonPhone = "456123789",
        };

        // Act
        var result = await _service.UpdatePerson(requestModel);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task UpdatePerson_WithNonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        InitializeDatabase();
        var requestModel = new UpdatePersonRequestModel
        {
            PersonId = 3,
            PersonFirstName = "UpdatedPerson",
            PersonLastName = "Test",
            PersonAddress = "789 Test St",
            PersonEmail = "updatedperson@gmail.com",
            PersonPhone = "456123789",
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdatePerson(requestModel));
    }
}