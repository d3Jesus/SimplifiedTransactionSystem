using Carter;
using ImprovedPicpay.Data;
using ImprovedPicpay.Helpers;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ImprovedPicpay.Features.Users;

public static partial class GetUsers
{
    public record Query() : IRequest<ServiceResponse<List<UsersResponse>>>;

    internal sealed class Handler : IRequestHandler<Query, ServiceResponse<List<UsersResponse>>>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<UsersResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _context.Users.ToListAsync();

            var users = result.Adapt<List<UsersResponse>>();

            return users;
        }
    }
}

public class GetUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("users", async (ISender sender) =>
        {
            var result = await sender.Send(new GetUsers.Query());

            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Data);
        });
    }
}