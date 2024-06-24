using Microsoft.AspNetCore.Mvc;
using ProjektAPBD.Exceptions;
using ProjektAPBD.RequestModels.PaymentContractRequestModels;
using ProjektAPBD.Services;

namespace ProjektAPBD.Endpoints;

public static class PaymentContractEndpoints
{
    public static void RegisterPaymentContractEndpoints(this RouteGroupBuilder builder)
    {
        var group = builder.MapGroup("paymentcontracts");
        
        group.MapPost("", async ([FromBody] PostPaymentContractRequestModel model, IPaymentContractService service) =>
        {
            try
            {
                var result = await service.PostPaymentContract(model);
                return Results.Ok("Payment contract created " + result);
            }
            catch (ArgumentException e)
            {
                return Results.BadRequest(e.Message);
            }
            catch (NotFoundException e)
            {
                return Results.NotFound(e.Message);
            }
            catch (TooLateException e)
            {
                return Results.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        });
        
        group.MapGet("", async (IPaymentContractService service) =>
        {
            try
            {
                var result = await service.GetAllPaymentsContracts();
                return Results.Ok(result);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        });
    }
}