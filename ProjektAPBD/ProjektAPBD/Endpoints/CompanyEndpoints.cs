using Kolokwium2.Exceptions;
using Microsoft.AspNetCore.Mvc;
using ProjektAPBD.RequestModels;
using ProjektAPBD.Services;

namespace ProjektAPBD.Endpoints;

public static class CompanyEndpoints
{
    public static void RegisterCompanyEndpoints(this RouteGroupBuilder builder)
    {
        var group = builder.MapGroup("companies");

        group.MapPost("", async ([FromBody] PostCompanyRequestModel model, ICompanyService service) =>
        {
            try
            {
                var result = await service.PostCompany(model);
                return Results.Ok("Company created "+result);
            }
            catch (ArgumentException e)
            {
                return Results.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        });
        
        group.MapPost("update", async ([FromBody] UpdateCompanyRequestModel model, ICompanyService service) =>
        {
            try
            {
                var result = await service.UpdateCompany(model);
                return Results.Ok("Company updated "+result);
            }
            catch (NotFoundException e)
            {
                return Results.NotFound(e.Message);
            }
            catch (ArgumentException e)
            {
                return Results.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        });
        
        group.MapGet("", async (ICompanyService service) =>
        {
            try
            {
                var result = await service.GetAllCompanies();
                return Results.Ok(result);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        });
    }
}