using Carter;
using ImprovedPicpay.Services;
using ImprovedPicpay.ViewModels.Transaction;

namespace ImprovedPicpay.Controllers;

public class TransactionsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var route = app.MapGroup("api/transaction");

        route.MapGet("", async (TransactionService service) =>
        {
            return Results.Ok(await service.GetAllAsync());
        });

        route.MapGet("{senderId}", async (string senderId, TransactionService service) =>
        {
            return Results.Ok(await service.GetByAsync(senderId));
        });

        route.MapPost("", async (AddTransactionViewModel model, TransactionService service) =>
        {
            var response = await service.CreateAsync(model);

            return Results.Ok(response);
        });
    }
}
