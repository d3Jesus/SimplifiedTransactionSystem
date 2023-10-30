using Carter;
using FluentValidation;
using ImprovedPicpay.Data;
using ImprovedPicpay.Features.Users;
using ImprovedPicpay.Helpers;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ImprovedPicpay.Features.Transactions;

public class GetTransactionBySenderId
{
    public record Query(string SenderId) : IRequest<ServiceResponse<TransactionResponse>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(c => c.SenderId)
                .NotEmpty()
                .NotNull();
        }
    }

    internal sealed class Handler : IRequestHandler<Query, ServiceResponse<TransactionResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<Query> _validator;

        public Handler(ApplicationDbContext context, IValidator<Query> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<ServiceResponse<TransactionResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return ServiceResponse.Failure<TransactionResponse>(
                    new Error("GetTransactionBySenderId.Validation", validationResult.ToString()));
            }

            var result = await _context.Transactions
                .FirstOrDefaultAsync(tr => tr.SenderId.Equals(request.SenderId));

            var transaction = result.Adapt<TransactionResponse>();

            return transaction;
        }
    }
}

public class GetTransactionBySenderIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/transactions/{senderId}", async (string senderId, ISender sender) =>
        {
            var query = new GetTransactionBySenderId.Query(senderId);
            var result = await sender.Send(query);

            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Data);
        });
    }
}