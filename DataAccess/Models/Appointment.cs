using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class Appointment : BaseEntity
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsCancelled { get; set; }
        public int UserId { get; set; }
        public virtual List<Activity> Activities { get; set; }
        public Appointment()
        {
            Activities = new List<Activity>();
        }
    }
}
