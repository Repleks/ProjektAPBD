using Microsoft.AspNetCore.Mvc;
using ProjektAPBD.Exceptions;
using ProjektAPBD.RequestModels.PaymentSubcriptionRequestModels;
using ProjektAPBD.Services;

namespace ProjektAPBD.Endpoints;

public static class PaymentSubscriptionEndpoints
{
    public static void RegisterPaymentSubscriptionEndpoints(this RouteGroupBuilder builder)
    {
        var group = builder.MapGroup("paymentSubscriptions");

        group.MapPost("", async ([FromBody] PostPaymentSubscriptionRequestModel model, IPaymentSubscriptionService service) =>
        {
            try
            {
                var result = await service.PostPaymentSubscription(model);
                return Results.Ok("PaymentSubscription created " + result);
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
        
        
        group.MapGet("", async (IPaymentSubscriptionService service) =>
        {
            try
            {
                var result = await service.GetAllPaymentSubscriptions();
                return Results.Ok(result);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }).RequireAuthorization("admin");
    }
}