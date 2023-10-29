namespace ImprovedPicpay.Features.Users;

public record UpdateUserRequest(string FirstName,
                                string MiddleName,
                                string LastName,
                                string Email,
                                string UserType,
                                string Password,
                                string Document,
                                decimal Balance);