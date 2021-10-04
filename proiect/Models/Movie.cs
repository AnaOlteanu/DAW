using proiect.Models.MyValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiect.Models
{
    public class Movie
    {
        public int MovieId { get; set; }

        [MinLength(2, ErrorMessage = "Movie title cannot be less than 2!"),
            MaxLength(100, ErrorMessage = "Movie title cannot be more than 100!")]
        public string Title { get; set; }

        [MinLength(2, ErrorMessage = "Description cannot be less than 2!"),
            MaxLength(6000, ErrorMessage = "Description cannot be more than 6000!")]
        public string Description { get; set; }

        [RegularExpression(@"^([0]?[1-9]|[1|2][0-9]|[3][0|1])[./-]([0]?[1-9]|[1][0-2])[./-]([0-9]{4}|[0-9]{2})$",
            ErrorMessage = "This is not a valid release date!Try using . or / or -")]
        public string ReleaseDate { get; set; }

        [DurationValidator]
        public int Duration { get; set; }

        //many to many movie-actor
        public virtual ICollection<Actor> Actors { get; set; }

        //many to many movie-genre
        public virtual ICollection<Genre> Genres { get; set; }

        //many to one movie-movie types
        [ForeignKey("MovieType")]
        public int MovieTypeId { get; set; }
        public virtual MovieType MovieType { get; set; }

        //dropdown lists

        [NotMapped]
        public IEnumerable<SelectListItem> MovieTypesList { get; set; }
        
        //checkboxex list
        [NotMapped]
        public List<CheckBoxViewModel> GenresList { get; set; }

        [NotMapped]
        public List<ActorCheckBoxViewModel> ActorsList { get; set; }


    }

 

}