using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RushHour.ViewModels
{
    public class RegisterUserViewModel
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(25, ErrorMessage = "Email can be max 25 symbols")]
        [Remote("ValidateEmail", "Home", ErrorMessage = "This email is already used.")]
        public string Email { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 symbols")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 symbols")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "Name must be at least 4 symbols")]
        public string Name { get; set; }
        [Phone]
        [Remote("ValidatePhone", "Home", ErrorMessage = "This phone is already used.")]
        [MinLength(5, ErrorMessage = "Phone number must be at least 5 symbols")]
        public string Phone { get; set; }
    }
}