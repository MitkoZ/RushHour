using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RushHour.Helpers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomChosenActivitiesAttribute : RequiredAttribute
    {
        public CustomChosenActivitiesAttribute()
        {
            ErrorMessage = "Please check at least 1 activity!";
        }

        public override bool IsValid(object value)
        {
            List<int> chosenActivities = (List<int>)value;
            bool result = true;
            if (chosenActivities.Count == 0)
            {
                result = false;
            }
            return result;
        }
    }
}