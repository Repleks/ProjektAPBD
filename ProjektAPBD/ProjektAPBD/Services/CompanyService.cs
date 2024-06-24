using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProjektAPBD.Contexts;
using ProjektAPBD.Exceptions;
using ProjektAPBD.Models;
using ProjektAPBD.RequestModels;

namespace ProjektAPBD.Services;

public interface ICompanyService
{
    Task<int> PostCompany(PostCompanyRequestModel company);
    Task<int> UpdateCompany(UpdateCompanyRequestModel company);
    Task<List<Company>> GetAllCompanies();
}

public class CompanyService(DatabaseContext context) : ICompanyService
{
    public async Task<int> PostCompany(PostCompanyRequestModel company)
    {
        using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var validationContext = new ValidationContext(company);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(company, validationContext, validationResults, true))
            {
                throw new ArgumentException("Invalid data");
            }

            var companyExists = await context.Companies.AnyAsync(c => c.CompanyKRS == company.CompanyKRS);
            if (companyExists)
            {
                throw new ArgumentException("Company with that KRS already exists");
            }

            var newCompany = new Company
            {
                CompanyName = company.CompanyName,
                CompanyAddress = company.CompanyAddress,
                CompanyEmail = company.CompanyEmail,
                CompanyPhone = company.CompanyPhone,
                CompanyKRS = company.CompanyKRS
            };

            await context.Companies.AddAsync(newCompany);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            return newCompany.CompanyId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<int> UpdateCompany(UpdateCompanyRequestModel company)
    {
        using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            if(company.IdCompany < 1)
            {
                throw new ArgumentException("Invalid company ID");
            }

            var validationContext = new ValidationContext(company);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(company, validationContext, validationResults, true))
            {
                throw new ArgumentException("Invalid data");
            }

            var companyToUpdate = await context.Companies.FirstOrDefaultAsync(c => c.CompanyId == company.IdCompany);
            if (companyToUpdate == null)
            {
                throw new NotFoundException("Company not found");
            }

            companyToUpdate.CompanyName = company.CompanyName;
            companyToUpdate.CompanyAddress = company.CompanyAddress;
            companyToUpdate.CompanyEmail = company.CompanyEmail;
            companyToUpdate.CompanyPhone = company.CompanyPhone;

            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            return companyToUpdate.CompanyId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<List<Company>> GetAllCompanies()
    {
        return await context.Companies.ToListAsync();
    }
    
}
