﻿using Carter;
using FluentValidation;
using ImprovedPicpay.Abstractions;
using ImprovedPicpay.Data;
using ImprovedPicpay.Entities;
using ImprovedPicpay.Enums;
using ImprovedPicpay.Helpers;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

            // Get authorization
            var result = await _transactionService.IsAuthorized();
            if (result)
            {
                return ServiceResponse.Failure<string>(
                    new Error("CreateTransaction.TransactionNotAuthorized", "This transaction is not authorized."));
            }

            bool senderIsCommonUser = sender.UserType.Equals(UserTypes.Common.ToString());
            if (senderIsCommonUser)
            {
                return ServiceResponse.Failure<string>(
                    new Error("CreateTransaction.UserTypeCannotSendMoney", "Common users can only receive money."));
            }

            bool senderHasEnoughBalance = sender.Balance >= request.Amount;
            if (!senderHasEnoughBalance)
            {
                return ServiceResponse.Failure<string>(
                    new Error("CreateTransaction.LowBalance", "You don't have enough balance to perform this operation."));
            }

            sender.Balance -= request.Amount;
            _context.Entry(sender).State = EntityState.Modified;

            receiver.Balance += request.Amount;
            _context.Entry(receiver).State = EntityState.Modified;

            var transaction = request.Adapt<Transaction>();
            transaction.Id = Guid.NewGuid().ToString();
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