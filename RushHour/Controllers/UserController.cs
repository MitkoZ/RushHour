using DataAccess.Models;
using Repositories;
using RushHour.Helpers;
using RushHour.ViewModels;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RushHour.Controllers
{
    public class UserController : Controller
    {
        #region Constructors and fields
        private UserService userService;
        public UserController()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            this.userService = new UserService(new ModelStateWrapper(this.ModelState), unitOfWork.UserRepository, unitOfWork);
        }
        #endregion

        [CustomAuthorize(AccessRightsInput = "101")]
        public ActionResult Register()
        {
            return View();
        }

        [CustomAuthorize(AccessRightsInput = "101")]
        [HttpPost]
        public ActionResult Register(RegisterUserViewModel registerViewModel)
        {
            User userInput = new DataAccess.Models.User();
            userInput.Name = registerViewModel.Name;
            userInput.Password = registerViewModel.Password;
            userInput.IsAdmin = false;
            userInput.Email = registerViewModel.Email;
            userInput.Phone = registerViewModel.Phone;
            if (!userService.ValidateUser(userInput))
            {
                return View(registerViewModel);
            }

            if (userService.Save(userInput))
            {
                TempData["Message"] = "Registered successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Ooops something went wrong";
            }
            return RedirectToAction("Index", "Home");
        }

        [CustomAuthorize(AccessRightsInput = "011", Redirect = true)]
        public ActionResult Edit(int id)
        {
            User userDb = userService.GetAll(user => user.Id == id).FirstOrDefault();
            UserViewModel editViewModel = new UserViewModel();
            editViewModel.Id = userDb.Id;
            editViewModel.Name = userDb.Name;
            editViewModel.Password = userDb.Password;
            editViewModel.IsAdmin = userDb.IsAdmin;
            editViewModel.Phone = userDb.Phone;
            editViewModel.Email = userDb.Email;
            return View(editViewModel);
        }

        [CustomAuthorize(AccessRightsInput = "011")]
        [HttpPost]
        public ActionResult Edit(UserViewModel editViewModel)
        {
            if (userService.PreValidate())
            {
                User userInput = new DataAccess.Models.User();
                userInput.Id = editViewModel.Id;
                userInput.IsAdmin = editViewModel.IsAdmin;
                userInput.Name = editViewModel.Name;
                userInput.Phone = editViewModel.Phone;
                userInput.Email = editViewModel.Email;
                userInput.Password = editViewModel.Password;
                if (userService.Save(userInput))
                {
                    TempData["Message"] = "Edit made successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Ooops something went wrong";
                }
                return RedirectToAction("Index", "Home");
            }
            return View(editViewModel);

        }

        [CustomAuthorize(AccessRightsInput = "011")]
        public ActionResult ChangePassword()
        {
            ChangePasswordViewModel changePasswordViewModel = new ChangePasswordViewModel();
            return View(changePasswordViewModel);
        }

        [CustomAuthorize(AccessRightsInput = "011")]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            User userDb = userService.GetAll(user => user.Id == LoginUserSession.Current.UserId).FirstOrDefault();
            if (!userService.ChangePasswordValidation(userDb, changePasswordViewModel.OldPassword))
            {
                return View(changePasswordViewModel);
            }
            userDb.Password = changePasswordViewModel.NewPassword;
            if (userService.Save(userDb))
            {
                TempData["Message"] = "Password saved successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Ooops something went wrong";
            }
            return RedirectToAction("Index", "Home");
        }

        [CustomAuthorize(AccessRightsInput = "001")]
        public ActionResult GetAll()
        {
            List<UserViewModel> usersViewModel = new List<UserViewModel>();
            foreach (User userDb in userService.GetAll())
            {
                usersViewModel.Add(new UserViewModel { Id = userDb.Id, Email = userDb.Email, Password = userDb.Password, Name = userDb.Name, Phone = userDb.Phone, IsAdmin = userDb.IsAdmin });
            }
            return View(usersViewModel);
        }

        [CustomAuthorize(AccessRightsInput = "001")]
        public ActionResult Delete(int id)
        {
            if (userService.DeleteById(id))
            {
                TempData["Message"] = "User deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Ooops something went wrong";
            }
            return RedirectToAction("Index", "Home");
        }
    }
}