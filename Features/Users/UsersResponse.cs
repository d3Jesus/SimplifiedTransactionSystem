namespace ImprovedPicpay.Features.Users;

public static partial class GetUserById
{
    public record UsersResponse(string Id,
                                string FirstName,
                                string MiddleName,
                                string LastName,
                                string UserType,
                                decimal Balance,
                                DateTime RegisteredIn,
                                bool IsLocked);
}
