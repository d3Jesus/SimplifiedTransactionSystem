using ImprovedPicpay.Models;
using ImprovedPicpay.ViewModels.Users;

namespace ImprovedPicpay.Mappers;

public static class UserMapper
{
    /// <summary>
    /// Maps from a list of User object to list of GetUsersViewModel object
    /// </summary>
    /// <param name="users">List of User object</param>
    /// <returns>Mapped list to GetUsersViewModel</returns>
    public static List<GetUsersViewModel> MapToViewModel(IEnumerable<User> users)
    {
        return users.Select(us => new GetUsersViewModel
        {
            id = us.Id,
            firstName = us.UserFirstName,
            middleName = us.UserMiddleName,
            lastName = us.UserLastName,
            registeredIn = us.DateOfRegistration,
            isLocked = us.IsLocked
        }).ToList();
    }

    /// <summary>
    /// Maps from a User object to GetUsersViewModel object
    /// </summary>
    /// <param name="user">User object</param>
    /// <returns>Mapped object to GetUsersViewModel</returns>
    public static GetUsersViewModel MapToViewModel(User user)
    {
        return new GetUsersViewModel
        {
            id = user.Id,
            firstName = user.UserFirstName,
            middleName = user.UserMiddleName,
            lastName = user.UserLastName,
            registeredIn = user.DateOfRegistration,
            isLocked = user.IsLocked
        };
    }

    /// <summary>
    /// Maps from a view model object to User object
    /// </summary>
    /// <param name="viewModel">View Model object</param>
    /// <returns>Mapped object to User/returns>

    public static User MapToUser(AddUserViewModel viewModel)
    {
        return new User
        {
            UserFirstName = viewModel.FirstName,
            UserMiddleName = viewModel.MiddleName,
            UserLastName = viewModel.LastName,
            Email = viewModel.Email,
            UserType = viewModel.UserType,
            Password = viewModel.Password
        };
    }

    /// <summary>
    /// Maps from a view model object to User object
    /// </summary>
    /// <param name="viewModel">View Model object</param>
    /// <returns>Mapped object to User/returns>

    public static User MapToUser(UpdateUserViewModel viewModel)
    {
        return new User
        {
            Id = viewModel.Id,
            UserFirstName = viewModel.FirstName,
            UserMiddleName = viewModel.MiddleName,
            UserLastName = viewModel.LastName,
            Email = viewModel.Email,
            UserType = viewModel.UserType,
            Password = viewModel.Password
        };
    }

    /// <summary>
    /// Maps from a view model object to User object
    /// </summary>
    /// <param name="viewModel">View Model object</param>
    /// <returns>Mapped object to User/returns>
    public static User MapFromUserToUser(this User existingUser, User user)
    {
        existingUser.UserFirstName = user.UserFirstName;
        existingUser.UserMiddleName = user.UserMiddleName;
        existingUser.UserLastName = user.UserLastName;
        existingUser.Email = user.Email;
        existingUser.UserType = user.UserType;
        existingUser.Password = user.Password;

        return existingUser;
    }
}
