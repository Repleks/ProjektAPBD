using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjektAPBD.Migrations
{
    /// <inheritdoc />
    public partial class companyDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    IdCompany = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KRS = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.IdCompany);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    IdDiscount = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.IdDiscount);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    IdPerson = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pesel = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.IdPerson);
                });

            migrationBuilder.CreateTable(
                name: "Software",
                columns: table => new
                {
                    IdSoftware = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CurrentVersion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Software", x => x.IdSoftware);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    IdCustomer = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.IdCustomer);
                    table.ForeignKey(
                        name: "FK_Customers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "IdCompany");
                    table.ForeignKey(
                        name: "FK_Customers_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "IdPerson");
                });

            migrationBuilder.CreateTable(
                name: "SoftwareVersions",
                columns: table => new
                {
                    IdSoftwareVersion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSoftware = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareVersions", x => x.IdSoftwareVersion);
                    table.ForeignKey(
                        name: "FK_SoftwareVersions_Software_IdSoftware",
                        column: x => x.IdSoftware,
                        principalTable: "Software",
                        principalColumn: "IdSoftware",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    IdContract = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCustomer = table.Column<int>(type: "int", nullable: false),
                    IdSoftware = table.Column<int>(type: "int", nullable: false),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PricePerMonth = table.Column<double>(type: "float", nullable: false),
                    AdditionalSupport = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    Signed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.IdContract);
                    table.ForeignKey(
                        name: "FK_Contracts_Customers_IdCustomer",
                        column: x => x.IdCustomer,
                        principalTable: "Customers",
                        principalColumn: "IdCustomer",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contracts_Software_IdSoftware",
                        column: x => x.IdSoftware,
                        principalTable: "Software",
                        principalColumn: "IdSoftware",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    IdSubscription = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Customer = table.Column<int>(type: "int", nullable: false),
                    Software = table.Column<int>(type: "int", nullable: false),
                    RenewalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PricePerMonth = table.Column<double>(type: "float", nullable: false),
                    ActiveStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.IdSubscription);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Customers_Customer",
                        column: x => x.Customer,
                        principalTable: "Customers",
                        principalColumn: "IdCustomer",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Software_Software",
                        column: x => x.Software,
                        principalTable: "Software",
                        principalColumn: "IdSoftware",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contracts_Discounts",
                columns: table => new
                {
                    IdContract = table.Column<int>(type: "int", nullable: false),
                    IdDiscount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts_Discounts", x => new { x.IdContract, x.IdDiscount });
                    table.ForeignKey(
                        name: "FK_Contracts_Discounts_Contracts_IdContract",
                        column: x => x.IdContract,
                        principalTable: "Contracts",
                        principalColumn: "IdContract",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contracts_Discounts_Discounts_IdDiscount",
                        column: x => x.IdDiscount,
                        principalTable: "Discounts",
                        principalColumn: "IdDiscount",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentsContracts",
                columns: table => new
                {
                    IdPayment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdContract = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    PaymentInformation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentsContracts", x => x.IdPayment);
                    table.ForeignKey(
                        name: "FK_PaymentsContracts_Contracts_IdContract",
                        column: x => x.IdContract,
                        principalTable: "Contracts",
                        principalColumn: "IdContract",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentsSubscriptions",
                columns: table => new
                {
                    IdPayment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSubscription = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentsSubscriptions", x => x.IdPayment);
                    table.ForeignKey(
                        name: "FK_PaymentsSubscriptions_Subscriptions_IdSubscription",
                        column: x => x.IdSubscription,
                        principalTable: "Subscriptions",
                        principalColumn: "IdSubscription",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions_Discounts",
                columns: table => new
                {
                    IdSubscription = table.Column<int>(type: "int", nullable: false),
                    IdDiscount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions_Discounts", x => new { x.IdSubscription, x.IdDiscount });
                    table.ForeignKey(
                        name: "FK_Subscriptions_Discounts_Discounts_IdDiscount",
                        column: x => x.IdDiscount,
                        principalTable: "Discounts",
                        principalColumn: "IdDiscount",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Discounts_Subscriptions_IdSubscription",
                        column: x => x.IdSubscription,
                        principalTable: "Subscriptions",
                        principalColumn: "IdSubscription",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "IdCompany", "Address", "Email", "KRS", "Name", "Phone" },
                values: new object[] { 1, "123 Test St", "testcompany@gmail.com", "12345678901234", "TestCompany", "123456789" });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "IdDiscount", "DateEnd", "DateStart", "Description", "Value" },
                values: new object[] { 1, new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "New Year Discount", 10 });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "IdPerson", "Address", "Email", "FirstName", "LastName", "Pesel", "Phone", "SoftDelete" },
                values: new object[] { 1, "Test address", "jankowalski@gmail.com", "Jan", "Kowalski", "12345678901", "098765432", false });

            migrationBuilder.InsertData(
                table: "Software",
                columns: new[] { "IdSoftware", "Category", "CurrentVersion", "Description", "Name" },
                values: new object[] { 1, "Finance", "1.2", "This is a test software.", "Test" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "IdCustomer", "CompanyId", "PersonId" },
                values: new object[,]
                {
                    { 1, null, 1 },
                    { 2, 1, null }
                });

            migrationBuilder.InsertData(
                table: "SoftwareVersions",
                columns: new[] { "IdSoftwareVersion", "IdSoftware", "Version" },
                values: new object[,]
                {
                    { 1, 1, "1.0" },
                    { 2, 1, "1.1" }
                });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "IdContract", "AdditionalSupport", "DateFrom", "DateTo", "IdCustomer", "IdSoftware", "PricePerMonth", "Signed", "TotalPrice" },
                values: new object[] { 1, 0, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 100.0, false, 1200.0 });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "IdSubscription", "ActiveStatus", "Customer", "PricePerMonth", "RenewalDate", "Software" },
                values: new object[] { 1, true, 1, 100.0, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Contracts_Discounts",
                columns: new[] { "IdContract", "IdDiscount" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "PaymentsContracts",
                columns: new[] { "IdPayment", "Amount", "IdContract", "PaymentDate", "PaymentInformation" },
                values: new object[] { 1, 1200.0, 1, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test payment" });

            migrationBuilder.InsertData(
                table: "PaymentsSubscriptions",
                columns: new[] { "IdPayment", "Amount", "PaymentDate", "IdSubscription" },
                values: new object[] { 1, 100.0, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Subscriptions_Discounts",
                columns: new[] { "IdDiscount", "IdSubscription" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_IdCustomer",
                table: "Contracts",
                column: "IdCustomer");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_IdSoftware",
                table: "Contracts",
                column: "IdSoftware");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_Discounts_IdDiscount",
                table: "Contracts_Discounts",
                column: "IdDiscount");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CompanyId",
                table: "Customers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PersonId",
                table: "Customers",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsContracts_IdContract",
                table: "PaymentsContracts",
                column: "IdContract");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsSubscriptions_IdSubscription",
                table: "PaymentsSubscriptions",
                column: "IdSubscription");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareVersions_IdSoftware",
                table: "SoftwareVersions",
                column: "IdSoftware");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Customer",
                table: "Subscriptions",
                column: "Customer");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Software",
                table: "Subscriptions",
                column: "Software");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Discounts_IdDiscount",
                table: "Subscriptions_Discounts",
                column: "IdDiscount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contracts_Discounts");

            migrationBuilder.DropTable(
                name: "PaymentsContracts");

            migrationBuilder.DropTable(
                name: "PaymentsSubscriptions");

            migrationBuilder.DropTable(
                name: "SoftwareVersions");

            migrationBuilder.DropTable(
                name: "Subscriptions_Discounts");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Software");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
