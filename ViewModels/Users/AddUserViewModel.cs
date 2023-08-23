﻿using System.ComponentModel.DataAnnotations;

namespace ImprovedPicpay.ViewModels.Users
{
    public class AddUserViewModel
    {
        [Required(ErrorMessage = "The user first name is required.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The user middle name is required.")]
        public string MiddleName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "The user last name is required.")]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "The user email is required.")]
        [EmailAddress(ErrorMessage = "This is not a valid email address")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "The user type is required.")]
        public string UserType { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "The user password is required.")]
        public string Password { get; set; } = string.Empty;
    }
}