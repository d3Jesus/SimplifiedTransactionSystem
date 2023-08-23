using ImprovedPicpay.Enums;

namespace ImprovedPicpay.Models
{
    public class User
    {
        /// <summary>
        /// User unique ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User's first name.
        /// </summary>
        public string UserFirstName { get; set; } = string.Empty;

        /// <summary>
        /// User's middle name.
        /// </summary>
        public string UserMiddleName { get; set; } = string.Empty;

        /// <summary>
        /// User's last name.
        /// </summary>
        public string UserLastName { get; set; } = string.Empty;

        /// <summary>
        /// User's email address that will be used to log in.
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// User's password that will be used to log in.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Date and time when the user was registered to the application.
        /// </summary>
        public DateTime DateOfRegistration { get; init; } = DateTime.Now;

        /// <summary>
        /// Specifies if the users is locked or not.
        /// </summary>
        public bool IsLocked { get; set; } = false;

        /// <summary>
        /// Specifies the type of user.
        /// </summary>
        public string UserType { get; set; } = UserTypes.Common.ToString();

        /// <summary>
        /// User document number
        /// </summary>
        public string Document { get; set; }
    }
}
