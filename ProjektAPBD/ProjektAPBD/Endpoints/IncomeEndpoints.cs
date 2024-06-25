using Microsoft.AspNetCore.Mvc;
using ProjektAPBD.Exceptions;
using ProjektAPBD.Services;

namespace ProjektAPBD.Endpoints;

public static class IncomeEndpoints
{
    public static void RegisterIncomeEndpoints(this RouteGroupBuilder builder)
    {
        var group = builder.MapGroup("income");

        group.MapPost("whole-company-current", async ([FromBody] string? currCode, IIncomeService service) =>
        {
            try
            {
                var result = await service.GetIncomeForWholeCompanyCurrent(currCode);
                return Results.Ok(result);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        });

        group.MapPost("software-current{softwareId:int}", async (int softwareId, [FromBody] string? currCode, IIncomeService service) =>
        {
            try
            {
                var result = await service.GetIncomeForSoftwareCurrentCurrent(softwareId, currCode);
                return Results.Ok(result);
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

        group.MapPost("whole-company-excepted", async ([FromBody] string? currCode, IIncomeService service) =>
        {
            try
            {
                var result = await service.GetIncomeForWholeCompanyExcepted(currCode);
                return Results.Ok(result);
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

        group.MapPost("software-excepted{softwareId:int}", async (int softwareId, [FromBody] string? currCode, IIncomeService service) =>
        {
            try
            {
                var result = await service.GetIncomeForSoftwareCurrentExcepted(softwareId, currCode);
                return Results.Ok(result);
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
    }
}