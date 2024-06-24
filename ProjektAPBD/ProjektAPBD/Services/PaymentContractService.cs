﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProjektAPBD.Contexts;
using ProjektAPBD.Exceptions;
using ProjektAPBD.Models;
using ProjektAPBD.RequestModels.PaymentContractRequestModels;

namespace ProjektAPBD.Services;

public interface IPaymentContractService
{
    Task<int> PostPaymentContract(PostPaymentContractRequestModel paymentContract);   
    Task<List<PaymentContract>> GetAllPaymentsContracts();
}
public class PaymentContractService(DatabaseContext context) : IPaymentContractService
{
    public async Task<int> PostPaymentContract(PostPaymentContractRequestModel paymentContract)
    {
        using var transaction = await context.Database.BeginTransactionAsync();
    
        try
        {
            var validationContext = new ValidationContext(paymentContract);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(paymentContract, validationContext, validationResults, true))
            {
                throw new ArgumentException("Invalid data");
            }
    
            var contractExists = await context.Contracts.AnyAsync(c => c.IdContract == paymentContract.ContractId);
            if (!contractExists)
            {
                throw new NotFoundException("Contract does not exist");
            }
            
            var contract = await context.Contracts.FirstOrDefaultAsync(c => c.IdContract == paymentContract.ContractId);
            if (contract.TotalPrice < paymentContract.Amount)
            {
                throw new ArgumentException("Payment exceeds the total contract amount");
            }
    
            if (contract.DateTo < DateTime.Now)
            {
                var payments = await context.PaymentsContracts.Where(pc => pc.ContractId == paymentContract.ContractId).ToListAsync();
                foreach (var payment in payments)
                {
                    payment.Amount = 0;
                    payment.PaymentInformation = "Payment after due date";
                }
                await context.SaveChangesAsync();
                throw new TooLateException("Cannot accept payment after the due date");
            }
    
            var newPaymentContract = new PaymentContract
            {
                PaymentId = paymentContract.PaymentId,
                ContractId = paymentContract.ContractId,
                PaymentDate = paymentContract.PaymentDate,
                Amount = paymentContract.Amount,
                PaymentInformation = paymentContract.PaymentInformation
            };
    
            await context.PaymentsContracts.AddAsync(newPaymentContract);
            await context.SaveChangesAsync();
    
            var totalPaid = await context.PaymentsContracts.Where(pc => pc.ContractId == paymentContract.ContractId).SumAsync(pc => pc.Amount);
            if (totalPaid >= contract.TotalPrice)
            {
                contract.Signed = true;
                await context.SaveChangesAsync();
            }
    
            await transaction.CommitAsync();
    
            return newPaymentContract.PaymentId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task<List<PaymentContract>> GetAllPaymentsContracts()
    {
        return await context.PaymentsContracts.ToListAsync();
    }
}