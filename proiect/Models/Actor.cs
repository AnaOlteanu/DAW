using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proiect.Models
{
    public class Actor
    {
        public int ActorId { get; set;}
        [MinLength(2, ErrorMessage = "Actor name cannot be less than 2!"),
            MaxLength(30, ErrorMessage = "Actor name cannot be more than 30!")]
        public string Name { get; set; }

        //many to many actor-movie
        public virtual ICollection<Movie> Movies { get; set; }
        
        //one to one actor-contact info
        [Required]
        public virtual ContactInfo ContactInfo { get; set; }

    }
}