using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProjektAPBD.Contexts;
using ProjektAPBD.Exceptions;
using ProjektAPBD.Models;
using ProjektAPBD.RequestModels.PaymentSubcriptionRequestModels;

namespace ProjektAPBD.Services;

public interface IPaymentSubscriptionService
{
    Task<int> PostPaymentSubscription(PostPaymentSubscriptionRequestModel model);
    Task<List<PaymentSubscription>> GetAllPaymentSubscriptions();
}
public class PaymentSubscriptionService : IPaymentSubscriptionService
{
    private readonly DatabaseContext _context;
    
    public PaymentSubscriptionService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<int> PostPaymentSubscription(PostPaymentSubscriptionRequestModel model)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
            {
                throw new ArgumentException("Invalid data");
            }
            
            var subscriptionExists = await _context.Subscriptions.AnyAsync(c => c.SubscriptionId == model.SubscriptionId);
            if (!subscriptionExists)
            {
                throw new NotFoundException("Subscription does not exist");
            }
            
            var subscription = await _context.Subscriptions.FindAsync(model.SubscriptionId);
            if (model.Amount != subscription.PricePerMonth)
            {
                throw new ArgumentException("Incorrect amount");
            }
            
            if(subscription.ActiveStatus == false)
            {
                throw new ArgumentException("Subscription is not active");
            }
            
            if(subscription.RenewalDate.Day < model.PaymentDate.Day || subscription.RenewalDate.Month < model.PaymentDate.Month || subscription.RenewalDate.Year < model.PaymentDate.Year)
            {
                subscription.ActiveStatus = false;
                await _context.SaveChangesAsync();
                throw new ArgumentException("Payment date is after renewal date");
            }
            
            if(subscription.RenewalDate.Day > model.PaymentDate.Day)
            {
                throw new ArgumentException("The subscription has already been paid for");
            }
            
            var newPaymentSubscription = new PaymentSubscription
            {
                SubscriptionId = model.SubscriptionId,
                PaymentDate = model.PaymentDate,
                Amount = model.Amount
            };

            await _context.PaymentsSubscriptions.AddAsync(newPaymentSubscription);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return newPaymentSubscription.PaymentId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<List<PaymentSubscription>> GetAllPaymentSubscriptions()
    {
        return await _context.PaymentsSubscriptions.ToListAsync();
    }
}