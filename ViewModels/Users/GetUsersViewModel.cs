namespace ImprovedPicpay.ViewModels.Users;

public record  struct GetUsersViewModel(string id,
                                        string firstName,
                                        string middleName,
                                        string lastName,
                                        DateTime registeredIn,
                                        bool isLocked);
