namespace ImprovedPicpay.ViewModels.Users;

public record  struct GetUsersViewModel(string id,
                                        string firstName,
                                        string middleName,
                                        string lastName,
                                        string userType,
                                        decimal balance,
                                        DateTime registeredIn,
                                        bool isLocked);
