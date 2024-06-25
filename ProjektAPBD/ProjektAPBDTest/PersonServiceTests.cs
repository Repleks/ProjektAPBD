using Xunit;
using Moq;
using System.Threading.Tasks;
using ProjektAPBD.Services;
using ProjektAPBD.Contexts;
using ProjektAPBD.Models;
using ProjektAPBD.RequestModels.PersonRequestModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjektAPBD.Exceptions;

public class PersonServiceTests
{
    private readonly Mock<DatabaseContext> _mockContext;
    private readonly PersonService _service;

    public PersonServiceTests()
    {
        _mockContext = new Mock<DatabaseContext>();
        _service = new PersonService(_mockContext.Object);
    }

    [Fact]
    public async Task PostPerson_WithValidData_ReturnsPersonId()
    {
        var requestModel = new PostPersonRequestModel
        {
            PersonFirstName = "Test",
            PersonLastName = "Person",
            PersonAddress = "Test Address",
            PersonEmail = "test@test.com",
            PersonPhone = "123456789",
            PersonPesel = "12345678901"
        };

        var result = await _service.PostPerson(requestModel);

        Assert.Equal(1, result);
    }

    [Fact]
    public async Task PostPerson_WithExistingPesel_ThrowsArgumentException()
    {
        var requestModel = new PostPersonRequestModel
        {
            PersonFirstName = "Test",
            PersonLastName = "Person",
            PersonAddress = "Test Address",
            PersonEmail = "test@test.com",
            PersonPhone = "123456789",
            PersonPesel = "12345678901"
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.PostPerson(requestModel));
    }

    [Fact]
    public async Task UpdatePerson_WithValidData_ReturnsPersonId()
    {
        var requestModel = new UpdatePersonRequestModel
        {
            PersonId = 1,
            PersonFirstName = "Updated",
            PersonLastName = "Person",
            PersonAddress = "Updated Address",
            PersonEmail = "updated@test.com",
            PersonPhone = "987654321"
        };

        var result = await _service.UpdatePerson(requestModel);

        Assert.Equal(1, result);
    }

    [Fact]
    public async Task UpdatePerson_WithNonExistingId_ThrowsArgumentException()
    {
        var requestModel = new UpdatePersonRequestModel
        {
            PersonId = 2,
            PersonFirstName = "Updated",
            PersonLastName = "Person",
            PersonAddress = "Updated Address",
            PersonEmail = "updated@test.com",
            PersonPhone = "987654321"
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdatePerson(requestModel));
    }

    [Fact]
    public async Task DeletePerson_WithValidId_ReturnsPersonId()
    {
        var result = await _service.DeletePerson(1);

        Assert.Equal(1, result);
    }

    [Fact]
    public async Task DeletePerson_WithNonExistingId_ThrowsNotFoundException()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeletePerson(2));
    }

    [Fact]
    public async Task GetAllPersons_ReturnsAllPersons()
    {
        var result = await _service.GetAllPersons();

        Assert.Equal(3, result.Count);
    }
}