using DataAccess.Models;
using Repositories;
using RushHour.Helpers;
using RushHour.ViewModels;
using Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RushHour.Controllers
{
    [CustomAuthorize(AccessRightsInput = "011")]
    public class AppointmentController : Controller
    {
        #region Constructors and fields
        private AppointmentService appointmentService;
        private UserService userService;
        private ActivityService activityService;
        public AppointmentController()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            this.appointmentService = new AppointmentService(new ModelStateWrapper(this.ModelState), unitOfWork.AppointmentRepository, unitOfWork);
            this.userService = new UserService(new ModelStateWrapper(this.ModelState), unitOfWork.UserRepository, unitOfWork);
            this.activityService = new ActivityService(new ModelStateWrapper(this.ModelState), unitOfWork.ActivityRepository, unitOfWork);
        }
        #endregion

        public ActionResult Create()
        {
            AppointmentViewModel createAppointmentViewModel = new AppointmentViewModel();
            if (TempData["currentAppointment"] != null)
            {
                Appointment currentAppointment = (Appointment)TempData["currentAppointment"];
                createAppointmentViewModel.Id = currentAppointment.Id;
                createAppointmentViewModel.StartDateTime = Convert.ToString(currentAppointment.StartDateTime);
                createAppointmentViewModel.EndDateTime = Convert.ToString(currentAppointment.EndDateTime);
                createAppointmentViewModel.currentAppointmentActivities = currentAppointment.Activities;
                createAppointmentViewModel.UserId = currentAppointment.UserId;
                createAppointmentViewModel.IsCancelled = currentAppointment.IsCancelled;
            }
            return View(createAppointmentViewModel);
        }

        [HttpPost]
        public ActionResult Create(AppointmentViewModel createAppointmentViewModel)
        {
            if (createAppointmentViewModel.IsCancelled && createAppointmentViewModel.Id != 0)
            {
                ModelState.AddModelError("", "Cannot edit an cancelled appointment");
            }

            if (appointmentService.PreValidate())
            {
                Appointment appointmentInput = new Appointment();
                bool isEdit = false;
                if (createAppointmentViewModel.Id != 0)
                {
                    appointmentInput.Id = createAppointmentViewModel.Id;
                    appointmentInput = appointmentService.GetAll(x => x.Id == createAppointmentViewModel.Id).FirstOrDefault();
                    isEdit = true;
                }
                else
                {
                    appointmentInput.UserId = LoginUserSession.Current.UserId;
                }

                appointmentInput.StartDateTime = DateTime.ParseExact(createAppointmentViewModel.StartDateTime, "dd/MM/yyyy HH:mm", null);
                List<Activity> chosenActivities = new List<Activity>();
                foreach (int activityId in createAppointmentViewModel.chosenActivitiesIds)
                {
                    chosenActivities.Add(activityService.GetAll(x => x.Id == activityId).FirstOrDefault());
                }
                appointmentInput.EndDateTime = appointmentService.CalculateEndDateTime(appointmentInput.StartDateTime, chosenActivities);
                if (!appointmentService.TakenDateTimeCheck(appointmentInput.StartDateTime, appointmentInput.EndDateTime, isEdit, appointmentInput.UserId)) //if current time is taken
                {
                    return View(createAppointmentViewModel);
                }
                appointmentInput.Activities.Clear();
                appointmentInput.Activities.AddRange(chosenActivities);
                if (appointmentService.Save(appointmentInput))
                {
                    TempData["Message"] = "Appointment saved successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Ooops something went wrong";
                }
                return RedirectToAction("GetAll");
            }
            return View(createAppointmentViewModel);
        }

        public ActionResult GetAll()
        {
            Func<DataAccess.Models.Appointment, bool> filter;
            if (!LoginUserSession.Current.IsAdmin)
            {
                filter = delegate (DataAccess.Models.Appointment appointment)
                {
                    if (appointment.UserId == LoginUserSession.Current.UserId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                };
            }
            else
            {
                filter = null;
            }
            List<Appointment> appointmentsDb = new List<Appointment>();
            appointmentsDb = appointmentService.GetAll(filter);
            List<AppointmentViewModel> appointmetsViewModel = new List<AppointmentViewModel>();

            foreach (Appointment appointment in appointmentsDb)
            {
                appointmetsViewModel.Add(new AppointmentViewModel
                {
                    Id = appointment.Id,
                    UserId = appointment.UserId,
                    UserEmail = userService.GetAll(x => x.Id == appointment.UserId).FirstOrDefault().Email,
                    StartDateTime = Convert.ToString(appointment.StartDateTime),
                    EndDateTime = Convert.ToString(appointment.EndDateTime),
                    IsCancelled = appointment.IsCancelled
                });
            }

            return View(appointmetsViewModel);
        }

        public ActionResult Edit(int id)
        {
            Appointment currentAppointment = appointmentService.GetAll(x => x.Id == id).FirstOrDefault();
            TempData["currentAppointment"] = currentAppointment;
            return RedirectToAction("Create");
        }

        [CustomAuthorize(AccessRightsInput = "001")]
        public ActionResult Delete(int id)
        {
            if (appointmentService.DeleteById(id))
            {
                TempData["Message"] = "Appointment deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Ooops something went wrong";
            }
            return RedirectToAction("GetAll");
        }

        public ActionResult Cancel(int id)
        {
            Appointment appointmentDb = appointmentService.GetAll(x => x.Id == id).FirstOrDefault();
            appointmentDb.IsCancelled = true;
            if (appointmentService.Save(appointmentDb))
            {
                TempData["Message"] = "Appointment cancelled successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Ooops something went wrong";
            }
            return RedirectToAction("GetAll");
        }
    }
}