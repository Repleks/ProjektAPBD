using Microsoft.EntityFrameworkCore;
using ProjektAPBD.Contexts;
using ProjektAPBD.Endpoints;
using ProjektAPBD.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IPaymentContractService, PaymentContractService>();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var baseEndpointsGroup = app.MapGroup("api");

baseEndpointsGroup.RegisterCompanyEndpoints();
baseEndpointsGroup.RegisterPersonEndpoints();
baseEndpointsGroup.RegisterContractEndpoints();
baseEndpointsGroup.RegisterPaymentContractEndpoints();

app.Run();

