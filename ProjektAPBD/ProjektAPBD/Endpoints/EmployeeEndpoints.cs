using Microsoft.AspNetCore.Mvc;
using ProjektAPBD.Exceptions;
using ProjektAPBD.RequestModels.EmployeeRequestModels;
using ProjektAPBD.Services;

namespace ProjektAPBD.Endpoints;

public static class EmployeeEndpoints
{
    public static void RegisterEmployeeEndpoints(this RouteGroupBuilder builder)
    {
        var group = builder.MapGroup("employees");
        
        group.MapPost("register", async ([FromBody] RegisterEmployeeRequestModel model, IEmployeeService service) =>
        {
            try
            {
                var result = await service.Register(model.Username, model.Password, model.Role);
                return Results.Ok("Employee created " + result.Id);
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
        
        group.MapGet("show", async (IEmployeeService service) =>
        {
            try
            {
                var result = await service.GetAll();
                return Results.Ok(result);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }).RequireAuthorization("admin");

        
        group.MapPost("login", async (string username, string password, IEmployeeService service) =>
        {
            try
            {
                var result = await service.Login(username, password);
                return Results.Ok(result);
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
        });
        
        group.MapDelete("delete", async (int id, IEmployeeService service) =>
        {
            try
            {
                var result = await service.DeleteEmployee(id);
                return Results.Ok("Employee deleted " + result);
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
        }).RequireAuthorization("admin");
    }
}