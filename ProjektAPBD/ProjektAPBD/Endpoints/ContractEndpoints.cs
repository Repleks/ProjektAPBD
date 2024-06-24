using Microsoft.AspNetCore.Mvc;
using ProjektAPBD.Exceptions;
using ProjektAPBD.RequestModels.ContractRequestModels;
using ProjektAPBD.Services;

namespace ProjektAPBD.Endpoints;

public static class ContractEndpoints
{
    public static void RegisterContractEndpoints(this RouteGroupBuilder builder)
    {
        var group = builder.MapGroup("contracts");

        group.MapPost("", async ([FromBody] PostContractRequestModel model, IContractService service) =>
        {
            try
            {
                var result = await service.PostContract(model);
                return Results.Ok("Contract created " + result);
            }
            catch (ArgumentException e)
            {
                return Results.BadRequest(e.Message);
            }
            catch (NotFoundException e)
            {
                return Results.NotFound(e.Message);
            }
            catch (AlreadyExistsException e)
            {
                return Results.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        });

        group.MapGet("", async (IContractService service) =>
        {
            try
            {
                var result = await service.GetAllContracts();
                return Results.Ok(result);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        });
    }
}