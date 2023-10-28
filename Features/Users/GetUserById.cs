using Carter;
using FluentValidation;
using ImprovedPicpay.Data;
using ImprovedPicpay.Helpers;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ImprovedPicpay.Features.Users;

public static partial class GetUserById
{
    public record Query(string Id) : IRequest<ServiceResponse<UsersResponse>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .NotNull();
        }
    }

    internal sealed class Handler : IRequestHandler<Query, ServiceResponse<UsersResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<Query> _validator;

        public Handler(ApplicationDbContext context, IValidator<Query> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<ServiceResponse<UsersResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return ServiceResponse.Failure<UsersResponse>(
                    new Error("GetUserById.Validation", validationResult.ToString()));
            }

            var result = await _context.Users
                .FirstOrDefaultAsync(user => user.Id.Equals(request.Id));

            var user = result.Adapt<UsersResponse>();

            return user;
        }
    }
}

public class GetUserByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("users/{id}", async (string id, ISender sender) =>
        {
            var query = new GetUserById.Query ( id );
            var result = await sender.Send(query);

            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Data);
        });
    }
}