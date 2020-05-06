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
            User normalUser = new User { Email = "pesho@abv.bg", Name = "pesho", Password = "123456", Phone = "08888888888888", IsAdmin = false };
            context.Users.AddOrUpdate(x => x.Email, normalUser);

            List<Activity> activities = new List<Activity>
            {
                new Activity
                {
                    Name = "Hair painting",
                    Duration = 25,
                    Price = 50
                },
                new Activity
                {
                    Name = "Haircut",
                    Duration = 20,
                    Price = 10
                },
                new Activity
                {
                    Name = "Nail Polishing",
                    Duration = 75,
                    Price = 13.35m
                },
                new Activity
                {
                    Name = "Face Lifting",
                    Duration = 120,
                    Price = 300.21m
                },
                new Activity
                {
                    Name = "Depilation",
                    Duration = 30,
                    Price = 300.82m
                }
            };

            context.Activities.AddOrUpdate(x => x.Name, activities.ToArray());

            Appointment appointment = new Appointment { StartDateTime = new DateTime(2017, 07, 28, 16, 25, 0), EndDateTime = new DateTime(2017, 07, 28, 16, 50, 0) };
            normalUser.Appointments.Add(appointment);

            if (!appointment.Activities.Any(x => x.Name == activities[0].Name))
            {
                appointment.Activities.Add(activities[0]);
            }
            context.SaveChanges();
        }
    }
}
