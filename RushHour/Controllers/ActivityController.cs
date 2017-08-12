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

namespace RushHour.Controllers
{
    [CustomAuthorize(AccessRightsInput = "001")]
    public class ActivityController : Controller
    {
        #region Constructors and fields
        private ActivityService activityService;
        public ActivityController()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            this.activityService = new ActivityService(new ModelStateWrapper(this.ModelState), unitOfWork.ActivityRepository, unitOfWork);
        }
        #endregion

        public ActionResult Create()
        {
            ActivityViewModel createActivityViewModel = new ActivityViewModel();
            return View(createActivityViewModel);
        }

        [HttpPost]
        public ActionResult Create(ActivityViewModel createActivityViewModel)
        {
            if (activityService.PreValidate())
            {
                Activity activityInput = new Activity();
                activityInput.Name = createActivityViewModel.Name;
                activityInput.Duration = createActivityViewModel.Duration;
                activityInput.Price = createActivityViewModel.Price;
                if (activityService.Save(activityInput))
                {
                    TempData["Message"] = "Activity saved successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Ooops something went wrong";
                }
                return RedirectToAction("GetAll");
            }
            return View();
        }

        public ActionResult GetAll()
        {
            List<Activity> activitiesDb = activityService.GetAll();
            List<ActivityViewModel> activitiesViewModel = new List<ActivityViewModel>();
            foreach (Activity item in activitiesDb)
            {
                activitiesViewModel.Add(
                    new ActivityViewModel
                    {
                        Id = item.Id,
                        Duration = item.Duration,
                        Name = item.Name,
                        Price = item.Price
                    });
            }
            return View(activitiesViewModel);
        }

        public ActionResult Edit(int id)
        {
            Activity activityDb = activityService.GetAll(activity => activity.Id == id).FirstOrDefault();
            ActivityViewModel editActivityViewModel = new ActivityViewModel();
            editActivityViewModel.Id = activityDb.Id;
            editActivityViewModel.Name = activityDb.Name;
            editActivityViewModel.Price = activityDb.Price;
            editActivityViewModel.Duration = activityDb.Duration;
            return View(editActivityViewModel);
        }

        [HttpPost]
        public ActionResult Edit(ActivityViewModel editActivityViewModel)
        {
            if (activityService.PreValidate())
            {
                Activity activityInput = new Activity();
                activityInput.Id = editActivityViewModel.Id;
                activityInput.Name = editActivityViewModel.Name;
                activityInput.Duration = editActivityViewModel.Duration;
                activityInput.Price = editActivityViewModel.Price;
                if (activityService.Save(activityInput))
                {
                    TempData["Message"] = "Activity saved successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Ooops something went wrong";
                }
                return RedirectToAction("GetAll");
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            if (activityService.DeleteById(id))
            {
                TempData["Message"] = "Activity deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Ooops something went wrong";
            }
            return RedirectToAction("GetAll");
        }
    }
}