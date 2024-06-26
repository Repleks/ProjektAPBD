using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProjektAPBD.Contexts;
using ProjektAPBD.Exceptions;
using ProjektAPBD.Models;
using ProjektAPBD.RequestModels.ContractRequestModels;

namespace ProjektAPBD.Services;

public interface IContractService
{
    Task<int> PostContract(PostContractRequestModel contract);
    Task<List<Contract>> GetAllContracts();
    Task<int> DeleteContract(int id);
}
public class ContractService : IContractService
{
    private readonly DatabaseContext _context;
    public ContractService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<int> PostContract(PostContractRequestModel contract)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
    
        try
        {
            var validationContext = new ValidationContext(contract);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(contract, validationContext, validationResults, true))
            {
                throw new ArgumentException("Invalid data");
            }
    
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == contract.IdCustomer);
            if (!customerExists)
            {
                throw new NotFoundException("Customer does not exist");
            }
            
            var customer = await _context.Customers.FindAsync(contract.IdCustomer);
            if (customer.PersonId != null)
            {
                var person = await _context.Persons.FindAsync(customer.PersonId);
                if (person.PersonSoftDelete)
                {
                    throw new NotFoundException("Person is soft deleted");
                }
            }
            
            var softwareExists = await _context.Software.AnyAsync(s => s.SoftwareId == contract.SoftwareId);
            if (!softwareExists)
            {
                throw new NotFoundException("Software does not exist");
            }

            var softwareVersionExists = await _context.SoftwareVersions.AnyAsync(sv => sv.IdSoftware == contract.SoftwareId && sv.Version == contract.SoftwareVersion);
            if (!softwareVersionExists)
            {
                throw new NotFoundException("Software version does not exist");
            }
            
            var contractDuration = (contract.ContractDateTo - contract.ContractDateFrom).TotalDays;
            if (contractDuration < 3 || contractDuration > 30)
            {
                throw new ArgumentException("Contract duration should be between 3 and 30 days");
            }
        
            var activeContractExists = await _context.Contracts.AnyAsync(c => c.IdCustomer == contract.IdCustomer && c.IdSoftware == contract.SoftwareId && c.Signed);
            if (activeContractExists)
            {
                throw new AlreadyExistsException("Customer already has an active contract for this software");
            }
        
            var activeSubscriptionExists = await _context.Subscriptions.AnyAsync(s => s.CustomerId == contract.IdCustomer && s.SoftwareId == contract.SoftwareId && s.ActiveStatus);
            if (activeSubscriptionExists)
            {
                throw new AlreadyExistsException("Customer already has an active subscription for this software");
            }
            
            if (contract.AdditionalSupportInYears < 0 || contract.AdditionalSupportInYears > 3)
            {
                throw new ArgumentException("Additional support should be between 1 and 3 years, or should be 0");
            }
        
            var previousCustomer = await _context.Contracts.AnyAsync(c => c.IdCustomer == contract.IdCustomer) || await _context.Subscriptions.AnyAsync(s => s.CustomerId == contract.IdCustomer);
            var previousCustomerDiscount = previousCustomer ? 0.05 : 0.00;
            
            var softwareDiscounts = await _context.SoftwareDiscounts
                .Include(sd => sd.Discount)
                .Where(sd => sd.SoftwareId == contract.SoftwareId)
                .ToListAsync();
        
            double maxDiscount = 0;
            
            foreach (var softwareDiscount in softwareDiscounts)
            {
                if (softwareDiscount.Discount.DiscountDateStart <= contract.ContractDateFrom &&
                    softwareDiscount.Discount.DiscountDateEnd >= contract.ContractDateTo)
                {
                    if (softwareDiscount.Discount.DiscountValue > maxDiscount)
                    {
                        maxDiscount = softwareDiscount.Discount.DiscountValue;
                    }
                }
            }
            
            maxDiscount /= 100;
            
            var discount = previousCustomerDiscount + maxDiscount;
        
            var price = contract.Price * (1 - discount) + contract.AdditionalSupportInYears * 1000;
        
            var newContract = new Contract
            {
                IdCustomer = contract.IdCustomer,
                IdSoftware = contract.SoftwareId,
                DateFrom = contract.ContractDateFrom,
                DateTo = contract.ContractDateTo,
                PricePerMonth = price / contractDuration,
                AdditionalSupport = contract.AdditionalSupportInYears + 1,
                TotalPrice = price,
                Signed = false
            };
            
            await _context.Contracts.AddAsync(newContract);
            await _context.SaveChangesAsync();
    
            await transaction.CommitAsync();
    
            return newContract.IdContract;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<List<Contract>> GetAllContracts()
    {
        return await _context.Contracts.ToListAsync();
    }
    
    public async Task<int> DeleteContract(int id)
    {
        if( id < 1)
        {
            throw new ArgumentException("Invalid contract ID");
        }
        
        var contract = await _context.Contracts.FirstOrDefaultAsync(c => c.IdContract == id);
        if (contract == null)
        {
            throw new NotFoundException("Contract does not exist");
        }
        
        _context.Contracts.Remove(contract);
        await _context.SaveChangesAsync();
        
        return contract.IdContract;
    }
}