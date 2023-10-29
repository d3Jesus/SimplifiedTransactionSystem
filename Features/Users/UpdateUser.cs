using Carter;
using FluentValidation;
using ImprovedPicpay.Data;
using ImprovedPicpay.Entities;
using ImprovedPicpay.Helpers;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImprovedPicpay.Features.Users;

public static class UpdateUser
{
    public record Command(string Id,
                          string FirstName,
                          string MiddleName,
                          string LastName,
                          string Email,
                          string UserType,
                          string Password,
                          string Document,
                          decimal Balance) : IRequest<ServiceResponse<UsersResponse>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.FirstName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(c => c.MiddleName).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(c => c.LastName).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(c => c.Email).EmailAddress().NotEmpty().NotNull();
            RuleFor(c => c.UserType).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(c => c.Password).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(c => c.Document).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(c => c.Balance).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, ServiceResponse<UsersResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<Command> _validator;

        public Handler(ApplicationDbContext context, IValidator<Command> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<ServiceResponse<UsersResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return ServiceResponse.Failure<UsersResponse>(
                    new Error("UpdateUser.Validation", validationResult.ToString()));
            }

            bool isEmailTaken = _context.Users.Any(us => us.Email.Equals(request.Email));
            if (isEmailTaken)
            {
                return ServiceResponse.Failure<UsersResponse>(
                    new Error("CreateUser.EnforceUniqueEmail", "The given user email is not available."));
            }

            bool isDocumentTaken = _context.Users.Any(us => us.Document.Equals(request.Document));
            if (isDocumentTaken)
            {
                return ServiceResponse.Failure<UsersResponse>(
                    new Error("CreateUser.EnforceUniqueDocumentNumber", "The user document is already in use."));
            }

            var user = _context.Users.FirstOrDefault(user => user.Id.Equals(request.Id));
            if (user is null)
                return ServiceResponse.Failure<UsersResponse>(
                    new Error("UpdateUser.UserNotFound", validationResult.ToString()));


            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync(cancellationToken);

            return user.Adapt<UsersResponse>();
        }
    }
}

public class UpdateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/users/{id}", async (string id, [FromBody] UpdateUserRequest request, ISender sender) =>
        {
            var command = new UpdateUser.Command(id,
                                                 request.FirstName,
                                                 request.MiddleName,
                                                 request.LastName,
                                                 request.Email,
                                                 request.UserType,
                                                 request.Password,
                                                 request.Document,
                                                 request.Balance);

            var result = await sender.Send(command);

            return Results.NoContent();
        });
    }
}