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
}
public class ContractService(DatabaseContext context) : IContractService
{
    public async Task<int> PostContract(PostContractRequestModel contract)
    {
        using var transaction = await context.Database.BeginTransactionAsync();
    
        try
        {
            var validationContext = new ValidationContext(contract);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(contract, validationContext, validationResults, true))
            {
                throw new ArgumentException("Invalid data");
            }
    
            var customerExists = await context.Customers.AnyAsync(c => c.CustomerId == contract.IdCustomer);
            if (!customerExists)
            {
                throw new NotFoundException("Customer does not exist");
            }
    
            var softwareExists = await context.Software.AnyAsync(s => s.SoftwareId == contract.SoftwareId);
            if (!softwareExists)
            {
                throw new NotFoundException("Software does not exist");
            }

            var softwareVersionExists = await context.SoftwareVersions.AnyAsync(sv => sv.IdSoftware == contract.SoftwareId && sv.Version == contract.SoftwareVersion);
            if (!softwareVersionExists)
            {
                throw new NotFoundException("Software version does not exist");
            }
            
            var contractDuration = (contract.ContractDateTo - contract.ContractDateFrom).TotalDays;
            if (contractDuration < 3 || contractDuration > 30)
            {
                throw new ArgumentException("Contract duration should be between 3 and 30 days");
            }
        
            var activeContractExists = await context.Contracts.AnyAsync(c => c.IdCustomer == contract.IdCustomer && c.IdSoftware == contract.SoftwareId && c.Signed);
            if (activeContractExists)
            {
                throw new AlreadyExistsException("Customer already has an active contract for this software");
            }
        
            var activeSubscriptionExists = await context.Subscriptions.AnyAsync(s => s.CustomerId == contract.IdCustomer && s.SoftwareId == contract.SoftwareId && s.ActiveStatus);
            if (activeSubscriptionExists)
            {
                throw new AlreadyExistsException("Customer already has an active subscription for this software");
            }
            
            if (contract.AdditionalSupportInYears < 0 || contract.AdditionalSupportInYears > 3)
            {
                throw new ArgumentException("Additional support should be between 1 and 3 years, or should be 0");
            }
        
            var previousCustomer = await context.Contracts.AnyAsync(c => c.IdCustomer == contract.IdCustomer) || await context.Subscriptions.AnyAsync(s => s.CustomerId == contract.IdCustomer);
            var previousCustomerDiscount = previousCustomer ? 0.05 : 0.00;
            
            var discounts = await context.Discounts.Where(d => d.DiscountDateStart <= DateTime.Now && d.DiscountDateEnd >= DateTime.Now).ToListAsync();
            
            var maxDiscount = discounts.Any() ? discounts.Max(d => d.DiscountValue) / 100.0 : 0.00;
        
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

            var contractDiscounts = new List<ContractDiscount>();
            foreach (var dizcount in discounts)
            {
                contractDiscounts.Add(new ContractDiscount
                {
                    IdContract = newContract.IdContract,
                    IdDiscount = dizcount.DiscountId
                });
            }
            newContract.ContractDiscounts = contractDiscounts;
            
        
            await context.Contracts.AddAsync(newContract);
            await context.SaveChangesAsync();
    
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
        return await context.Contracts.ToListAsync();
    }
}