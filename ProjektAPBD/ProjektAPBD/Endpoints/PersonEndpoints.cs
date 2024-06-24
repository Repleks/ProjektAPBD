﻿using Kolokwium2.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjektAPBD.RequestModels.PersonRequestModels;
using ProjektAPBD.Services;

namespace ProjektAPBD.Endpoints;

public static class PersonEndpoints
{
    public static void RegisterPersonEndpoints(this RouteGroupBuilder builder)
    {
        var group = builder.MapGroup("persons");

        group.MapPost("", async ([FromBody] PostPersonRequestModel model, IPersonService service) =>
        {
            try
            {
                var result = await service.PostPerson(model);
                return Results.Ok("Person created "+result);
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
        
        group.MapPost("update", async ([FromBody] UpdatePersonRequestModel model, IPersonService service) =>
        {
            try
            {
                var result = await service.UpdatePerson(model);
                return Results.Ok("Person updated "+result);
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
        
        group.MapGet("", async (IPersonService service) =>
        {
            try
            {
                var result = await service.GetAllPersons();
                return Results.Ok(result);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        });
        
        group.MapDelete("{id}", async ([FromRoute] int id, IPersonService service) =>
        {
            try
            {
                var result = await service.DeletePerson(id);
                return Results.Ok("Person deleted " + result);
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