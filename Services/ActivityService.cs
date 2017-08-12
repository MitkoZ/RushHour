using DataAccess.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ActivityService : BaseService<Activity, ActivityRepository, UnitOfWork>
    {
        public ActivityService(IValidationDictionary validationDictionary, ActivityRepository repository, UnitOfWork unitOfWork) : base(validationDictionary, repository, unitOfWork)
        {
        }
    }
}
