using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProjektAPBD.Contexts;
using ProjektAPBD.Exceptions;
using ProjektAPBD.Models;
using ProjektAPBD.RequestModels.PersonRequestModels;

namespace ProjektAPBD.Services;

public interface IPersonService
{
    Task<int> PostPerson(PostPersonRequestModel company);
    Task<int> UpdatePerson(UpdatePersonRequestModel company);
    Task<List<Person>> GetAllPersons();
    Task<int> DeletePerson(int id);
}

public class PersonService : IPersonService
{
    private readonly DatabaseContext _context;
    public PersonService(DatabaseContext context)
    {
        _context = context;
    }
    public async Task<int> PostPerson(PostPersonRequestModel person)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var validationContext = new ValidationContext(person);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(person, validationContext, validationResults, true))
            {
                throw new ArgumentException("Invalid data");
            }

            var personExists = await _context.Persons.AnyAsync(c => c.PersonPesel == person.PersonPesel);
            if (personExists)
            {
                throw new ArgumentException("Person with that PESEL already exists");
            }

            var newPerson = new Person
            {
                PersonFirstName = person.PersonFirstName,
                PersonLastName = person.PersonLastName,
                PersonAddress = person.PersonAddress,
                PersonEmail = person.PersonEmail,
                PersonPhone = person.PersonPhone,
                PersonPesel = person.PersonPesel,
                PersonSoftDelete = false
            };
            
            var maxIdPerson = await _context.Persons.MaxAsync(p => p.PersonId);
            newPerson.PersonId = maxIdPerson + 1;
            
            var Customer = new Customer
            {
                PersonId = newPerson.PersonId,
                CompanyId = null
            };

            await _context.Persons.AddAsync(newPerson);
            await _context.Customers.AddAsync(Customer);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return newPerson.PersonId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<int> UpdatePerson(UpdatePersonRequestModel person)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            if(person.PersonId < 1)
            {
                throw new NotFoundException("Invalid person ID");
            }

            var validationContext = new ValidationContext(person);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(person, validationContext, validationResults, true))
            {
                throw new ArgumentException("Invalid data");
            }

            var personToUpdate = await _context.Persons.FirstOrDefaultAsync(p => p.PersonId == person.PersonId);
            if (personToUpdate == null)
            {
                throw new NotFoundException("Person not found");
            }

            personToUpdate.PersonFirstName = person.PersonFirstName;
            personToUpdate.PersonLastName = person.PersonLastName;
            personToUpdate.PersonAddress = person.PersonAddress;
            personToUpdate.PersonEmail = person.PersonEmail;
            personToUpdate.PersonPhone = person.PersonPhone;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return personToUpdate.PersonId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<List<Person>> GetAllPersons()
    {
        return await _context.Persons.ToListAsync();
    }
    
    public async Task<int> DeletePerson(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            if(id < 1)
            {
                throw new ArgumentException("Invalid person ID");
            }

            var personToDelete = await _context.Persons.FirstOrDefaultAsync(p => p.PersonId == id);
            if (personToDelete == null)
            {
                throw new NotFoundException("Person not found");
            }

            personToDelete.PersonSoftDelete = true;
            personToDelete.PersonFirstName = "Deleted";
            personToDelete.PersonLastName = "Deleted";
            personToDelete.PersonAddress = "Deleted";
            personToDelete.PersonEmail = "Deleted";
            personToDelete.PersonPhone = "Deleted";
            personToDelete.PersonPesel = "Deleted";

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return personToDelete.PersonId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}