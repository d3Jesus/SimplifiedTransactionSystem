using Carter;
using FluentValidation;
using ImprovedPicpay.Data;
using ImprovedPicpay.Helpers;
using ImprovedPicpay.Entities;
using Mapster;
using MediatR;

namespace ImprovedPicpay.Features.Users;

public static class CreateUser
{
    public record Command(string FirstName,
                          string MiddleName,
                          string LastName,
                          string Email,
                          string UserType,
                          string Password,
                          string Document,
                          decimal Balance) : IRequest<ServiceResponse<string>>;

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

    internal sealed class Handler : IRequestHandler<Command, ServiceResponse<string>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<Command> _validator;

        public Handler(ApplicationDbContext context, IValidator<Command> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<ServiceResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return ServiceResponse.Failure<string>(
                    new Error("CreateUser.Validation", validationResult.ToString()));
            }

            var user = request.Adapt<User>();
            user.Id = Guid.NewGuid().ToString();
            _context.Add(user);

            await _context.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}

public class CreateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("users", async (CreateUser.Command command, ISender sender) =>
        {
            var result = await sender.Send(command);

            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Data);
        });
    }
}