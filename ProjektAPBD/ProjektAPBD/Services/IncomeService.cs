using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ProjektAPBD.Contexts;
using ProjektAPBD.Exceptions;

namespace ProjektAPBD.Services;

public interface IIncomeService
{
    Task<double> GetIncomeForWholeCompanyCurrent(string currCode);
    Task<double> GetIncomeForSoftwareCurrentCurrent(int softwareId, string currCode);
    Task<double> GetIncomeForWholeCompanyExcepted(string currCode);
    Task<double> GetIncomeForSoftwareCurrentExcepted(int softwareId, string currCode);
}
public class IncomeService : IIncomeService
{
    private readonly DatabaseContext _context;
    public IncomeService(DatabaseContext context)
    {
        _context = context;
    }
    public async Task<double> GetIncomeForWholeCompanyCurrent(string? currCode)
    {
        var totalIncomeFromContracts = await _context.Contracts
            .Where(c => c.Signed == true)
            .SumAsync(c => c.TotalPrice);

        var totalIncomeFromSubscriptions = await _context.PaymentsSubscriptions
            .Where(ps => ps.Subscription.ActiveStatus == true)
            .SumAsync(ps => ps.Amount);

        var totalIncome = totalIncomeFromContracts + totalIncomeFromSubscriptions;
        if(currCode != null)
        {
            using var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync("https://api.nbp.pl/api/exchangerates/rates/a/"+currCode+"/?format=json");

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(responseContent);
                var exchangeRate = jsonDocument.RootElement.GetProperty("rates")[0].GetProperty("mid").GetDouble();

                totalIncome /= exchangeRate;
            }
            else
            {
                throw new ArgumentException("Invalid currency code");
            }
        }
        return totalIncome;
    }

    public async Task<double> GetIncomeForSoftwareCurrentCurrent(int softwareId, string? currCode)
    {
        if (softwareId < 1)
        {
            throw new ArgumentException("Invalid software ID");
        }
        
        var softwareExists = await _context.Software.AnyAsync(s => s.SoftwareId == softwareId);
        if (!softwareExists)
        {
            throw new NotFoundException("Software does not exist");
        }
        
        var softwareIncomeFromContracts = await _context.Contracts
            .Where(c => c.Signed == true && c.IdSoftware == softwareId)
            .SumAsync(c => c.TotalPrice);

        var softwareIncomeFromSubscriptions = await _context.PaymentsSubscriptions
            .Where(ps => ps.Subscription.SoftwareId == softwareId && ps.Subscription.ActiveStatus == true)
            .SumAsync(ps => ps.Amount);

        var totalIncome = softwareIncomeFromContracts + softwareIncomeFromSubscriptions;
        
        if(currCode != null)
        {
            using var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync("https://api.nbp.pl/api/exchangerates/rates/a/"+currCode+"/?format=json");

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(responseContent);
                var exchangeRate = jsonDocument.RootElement.GetProperty("rates")[0].GetProperty("mid").GetDouble();

                totalIncome /= exchangeRate;
            }
            else
            {
                throw new ArgumentException("Invalid currency code");
            }
        }
        return totalIncome;
    }
    
    public async Task<double> GetIncomeForWholeCompanyExcepted(string? currCode)
    {
        var totalIncomeFromContracts = await _context.Contracts
            .SumAsync(c => c.TotalPrice);

        var totalIncomeFromSubscriptions = await _context.PaymentsSubscriptions
            .SumAsync(ps => ps.Amount);

        var totalIncomeFromMonthlyFees = await _context.Subscriptions
            .SumAsync(s => s.PricePerMonth);

        var totalIncome = totalIncomeFromContracts + totalIncomeFromSubscriptions + totalIncomeFromMonthlyFees;
        
        if(currCode != null)
        {
            using var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync("https://api.nbp.pl/api/exchangerates/rates/a/"+currCode+"/?format=json");

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(responseContent);
                var exchangeRate = jsonDocument.RootElement.GetProperty("rates")[0].GetProperty("mid").GetDouble();

                totalIncome /= exchangeRate;
            }
            else
            {
                throw new ArgumentException("Invalid currency code");
            }
        }
        return totalIncome;
    }

    public async Task<double> GetIncomeForSoftwareCurrentExcepted(int softwareId, string? currCode)
    {
        if (softwareId < 1)
        {
            throw new ArgumentException("Invalid software ID");
        }
        
        var softwareExists = await _context.Software.AnyAsync(s => s.SoftwareId == softwareId);
        if (!softwareExists)
        {
            throw new NotFoundException("Software does not exist");
        }
        
        var softwareIncomeFromContracts = await _context.Contracts
            .Where(c => c.IdSoftware == softwareId)
            .SumAsync(c => c.TotalPrice);

        var softwareIncomeFromSubscriptions = await _context.PaymentsSubscriptions
            .Where(ps => ps.Subscription.SoftwareId == softwareId)
            .SumAsync(ps => ps.Amount);

        var softwareIncomeFromMonthlyFees = await _context.Subscriptions
            .Where(s => s.SoftwareId == softwareId)
            .SumAsync(s => s.PricePerMonth);
        
        var totalIncome = softwareIncomeFromContracts + softwareIncomeFromSubscriptions + softwareIncomeFromMonthlyFees;
        
        if(currCode != null)
        {
            using var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync("https://api.nbp.pl/api/exchangerates/rates/a/"+currCode+"/?format=json");

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(responseContent);
                var exchangeRate = jsonDocument.RootElement.GetProperty("rates")[0].GetProperty("mid").GetDouble();

                totalIncome /= exchangeRate;
            }
            else
            {
                throw new ArgumentException("Invalid currency code");
            }
        }
        return totalIncome;
    }
    
}