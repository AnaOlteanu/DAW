using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiect.Models
{
    public class ContactInfo
    {
        [Key]
        public int ContactInfoId { get; set; }

        [RegularExpression(@"^07(\d{8})$", 
            ErrorMessage = "This is not a valid phone number!")]
        public string PhoneNumber { get; set; }

       [RegularExpression(@"^(0|([1-9](\d{3})))$", 
            ErrorMessage = "This is not a valid year!")]
        public int BirthYear { get; set; }

        [RegularExpression(@"^(0|([1-9])|(1[012]))$", 
            ErrorMessage = "This is not a valid month!")]
        public int BirthMonth { get; set; }

        [RegularExpression(@"^(0|([1-9])|([12]\d)|(3[01]))$", 
            ErrorMessage = "This is not a valid day number!")]
        public int BirthDay { get; set; }

        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            ErrorMessage = "This is not a valid email address!")]
        public string Email { get; set; }

        
        [RegularExpression(@"(^Male|^Female)$",
            ErrorMessage = "This is not a valid gender type!")]
        public Gender GenderType { get; set; }

        //one-to-one relationship 
        public virtual Actor Actors { get; set; }
    }
    public enum Gender
    {
        Male,
        Female
    }
}