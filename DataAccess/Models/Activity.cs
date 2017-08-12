using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Models
{
    public class Activity : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public float Duration { get; set; }
        [Required]
        public decimal Price { get; set; }
        public virtual List<Appointment> Appointments { get; set; }
        public Activity()
        {
            Appointments = new List<Appointment>();
        }
    }
}
