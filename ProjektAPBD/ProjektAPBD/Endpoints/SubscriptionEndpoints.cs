using Microsoft.AspNetCore.Mvc;
using ProjektAPBD.Exceptions;
using ProjektAPBD.RequestModels.SubscriptionRequestModels;
using ProjektAPBD.Services;

namespace ProjektAPBD.Endpoints;

public static class SubscriptionEndpoints
{
    public static void RegisterSubscriptionEndpoints(this RouteGroupBuilder builder)
    {
        var group = builder.MapGroup("subscriptions");

        group.MapPost("", async ([FromBody] PostSubscriptionRequestModel model, ISubcriptionService service) =>
        {
            try
            {
                var result = await service.PostSubcription(model);
                return Results.Ok("Subscription created "+result);
            }
            catch (ArgumentException e)
            {
                return Results.BadRequest(e.Message);
            }
            catch (NotFoundException e)
            {
                return Results.NotFound(e.Message);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }).RequireAuthorization("user");
        
        group.MapGet("", async (ISubcriptionService service) =>
        {
            try
            {
                var result = await service.GetAllSubcription();
                return Results.Ok(result);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }).RequireAuthorization("admin");
    }
}