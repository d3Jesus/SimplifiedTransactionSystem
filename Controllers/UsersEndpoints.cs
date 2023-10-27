using ImprovedPicpay.Services;
using ImprovedPicpay.ViewModels.Users;
using Carter;
using MediatR;
using ImprovedPicpay.Features.Users;

namespace ImprovedPicpay.Controllers;

public class UsersEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var route = app.MapGroup("api/users");

        route.MapGet("", async (UserService userService) =>
        {
            return Results.Ok(await userService.GetAllAsync());
        });

        route.MapGet("{id}", async (string id, UserService userService) =>
        {
            return Results.Ok(await userService.GetByAsync(id));
        });

        route.MapPost("", async (CreateUser.Command command, ISender sender) =>
        {
            var result = await sender.Send(command);

            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Data);
        });

        route.MapPut("", async (UpdateUserViewModel model, UserService service) =>
        {
            var response = await service.UpdateAsync(model);

            if (!response.Succeeded)
                return Results.BadRequest("Error updating");

            return Results.Ok("Updated");
        });
    }
}
