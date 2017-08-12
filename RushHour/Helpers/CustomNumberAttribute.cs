using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace RushHour.Helpers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomNumberAttribute : RequiredAttribute
    {
        public CustomNumberAttribute()
        {
            ErrorMessage = "Please enter a valid number";
        }

        public override bool IsValid(object value)
        {
            string input = value.ToString();
            decimal number;
            bool result = decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out number);
            if (!(number > 0))
            {
                result = false;
            }
            return result;
        }
    }
}