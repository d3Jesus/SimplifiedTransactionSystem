namespace ImprovedPicpay.Features.Users;

public record UsersResponse(string Id,
                                string FirstName,
                                string MiddleName,
                                string LastName,
                                string UserType,
                                decimal Balance,
                                DateTime RegisteredIn,
                                bool IsLocked);
