using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RushHour.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(25, ErrorMessage = "Email can be max 25 symbols")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "Name must be at least 4 symbols")]
        public string Name { get; set; }
        [Phone]
        [MinLength(5, ErrorMessage = "Phone number must be at least 5 symbols")]
        public string Phone { get; set; }
        public bool IsAdmin { get; set; }
    }
}