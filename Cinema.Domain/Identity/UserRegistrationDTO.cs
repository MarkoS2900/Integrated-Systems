using Cinema.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.Domain.Identity
{
    public class UserRegistrationDTO
    {
        [Required(ErrorMessage = "Name is required!")]
        [StringLength(100)]
        public string name { get; set; }
        [Required(ErrorMessage = "Last Name is required!")]
        [StringLength(100)]
        public string lastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address, please try again!")]
        [Required(ErrorMessage = "Email is required!")]
        public string email { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        public string password { get; set; }

        [Required(ErrorMessage = "You must confirm password!")]
        [Compare("Password", ErrorMessage = "The Password and Confirm Password do not match.")]
        public string confirmPassword { get; set; }
        public Roles userRole { get; set; }
    }
}
