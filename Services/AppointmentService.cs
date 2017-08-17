using DataAccess.Models;
using Repositories;
using RushHour.Helpers;
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

        public bool TakenDateTimeCheck(DateTime StartDateTime, DateTime EndDateTime, bool isEdit, int userId)
        {
            Appointment appointmentDb = repository.GetAll(x => !x.IsCancelled && (x.StartDateTime <= EndDateTime) && (StartDateTime <= x.EndDateTime)).FirstOrDefault();
            if (appointmentDb != null) //creating or editing an appointment
            {
                if (isEdit && appointmentDb.UserId == userId) //editing an already created appointment by the current user
                {
                    List<Appointment> appointmentsDb = repository.GetAll(x => !x.IsCancelled && (x.StartDateTime <= EndDateTime) && (StartDateTime <= x.EndDateTime));
                    if (appointmentsDb.Count > 1) //it means that another appointment exists at that time (not just the current user's one)
                    {
                        this.validationDictionary.AddError("DateTimeTaken error", "Start date time or End date time overlaps with another appointment");
                    }
                }
                else //creating an appointment case
                {
                    this.validationDictionary.AddError("DateTimeTaken error", "Start date time or End date time overlaps with another appointment");
                }
            }
            return this.validationDictionary.isValid;
        }
    }
}
