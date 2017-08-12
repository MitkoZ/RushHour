using DataAccess.Models;
using Repositories;
using RushHour.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RushHour.ViewModels
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Start Date Time")]
        public string StartDateTime { get; set; }
        [Display(Name = "End Date Time")]
        public string EndDateTime { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public bool IsCancelled { get; set; }
        public List<Activity> Activities { get; set; }
        [CustomChosenActivitiesAttribute]
        public List<int> chosenActivitiesIds { get; set; }
        public List<Activity> currentAppointmentActivities { get; set; }
        public AppointmentViewModel()
        {
            this.chosenActivitiesIds = new List<int>();
            this.Activities = new UnitOfWork().ActivityRepository.GetAll();
            this.currentAppointmentActivities = new List<Activity>();
        }
    }
}