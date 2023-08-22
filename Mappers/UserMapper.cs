using ImprovedPicpay.Models;
using ImprovedPicpay.ViewModels.Users;

namespace ImprovedPicpay.Mappers;

public static class UserMapper
{
    public static List<GetUsersViewModel> MapToViewModel(IEnumerable<User> users)
    {
        return users.Select(us => new GetUsersViewModel
        {
            firstName = us.UserFirstName,
            middleName = us.UserMiddleName,
            lastName = us.UserLastName,
            registeredIn = us.DateOfRegistration,
            isLocked = us.IsLocked
        }).ToList();
    }
}
