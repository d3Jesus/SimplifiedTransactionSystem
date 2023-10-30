using Carter;
using FluentValidation;
using ImprovedPicpay.Data;
using ImprovedPicpay.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ImprovedPicpay.Features.Transactions;

public class GetTransactionsBySenderId
{
    public record Query(string SenderId) : IRequest<ServiceResponse<List<TransactionResponse>>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(c => c.SenderId)
                .NotEmpty()
                .NotNull();
        }
    }

    internal sealed class Handler : IRequestHandler<Query, ServiceResponse<List<TransactionResponse>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<Query> _validator;

        public Handler(ApplicationDbContext context, IValidator<Query> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<ServiceResponse<List<TransactionResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return ServiceResponse.Failure<List<TransactionResponse>>(
                    new Error("GetTransactionsBySenderId.Validation", validationResult.ToString()));
            }

            var transactions = await _context.Transactions
                .Where(tr => tr.SenderId.Equals(request.SenderId))
                .Select(tr => new TransactionResponse
                {
                    Id = tr.Id,
                    SenderId = tr.SenderId,
                    ReceiverId = tr.ReceiverId,
                    Amount = tr.Amount,
                    Timestamp = tr.Timestamp
                })
                .ToListAsync(cancellationToken);

            if (transactions is null)
            {
                return ServiceResponse.Failure<List<TransactionResponse>>(
                    new Error("GetTransactionsBySenderId.Null", "Transactions made by the given sender ID were not found."));
            }

            return transactions;
        }
    }
}

public class GetTransactionsBySenderIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/transactions/{senderId}", async (string senderId, ISender sender) =>
        {
            var query = new GetTransactionsBySenderId.Query(senderId);
            var result = await sender.Send(query);

            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Data);
        });
    }
}