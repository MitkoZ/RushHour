using DataAccess.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : BaseService<User, UserRepository, UnitOfWork>
    {
        public UserService(IValidationDictionary validationDictionary, UserRepository repository, UnitOfWork unitOfWork) : base(validationDictionary, repository, unitOfWork)
        {
        }

        public bool ValidateUser(User userInput)
        {
            PreValidate();
            User dbUser = new User();
            dbUser = unitOfWork.UserRepository.GetAll(user => user.Phone == userInput.Phone || user.Email == userInput.Email).FirstOrDefault();

            if (dbUser != null)
            {
                if (dbUser.Phone == userInput.Phone)
                {
                    this.validationDictionary.AddError("", "A user with this phone already exists!");
                }

                if (dbUser.Email == userInput.Email)
                {
                    this.validationDictionary.AddError("", "A user with this email already exists!");
                }
            }
            return this.validationDictionary.isValid;
        }

        public bool ChangePasswordValidation(User userDb, string oldPassword)
        {
            PreValidate();
            if (userDb.Password != oldPassword)
            {
                this.validationDictionary.AddError("", "Invalid old password");
            }
            return this.validationDictionary.isValid;
        }

    }
}
