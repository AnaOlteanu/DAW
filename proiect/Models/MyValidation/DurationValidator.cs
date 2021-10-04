using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proiect.Models.MyValidation
{
    public class DurationValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var movie = (Movie)validationContext.ObjectInstance;
            int duration = movie.Duration;
            bool cond = true;
            if (duration % 5 != 0 || duration < 20)
                cond = false;


            return cond ? ValidationResult.Success : new ValidationResult("This is not a valid duration!It must be over 20, multiple of 5");
        }
    }
}