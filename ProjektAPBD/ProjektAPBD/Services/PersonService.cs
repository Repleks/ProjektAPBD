using System.ComponentModel.DataAnnotations;
using Kolokwium2.Exceptions;
using Microsoft.EntityFrameworkCore;
using ProjektAPBD.Contexts;
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

public class PersonService(DatabaseContext context) : IPersonService
{
    public async Task<int> PostPerson(PostPersonRequestModel person)
    {
        var validationContext = new ValidationContext(person);
        var validationResults = new List<ValidationResult>();
        if (!Validator.TryValidateObject(person, validationContext, validationResults, true))
        {
            throw new ArgumentException("Invalid data");
        }
        
        using var transaction = await context.Database.BeginTransactionAsync();
        var personExists = await context.People.AnyAsync(c => c.PersonPesel == person.PersonPesel);
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
            
        await context.People.AddAsync(newPerson);
        await context.SaveChangesAsync();
        await transaction.CommitAsync();
        return newPerson.PersonId;
    }
    
    public async Task<int> UpdatePerson(UpdatePersonRequestModel person)
    {
        if(person.PersonId < 1)
        {
            throw new ArgumentException("Invalid person ID");
        }
        
        var validationContext = new ValidationContext(person);
        var validationResults = new List<ValidationResult>();
        if (!Validator.TryValidateObject(person, validationContext, validationResults, true))
        {
            throw new ArgumentException("Invalid data");
        }
        
        using var transaction = await context.Database.BeginTransactionAsync();
        var personToUpdate = await context.People.FirstOrDefaultAsync(p => p.PersonId == person.PersonId);
        if (personToUpdate == null)
        {
            throw new ArgumentException("Person not found");
        }
        
        personToUpdate.PersonFirstName = person.PersonFirstName;
        personToUpdate.PersonLastName = person.PersonLastName;
        personToUpdate.PersonAddress = person.PersonAddress;
        personToUpdate.PersonEmail = person.PersonEmail;
        personToUpdate.PersonPhone = person.PersonPhone;
        
        await context.SaveChangesAsync();
        await transaction.CommitAsync();
        return personToUpdate.PersonId;
    }
    
    public async Task<List<Person>> GetAllPersons()
    {
        return await context.People.ToListAsync();
    }
    
    public async Task<int> DeletePerson(int id)
    {
        if(id < 1)
        {
            throw new ArgumentException("Invalid person ID");
        }
        
        using var transaction = await context.Database.BeginTransactionAsync();
        var personToDelete = await context.People.FirstOrDefaultAsync(p => p.PersonId == id);
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
        
        await context.SaveChangesAsync();
        await transaction.CommitAsync();
        return personToDelete.PersonId;
    }
}