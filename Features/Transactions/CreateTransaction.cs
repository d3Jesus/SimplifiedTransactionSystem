using Carter;
using FluentValidation;
using ImprovedPicpay.Abstractions;
using ImprovedPicpay.Data;
using ImprovedPicpay.Entities;
using ImprovedPicpay.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace ImprovedPicpay.Features.Transactions;

public static class CreateTransaction
{
    public record TransactionCommand(string SenderId,
                          string ReceiverId,
                          decimal Amount) : IRequest<ServiceResponse<string>>;

    public class Validator : AbstractValidator<TransactionCommand>
    {
        public Validator()
        {
            RuleFor(v => v.SenderId).NotEmpty().NotNull();
            RuleFor(v => v.ReceiverId).NotEmpty().NotNull();
            RuleFor(v => v.Amount).NotEmpty().NotNull();
        }
    }

    internal sealed class Handler : IRequestHandler<TransactionCommand, ServiceResponse<string>>
    {
        private readonly IValidator<TransactionCommand> _validator;
        private readonly ApplicationDbContext _context;
        private readonly ITransactionService _transactionService;

        public Handler(IValidator<TransactionCommand> validator,
                       ApplicationDbContext context,
                       ITransactionService transactionService)
        {
            _validator = validator;
            _context = context;
            _transactionService = transactionService;
        }

        public async Task<ServiceResponse<string>> Handle(TransactionCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return ServiceResponse.Failure<string>(
                    new Error("CreateTransaction.Validation", validationResult.ToString()));
            }

            User sender = await GetUser(request.SenderId);
            User receiver = await GetUser(request.ReceiverId);

            var transaction = sender.Transfer(receiver, request.Amount);

            _context.Entry(sender).State = EntityState.Modified;

            receiver.IncreaseBalance(request.Amount);
            _context.Entry(receiver).State = EntityState.Modified;

            _context.Add(transaction);

            await _context.SaveChangesAsync(cancellationToken);

            // send notification

            return transaction.Id;
        }

        private async Task<User> GetUser(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);
        }
    }
}

public class CreateTransactionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/transactions", async (CreateTransaction.TransactionCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Data);
        });
    }
}