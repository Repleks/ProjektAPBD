using Microsoft.EntityFrameworkCore;
using ProjektAPBD.Models;

namespace ProjektAPBD.Contexts;

public class DatabaseContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    
    public DbSet<Contract> Contracts { get; set; }
    
    public DbSet<Customer> Customers { get; set; }
    
    public DbSet<Discount> Discounts { get; set; }
    
    public DbSet<PaymentContract> PaymentsContracts { get; set; }
    
    public DbSet<Person> Persons { get; set; }
    
    public DbSet<Software> Software { get; set; }
    
    public DbSet<Subscription> Subscriptions { get; set; }
    
    public DbSet<SubscriptionDicount> SubscriptionsDicounts { get; set; }
    
    public DbSet<PaymentSubscription> PaymentsSubscriptions { get; set; }
    
    public DbSet<ContractDiscount> ContractsDiscounts { get; set; }
    
    public DbSet<SoftwareVersion> SoftwareVersions { get; set; }
    
    
    
    protected DatabaseContext()
    {
    }
    
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Company>().HasData(new Company
        {
            CompanyId = 1,
            CompanyName = "TestCompany",
            CompanyAddress = "123 Test St",
            CompanyEmail = "testcompany@gmail.com",
            CompanyPhone = "123456789",
            CompanyKRS = "12345678901234"
        });

        modelBuilder.Entity<Person>().HasData(new Person
        {
            PersonId = 1,
            PersonFirstName = "Jan",
            PersonLastName = "Kowalski",
            PersonAddress = "Test address",
            PersonEmail = "jankowalski@gmail.com",
            PersonPhone = "098765432",
            PersonPesel = "12345678901",
            PersonSoftDelete = false
        });

        modelBuilder.Entity<Customer>().HasData(new Customer
        {
            CustomerId = 1,
            PersonId = 1,
            CompanyId = null
        },
        new Customer
        {
            CustomerId = 2,
            PersonId = null,
            CompanyId = 1
        });

        modelBuilder.Entity<Software>().HasData(new Software
        {
            SoftwareId = 1,
            SoftwareName = "Test",
            SoftwareDescription = "This is a test software.",
            SoftwareCurrentVersion = "1.2",
            SoftwareCategory = "Finance"
        });

        modelBuilder.Entity<Software>().HasData(new Software
        {
            SoftwareId = 2,
            SoftwareName = "Test2",
            SoftwareDescription = "This is new test software.",
            SoftwareCurrentVersion = "1.0",
            SoftwareCategory = "Bookkeeping"
        });

        modelBuilder.Entity<Discount>().HasData(new Discount
        {
            DiscountId = 1,
            DiscountDescription = "New Year Discount",
            DiscountValue = 10,
            DiscountDateStart = new DateTime(2023, 1, 1),
            DiscountDateEnd = new DateTime(2023, 12, 31)
        });
        
        modelBuilder.Entity<Discount>().HasData(new Discount
        {
            DiscountId = 2,
            DiscountDescription = "Test Discount",
            DiscountValue = 50,
            DiscountDateStart = new DateTime(2023, 1, 1),
            DiscountDateEnd = new DateTime(2025, 12, 31)
        });

        modelBuilder.Entity<Contract>().HasData(new Contract
        {
            IdContract = 1,
            IdCustomer = 1,
            IdSoftware = 1,
            DateFrom = new DateTime(2023, 1, 1),
            DateTo = new DateTime(2024, 1, 1),
            PricePerMonth = 100,
            AdditionalSupport = 0,
            TotalPrice = 1200,
            Signed = false
        });

        modelBuilder.Entity<ContractDiscount>().HasData(new ContractDiscount
        {
            IdContract = 1,
            IdDiscount = 1
        });

        modelBuilder.Entity<PaymentContract>().HasData(new PaymentContract
        {
            PaymentId = 1,
            ContractId = 1,
            PaymentDate = new DateTime(2023, 1, 1),
            Amount = 1200,
            PaymentInformation = "Test payment"
        });

        modelBuilder.Entity<Subscription>().HasData(new Subscription
        {
            SubscriptionId = 1,
            CustomerId = 1,
            SoftwareId = 1,
            RenewalDate = new DateTime(2023, 1, 1),
            PricePerMonth = 100,
            ActiveStatus = true
        });

        modelBuilder.Entity<SubscriptionDicount>().HasData(new SubscriptionDicount
        {
            IdSubscription = 1,
            IdDiscount = 1
        });

        modelBuilder.Entity<PaymentSubscription>().HasData(new PaymentSubscription
        {
            PaymentId = 1,
            SubscriptionId = 1,
            PaymentDate = new DateTime(2023, 1, 1),
            Amount = 100
        });
        
        modelBuilder.Entity<SoftwareVersion>().HasData(new SoftwareVersion
        {
            IdSoftwareVersion = 1,
            IdSoftware = 1,
            Version = "1.0"
        });
        
        modelBuilder.Entity<SoftwareVersion>().HasData(new SoftwareVersion
        {
            IdSoftwareVersion = 2,
            IdSoftware = 1,
            Version = "1.1"
        });
        
        modelBuilder.Entity<SoftwareVersion>().HasData(new SoftwareVersion
        {
            IdSoftwareVersion = 3,
            IdSoftware = 2,
            Version = "1.0"
        });
    }
}
