using Repositories;
using RushHour.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Models;
using RushHour.Helpers;
using Services;

namespace RushHour.Controllers
{
    public class HomeController : Controller
    {
        #region Constructors and fields
        private UserService userService;
        private HomeService homeService;

        public HomeController()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            this.userService = new UserService(new ModelStateWrapper(this.ModelState), unitOfWork.UserRepository, unitOfWork);
            this.homeService = new HomeService(new ModelStateWrapper(this.ModelState), unitOfWork);
        }
        #endregion

        [CustomAuthorize(AccessRightsInput = "111")]
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize(AccessRightsInput = "100", Redirect = true)]
        public ActionResult Login()
        {
            return View();
        }

        [CustomAuthorize(AccessRightsInput = "100", Redirect = true)]
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (homeService.PreValidate())
            {
                User userDb = userService.GetAll(user => user.Email == loginViewModel.Email && user.Password == loginViewModel.Password).FirstOrDefault();

                if (!homeService.ValidateUserDb(userDb))
                {
                    ViewBag.ErrorMessage = "Invalid email and/or password!";
                    return View();
                }

                LoginUserSession.Current.SetCurrentUser(userDb.Id, userDb.Name, userDb.Email, userDb.IsAdmin);
            }
            ViewBag.Message = "Logged successfully!";
            return View("Index");
        }

        [CustomAuthorize(AccessRightsInput = "011", Redirect = true)]
        public ActionResult Logout()
        {
            LoginUserSession.Current.Logout();
            return View("Index");
        }

        [AllowAnonymous]
        public ActionResult ValidatePhone(string phone)
        {
            bool isPhoneUsed = false;
            if (!string.IsNullOrEmpty(phone))
            {
                User userDb = userService.GetAll(user => user.Phone == phone).FirstOrDefault();
                if (homeService.ValidateUserDb(userDb))
                {
                    isPhoneUsed = true;
                }
            }
            return Json(!isPhoneUsed, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult ValidateEmail(string email)
        {
            bool isEmailUsed = false;
            if (!string.IsNullOrEmpty(email))
            {
                User userDb = userService.GetAll(user => user.Email == email).FirstOrDefault();
                if (homeService.ValidateUserDb(userDb))
                {
                    isEmailUsed = true;
                }
            }
            return Json(!isEmailUsed, JsonRequestBehavior.AllowGet);
        }
    }
}