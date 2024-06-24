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
}
public class ContractService(DatabaseContext context) : IContractService
{
    public async Task<int> PostContract(PostContractRequestModel contract)
    {
        var validationContext = new ValidationContext(contract);
        var validationResults = new List<ValidationResult>();
        if (!Validator.TryValidateObject(contract, validationContext, validationResults, true))
        {
            throw new ArgumentException("Invalid data");
        }
        
        
    
        var contractDuration = (contract.ContractDateTo - contract.ContractDateFrom).TotalDays;
        if (contractDuration < 3 || contractDuration > 30)
        {
            throw new ArgumentException("Contract duration should be between 3 and 30 days");
        }
    
        var activeContractExists = await context.Contracts.AnyAsync(c => c.IdCustomer == contract.IdCustomer && c.IdSoftware == contract.Software && c.Signed && c.DateTo > DateTime.Now);
        if (activeContractExists)
        {
            throw new AlreadyExistsException("Customer already has an active contract for this software");
        }
    
        var activeSubscriptionExists = await context.Subscriptions.AnyAsync(s => s.CustomerId == contract.IdCustomer && s.SoftwareId == contract.Software && s.ActiveStatus && s.RenewalDate > DateTime.Now);
        if (activeSubscriptionExists)
        {
            throw new AlreadyExistsException("Customer already has an active subscription for this software");
        }
    
        if (contract.AdditionalSupportInYears != 1 || contract.AdditionalSupportInYears != 2 ||
            contract.AdditionalSupportInYears != 3)
        {
            throw new ArgumentException("Additional Support In Years has to be 1, 2 or 3");
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
            IdSoftware = contract.Software,
            DateFrom = contract.ContractDateFrom,
            DateTo = contract.ContractDateTo,
            PricePerMonth = price / contractDuration,
            AdditionalSupport = contract.AdditionalSupportInYears,
            TotalPrice = price,
            Signed = false
        };
    
        await context.Contracts.AddAsync(newContract);
        await context.SaveChangesAsync();
    
        return newContract.IdContract;
    }
}