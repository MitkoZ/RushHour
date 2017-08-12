using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace DataAccess.Models
{
    public class RushHourContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public RushHourContext()
        {
           
        }
    }
}
