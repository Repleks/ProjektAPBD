using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProjektAPBD.Contexts;
using ProjektAPBD.Exceptions;
using ProjektAPBD.Models;
using ProjektAPBD.RequestModels.SubscriptionRequestModels;

namespace ProjektAPBD.Services;

public interface ISubcriptionService
{
    Task<int> PostSubcription(PostSubscriptionRequestModel company);   
    Task<List<Subscription>> GetAllSubcription();
}
public class SubcriptionService : ISubcriptionService
{
    private readonly DatabaseContext _context;
    
    public SubcriptionService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<int> PostSubcription(PostSubscriptionRequestModel subcription)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            var validationContext = new ValidationContext(subcription);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(subcription, validationContext, validationResults, true))
            {
                throw new ArgumentException("Invalid data");
            }
            
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == subcription.CustomerId);
            if (!customerExists)
            {
                throw new NotFoundException("Customer does not exist");
            }
            var customer = await _context.Customers.FindAsync(subcription.CustomerId);
            if (customer.PersonId != null)
            {
                var person = await _context.Persons.FindAsync(customer.PersonId);
                if (person.PersonSoftDelete)
                {
                    throw new NotFoundException("Person is soft deleted");
                }
            }
            
            var softwareExists = await _context.Software.AnyAsync(s => s.SoftwareId == subcription.SoftwareId);
            if (!softwareExists)
            {
                throw new NotFoundException("Software does not exist");
            }
            
            if (subcription.RenewalDate < DateTime.Now.AddMonths(1) || subcription.RenewalDate > DateTime.Now.AddMonths(24))
            {
                throw new ArgumentException("Renewal date must be between 1 and 24 months");
            }
            
            var previousCustomer = await _context.Contracts.AnyAsync(c => c.IdCustomer == subcription.CustomerId) || await _context.Subscriptions.AnyAsync(s => s.CustomerId == subcription.CustomerId);
            var previousCustomerDiscount = previousCustomer ? 0.05 : 0.00;
            
            var softwareDiscounts = await _context.SoftwareDiscounts
                .Include(sd => sd.Discount)
                .Where(sd => sd.SoftwareId == subcription.SoftwareId)
                .ToListAsync();
        
            double maxDiscount = 0;
            
            foreach (var softwareDiscount in softwareDiscounts)
            {
                if (softwareDiscount.Discount.DiscountDateStart <= DateTime.Now &&
                    softwareDiscount.Discount.DiscountDateEnd >= DateTime.Now)
                {
                    if (softwareDiscount.Discount.DiscountValue > maxDiscount)
                    {
                        maxDiscount = softwareDiscount.Discount.DiscountValue;
                    }
                }
            }
  
            maxDiscount /= 100;
            
            var discount = previousCustomerDiscount + maxDiscount;
        
            var firstPayment = subcription.PricePerMonth - (subcription.PricePerMonth * discount);
            
            var newSubcription = new Subscription
            {
                CustomerId = subcription.CustomerId,
                SoftwareId = subcription.SoftwareId,
                SubscriptionName = subcription.SubscriptionName,
                RenewalDate = subcription.RenewalDate,
                PricePerMonth = subcription.PricePerMonth*(1-previousCustomerDiscount),
                FirstPaymentPrice = firstPayment,
                ActiveStatus = true,
                ActivationDate = DateTime.Now
            };
            
            var newPayment = new PaymentSubscription
            {
                Subscription = newSubcription,
                PaymentDate = DateTime.Now,
                Amount = firstPayment
            };
            
            await _context.PaymentsSubscriptions.AddAsync(newPayment);
            await _context.Subscriptions.AddAsync(newSubcription);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            return newSubcription.SubscriptionId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<List<Subscription>> GetAllSubcription()
    {
        return await _context.Subscriptions.ToListAsync();
    }
}