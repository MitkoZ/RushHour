using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Models
{
    public class User : BaseEntity
    {
        [Required]
        [Index(IsUnique = true)]
        [StringLength(200)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool IsAdmin { get; set; }
        public virtual List<Appointment> Appointments { get; set; }
        public User()
        {
            Appointments = new List<Appointment>();
        }
    }
}
