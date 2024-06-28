﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjektAPBD.Contexts;

#nullable disable

namespace ProjektAPBD.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProjektAPBD.Models.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdCompany");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CompanyId"));

                    b.Property<string>("CompanyAddress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Address");

                    b.Property<string>("CompanyEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Email");

                    b.Property<string>("CompanyKRS")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)")
                        .HasColumnName("KRS");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Name");

                    b.Property<string>("CompanyPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Phone");

                    b.HasKey("CompanyId");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            CompanyId = 1,
                            CompanyAddress = "123 Test St",
                            CompanyEmail = "testcompany@gmail.com",
                            CompanyKRS = "123456789",
                            CompanyName = "TestCompany",
                            CompanyPhone = "123456789"
                        });
                });

            modelBuilder.Entity("ProjektAPBD.Models.Contract", b =>
                {
                    b.Property<int>("IdContract")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdContract");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdContract"));

                    b.Property<int>("AdditionalSupport")
                        .HasColumnType("int")
                        .HasColumnName("AdditionalSupport");

                    b.Property<DateTime>("DateFrom")
                        .HasColumnType("datetime2")
                        .HasColumnName("DateFrom");

                    b.Property<DateTime>("DateTo")
                        .HasColumnType("datetime2")
                        .HasColumnName("DateTo");

                    b.Property<int>("DiscountId")
                        .HasColumnType("int")
                        .HasColumnName("Discount");

                    b.Property<int>("IdCustomer")
                        .HasColumnType("int")
                        .HasColumnName("IdCustomer");

                    b.Property<int>("IdSoftware")
                        .HasColumnType("int")
                        .HasColumnName("IdSoftware");

                    b.Property<double>("PricePerMonth")
                        .HasColumnType("float")
                        .HasColumnName("PricePerMonth");

                    b.Property<bool>("Signed")
                        .HasColumnType("bit")
                        .HasColumnName("Signed");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("float")
                        .HasColumnName("TotalPrice");

                    b.HasKey("IdContract");

                    b.HasIndex("IdCustomer");

                    b.HasIndex("IdSoftware");

                    b.ToTable("Contracts");

                    b.HasData(
                        new
                        {
                            IdContract = 1,
                            AdditionalSupport = 0,
                            DateFrom = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateTo = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DiscountId = 0,
                            IdCustomer = 1,
                            IdSoftware = 1,
                            PricePerMonth = 100.0,
                            Signed = false,
                            TotalPrice = 1200.0
                        });
                });

            modelBuilder.Entity("ProjektAPBD.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdCustomer");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerId"));

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int?>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("CustomerId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("PersonId");

                    b.ToTable("Customers");

                    b.HasData(
                        new
                        {
                            CustomerId = 1,
                            PersonId = 1
                        },
                        new
                        {
                            CustomerId = 2,
                            CompanyId = 1
                        });
                });

            modelBuilder.Entity("ProjektAPBD.Models.Discount", b =>
                {
                    b.Property<int>("DiscountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdDiscount");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DiscountId"));

                    b.Property<DateTime>("DiscountDateEnd")
                        .HasColumnType("datetime2")
                        .HasColumnName("DateEnd");

                    b.Property<DateTime>("DiscountDateStart")
                        .HasColumnType("datetime2")
                        .HasColumnName("DateStart");

                    b.Property<string>("DiscountDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Description");

                    b.Property<int>("DiscountValue")
                        .HasColumnType("int")
                        .HasColumnName("Value");

                    b.HasKey("DiscountId");

                    b.ToTable("Discounts");

                    b.HasData(
                        new
                        {
                            DiscountId = 1,
                            DiscountDateEnd = new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DiscountDateStart = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DiscountDescription = "New Year Discount",
                            DiscountValue = 10
                        },
                        new
                        {
                            DiscountId = 2,
                            DiscountDateEnd = new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DiscountDateStart = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DiscountDescription = "Test Discount",
                            DiscountValue = 50
                        });
                });

            modelBuilder.Entity("ProjektAPBD.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("ProjektAPBD.Models.PaymentContract", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdPayment");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentId"));

                    b.Property<double>("Amount")
                        .HasColumnType("float")
                        .HasColumnName("Amount");

                    b.Property<int>("ContractId")
                        .HasColumnType("int")
                        .HasColumnName("IdContract");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("PaymentDate");

                    b.Property<string>("PaymentInformation")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("PaymentInformation");

                    b.HasKey("PaymentId");

                    b.HasIndex("ContractId");

                    b.ToTable("PaymentsContracts");

                    b.HasData(
                        new
                        {
                            PaymentId = 1,
                            Amount = 1200.0,
                            ContractId = 1,
                            PaymentDate = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PaymentInformation = "Test payment"
                        });
                });

            modelBuilder.Entity("ProjektAPBD.Models.PaymentSubscription", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdPayment");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentId"));

                    b.Property<double>("Amount")
                        .HasColumnType("float")
                        .HasColumnName("Amount");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("PaymentDate");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("int")
                        .HasColumnName("IdSubscription");

                    b.HasKey("PaymentId");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("PaymentsSubscriptions");

                    b.HasData(
                        new
                        {
                            PaymentId = 1,
                            Amount = 100.0,
                            PaymentDate = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SubscriptionId = 1
                        });
                });

            modelBuilder.Entity("ProjektAPBD.Models.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdPerson");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PersonId"));

                    b.Property<string>("PersonAddress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Address");

                    b.Property<string>("PersonEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Email");

                    b.Property<string>("PersonFirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("FirstName");

                    b.Property<string>("PersonLastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("LastName");

                    b.Property<string>("PersonPesel")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)")
                        .HasColumnName("Pesel");

                    b.Property<string>("PersonPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Phone");

                    b.Property<bool>("PersonSoftDelete")
                        .HasColumnType("bit")
                        .HasColumnName("SoftDelete");

                    b.HasKey("PersonId");

                    b.ToTable("Persons");

                    b.HasData(
                        new
                        {
                            PersonId = 1,
                            PersonAddress = "Test address",
                            PersonEmail = "jankowalski@gmail.com",
                            PersonFirstName = "Jan",
                            PersonLastName = "Kowalski",
                            PersonPesel = "12345678901",
                            PersonPhone = "098765432",
                            PersonSoftDelete = false
                        });
                });

            modelBuilder.Entity("ProjektAPBD.Models.Software", b =>
                {
                    b.Property<int>("SoftwareId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdSoftware");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SoftwareId"));

                    b.Property<string>("SoftwareCategory")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Category");

                    b.Property<string>("SoftwareCurrentVersion")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("CurrentVersion");

                    b.Property<string>("SoftwareDescription")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Description");

                    b.Property<string>("SoftwareName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Name");

                    b.HasKey("SoftwareId");

                    b.ToTable("Software");

                    b.HasData(
                        new
                        {
                            SoftwareId = 1,
                            SoftwareCategory = "Finance",
                            SoftwareCurrentVersion = "1.2",
                            SoftwareDescription = "This is a test software.",
                            SoftwareName = "Test"
                        },
                        new
                        {
                            SoftwareId = 2,
                            SoftwareCategory = "Bookkeeping",
                            SoftwareCurrentVersion = "1.0",
                            SoftwareDescription = "This is new test software.",
                            SoftwareName = "Test2"
                        });
                });

            modelBuilder.Entity("ProjektAPBD.Models.SoftwareDiscount", b =>
                {
                    b.Property<int>("SoftwareDiscountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdSoftwareDiscount");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SoftwareDiscountId"));

                    b.Property<int>("DiscountId")
                        .HasColumnType("int")
                        .HasColumnName("IdDiscount");

                    b.Property<int>("SoftwareId")
                        .HasColumnType("int")
                        .HasColumnName("IdSoftware");

                    b.HasKey("SoftwareDiscountId");

                    b.HasIndex("DiscountId");

                    b.HasIndex("SoftwareId");

                    b.ToTable("SoftwareDiscounts");

                    b.HasData(
                        new
                        {
                            SoftwareDiscountId = 1,
                            DiscountId = 1,
                            SoftwareId = 1
                        },
                        new
                        {
                            SoftwareDiscountId = 2,
                            DiscountId = 1,
                            SoftwareId = 2
                        },
                        new
                        {
                            SoftwareDiscountId = 3,
                            DiscountId = 2,
                            SoftwareId = 2
                        });
                });

            modelBuilder.Entity("ProjektAPBD.Models.SoftwareVersion", b =>
                {
                    b.Property<int>("IdSoftwareVersion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSoftwareVersion"));

                    b.Property<int>("IdSoftware")
                        .HasColumnType("int");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdSoftwareVersion");

                    b.HasIndex("IdSoftware");

                    b.ToTable("SoftwareVersions");

                    b.HasData(
                        new
                        {
                            IdSoftwareVersion = 1,
                            IdSoftware = 1,
                            Version = "1.0"
                        },
                        new
                        {
                            IdSoftwareVersion = 2,
                            IdSoftware = 1,
                            Version = "1.1"
                        },
                        new
                        {
                            IdSoftwareVersion = 3,
                            IdSoftware = 2,
                            Version = "1.0"
                        });
                });

            modelBuilder.Entity("ProjektAPBD.Models.Subscription", b =>
                {
                    b.Property<int>("SubscriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdSubscription");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubscriptionId"));

                    b.Property<DateTime>("ActivationDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("ActivationDate");

                    b.Property<bool>("ActiveStatus")
                        .HasColumnType("bit")
                        .HasColumnName("ActiveStatus");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int")
                        .HasColumnName("Customer");

                    b.Property<double>("FirstPaymentPrice")
                        .HasColumnType("float")
                        .HasColumnName("FirstPaymentPrice");

                    b.Property<double>("PricePerMonth")
                        .HasColumnType("float")
                        .HasColumnName("PricePerMonth");

                    b.Property<DateTime>("RenewalDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("RenewalDate");

                    b.Property<int>("SoftwareId")
                        .HasColumnType("int")
                        .HasColumnName("Software");

                    b.Property<string>("SubscriptionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name");

                    b.HasKey("SubscriptionId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("SoftwareId");

                    b.ToTable("Subscriptions");

                    b.HasData(
                        new
                        {
                            SubscriptionId = 1,
                            ActivationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ActiveStatus = true,
                            CustomerId = 1,
                            FirstPaymentPrice = 100.0,
                            PricePerMonth = 100.0,
                            RenewalDate = new DateTime(2024, 7, 28, 21, 21, 4, 169, DateTimeKind.Local).AddTicks(9546),
                            SoftwareId = 1,
                            SubscriptionName = "Test Subscription"
                        });
                });

            modelBuilder.Entity("ProjektAPBD.Models.Contract", b =>
                {
                    b.HasOne("ProjektAPBD.Models.Customer", "Customer")
                        .WithMany("Contracts")
                        .HasForeignKey("IdCustomer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjektAPBD.Models.Software", "Software")
                        .WithMany("Contracts")
                        .HasForeignKey("IdSoftware")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Software");
                });

            modelBuilder.Entity("ProjektAPBD.Models.Customer", b =>
                {
                    b.HasOne("ProjektAPBD.Models.Company", "Company")
                        .WithMany("Customers")
                        .HasForeignKey("CompanyId");

                    b.HasOne("ProjektAPBD.Models.Person", "Person")
                        .WithMany("Customers")
                        .HasForeignKey("PersonId");

                    b.Navigation("Company");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("ProjektAPBD.Models.PaymentContract", b =>
                {
                    b.HasOne("ProjektAPBD.Models.Contract", "Contract")
                        .WithMany()
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("ProjektAPBD.Models.PaymentSubscription", b =>
                {
                    b.HasOne("ProjektAPBD.Models.Subscription", "Subscription")
                        .WithMany("PaymentSubscriptions")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("ProjektAPBD.Models.SoftwareDiscount", b =>
                {
                    b.HasOne("ProjektAPBD.Models.Discount", "Discount")
                        .WithMany("SoftwareDiscounts")
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjektAPBD.Models.Software", "Software")
                        .WithMany("SoftwareDiscounts")
                        .HasForeignKey("SoftwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Discount");

                    b.Navigation("Software");
                });

            modelBuilder.Entity("ProjektAPBD.Models.SoftwareVersion", b =>
                {
                    b.HasOne("ProjektAPBD.Models.Software", "Software")
                        .WithMany("SoftwareVersions")
                        .HasForeignKey("IdSoftware")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Software");
                });

            modelBuilder.Entity("ProjektAPBD.Models.Subscription", b =>
                {
                    b.HasOne("ProjektAPBD.Models.Customer", "Customer")
                        .WithMany("Subscriptions")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjektAPBD.Models.Software", "Software")
                        .WithMany()
                        .HasForeignKey("SoftwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Software");
                });

            modelBuilder.Entity("ProjektAPBD.Models.Company", b =>
                {
                    b.Navigation("Customers");
                });

            modelBuilder.Entity("ProjektAPBD.Models.Customer", b =>
                {
                    b.Navigation("Contracts");

                    b.Navigation("Subscriptions");
                });

            modelBuilder.Entity("ProjektAPBD.Models.Discount", b =>
                {
                    b.Navigation("SoftwareDiscounts");
                });

            modelBuilder.Entity("ProjektAPBD.Models.Person", b =>
                {
                    b.Navigation("Customers");
                });

            modelBuilder.Entity("ProjektAPBD.Models.Software", b =>
                {
                    b.Navigation("Contracts");

                    b.Navigation("SoftwareDiscounts");

                    b.Navigation("SoftwareVersions");
                });

            modelBuilder.Entity("ProjektAPBD.Models.Subscription", b =>
                {
                    b.Navigation("PaymentSubscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
