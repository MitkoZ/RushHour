using DataAccess.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AppointmentService : BaseService<Appointment, AppointmentRepository, UnitOfWork>
    {
        public AppointmentService(IValidationDictionary validationDictionary, AppointmentRepository repository, UnitOfWork unitOfWork) : base(validationDictionary, repository, unitOfWork)
        {
        }

        public bool Validate(bool isCancelled, int appointmentId)
        {
            if (isCancelled && appointmentId != 0)
            {
                this.validationDictionary.AddError("Updating a cancelled appointment", "Cannot edit an cancelled appointment");
            }
            return this.validationDictionary.isValid;
        }

        public DateTime CalculateEndDateTime(DateTime StartDateTime, List<Activity> chosenActivities)
        {
            float duration = chosenActivities.Sum(x => x.Duration);
            TimeSpan span = TimeSpan.FromMinutes(duration);
            DateTime EndDateTime = StartDateTime + span;
            return EndDateTime;
        }
    }
}
