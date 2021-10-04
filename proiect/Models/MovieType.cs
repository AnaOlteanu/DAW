using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proiect.Models
{
    public class MovieType
    {
        public int MovieTypeId { get; set; }
        [MinLength(2, ErrorMessage = "MovieType name cannot be less than 2!"),
            MaxLength(30, ErrorMessage = "MovieType name cannot be more than 30!")]
        public string Name { get; set; }

        //many to one movie type - movie
        public virtual ICollection<Movie> Movies { get; set; }

    }
}