using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private RushHourContext context = new RushHourContext();

        private ActivityRepository activityRepository;
        private AppointmentRepository appointmentRepository;
        private UserRepository userRepository;


        public ActivityRepository ActivityRepository
        {
            get
            {
                if (this.activityRepository == null)
                {
                    this.activityRepository = new ActivityRepository(context);
                }
                return activityRepository;
            }
        }

        public AppointmentRepository AppointmentRepository
        {

            get
            {
                if (this.appointmentRepository == null)
                {
                    this.appointmentRepository = new AppointmentRepository(context);
                }
                return appointmentRepository;
            }
        }

        public UserRepository UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new UserRepository(context);
                }
                return userRepository;
            }
        }


        public int Save()
        {
            return context.SaveChanges();
        }
    }
}
