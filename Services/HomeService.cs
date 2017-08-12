using DataAccess.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class HomeService
    {
        #region Constructors and fields
        private IValidationDictionary validationDictionary;
        private UnitOfWork unitOfWork;
        public HomeService(IValidationDictionary validationDictionary, UnitOfWork unitOfWork)
        {
            this.validationDictionary = validationDictionary;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        public bool PreValidate()
        {
            if (!validationDictionary.isValid)
            {
                return false;
            }
            return true;
        }

        public bool ValidateUserDb(User userDb)
        {
            if (userDb != null)
            {
                return true;
            }
            return false;
        }

    }
}
