namespace DataAccess.Migrations
{
    using DataAccess.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataAccess.Models.RushHourContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "DataAccess.Models.RushHourContext";
        }

        protected override void Seed(DataAccess.Models.RushHourContext context)
        {
            context.Users.AddOrUpdate(x => x.Email, new User { Email = "admin@abv.bg", Name = "admin", Password = "adminpass", Phone = "099999999999999", IsAdmin = true });
            User user = new User { Email = "pesho@abv.bg", Name = "pesho", Password = "123456", Phone = "08888888888888", IsAdmin = false };
            Appointment appointment = new Appointment { StartDateTime = new DateTime(2017, 07, 28, 16, 25, 0), EndDateTime = new DateTime(2017, 07, 28, 16, 50, 0)};
            user.Appointments.Add(appointment);
            context.Users.AddOrUpdate(x => x.Email, user);
            if (!appointment.Activities.Any(x => x.Name == "Hair painting"))
            {
                appointment.Activities.Add(new Activity { Name = "Hair painting", Duration = 25, Price = 50 });
            }
            context.SaveChanges();
        }
    }
}
