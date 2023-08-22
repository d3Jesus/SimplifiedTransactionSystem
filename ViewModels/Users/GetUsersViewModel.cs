namespace ImprovedPicpay.ViewModels.Users;

public record  struct GetUsersViewModel(string firstName,
                                        string middleName,
                                        string lastName,
                                        DateTime registeredIn,
                                        bool isLocked);
