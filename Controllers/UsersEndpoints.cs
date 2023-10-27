using ImprovedPicpay.Services;
using ImprovedPicpay.ViewModels.Users;
using Carter;

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

        route.MapPost("", async (AddUserViewModel model, UserService service) =>
        {
            var response = await service.AddAsync(model);

            if (!response.Succeeded)
                return Results.BadRequest("Error creating");

            return Results.Ok("Created");
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
