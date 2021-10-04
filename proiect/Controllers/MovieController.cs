using proiect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiect.Controllers
{
    public class MovieController : Controller
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        // GET: Movie
        public ActionResult Index()
        {
            List<Movie> movies = ctx.Movies.ToList();
            ViewBag.Movies = movies;
            return View();
        }

 
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Movie movie = ctx.Movies.Find(id);
                if (movie != null)
                {
                    return View(movie);
                }
                return HttpNotFound("Couldn't find the movie with id " + id.ToString() + "!");

            }
            return HttpNotFound("Missing movie id parameter!");
        }

        [Authorize]
        [HttpGet]
        public ActionResult New()
        {
            Movie movie = new Movie();
            movie.Actors = new List<Actor>();
            movie.ActorsList = GetAllActors();
            movie.MovieTypesList = GetAllMovieTypes();
            movie.GenresList = GetAllGenres();
            movie.Genres = new List<Genre>();
            return View(movie);
        }

        [Authorize]
        [HttpPost]
        public ActionResult New(Movie movieRequest)
        {
            movieRequest.MovieTypesList = GetAllMovieTypes();

            //memoram intr-o lista doar genurile care au fost selectate
            var selectedGenres = movieRequest.GenresList.Where(b => b.Checked).ToList();
            var selectedActors = movieRequest.ActorsList.Where(b => b.Checked).ToList();

            try
            {
        
                if (ModelState.IsValid)
                {
                    movieRequest.Genres = new List<Genre>();
                    movieRequest.Actors = new List<Actor>();
                    for(int i = 0; i < selectedGenres.Count(); i++)
                    {
                        //filmului pe care vrem sa il adaugam ii asignam genurile selectate
                        Genre genre = ctx.Genres.Find(selectedGenres[i].Id);
                        movieRequest.Genres.Add(genre);
                    }
                    for (int i = 0; i < selectedActors.Count(); i++)
                    {
                        //filmului pe care vrem sa il adaugam ii asignam actorii selectati
                        Actor actor = ctx.Actors.Find(selectedActors[i].Id);
                        movieRequest.Actors.Add(actor);
                    }
                    ctx.Movies.Add(movieRequest);
                    ctx.SaveChanges();
                  
                    return RedirectToAction("Index");
                }
                return View(movieRequest);
            }
            catch(Exception e)
            {
                var msg = e.Message;
                return View(movieRequest);
            }
        }

        [Authorize(Roles = "Admin, Actor")]
        [HttpGet]
        public ActionResult Edit(int? id)
        {

            if (id.HasValue)
            {

                Movie movie = ctx.Movies.Find(id);
                movie.MovieTypesList = GetAllMovieTypes();
                movie.GenresList = GetAllGenres();
                movie.ActorsList = GetAllActors();

                foreach(Genre checkedGenre in movie.Genres)
                {
                    movie.GenresList.FirstOrDefault(g => g.Id == checkedGenre.GenreId).Checked = true;
                }

                foreach (Actor checkedActor in movie.Actors)
                {
                    movie.ActorsList.FirstOrDefault(g => g.Id == checkedActor.ActorId).Checked = true;
                }

                if (movie == null)
                {
                    return HttpNotFound("Couldn't find the movie with id " + id.ToString());
                }
                
                return View(movie);
            }
            return HttpNotFound("Missing movie id parameter!");
        }

        [Authorize(Roles = "Admin, Actor")]
        [HttpPut]
        public ActionResult Edit(int id, Movie movieRequest)
        {
            movieRequest.MovieTypesList = GetAllMovieTypes();
            Movie movie = ctx.Movies.Include("MovieType")
                        .SingleOrDefault(m => m.MovieId.Equals(id));
            var selectedGenres = movieRequest.GenresList.Where(b => b.Checked).ToList();
            var selectedActors = movieRequest.ActorsList.Where(b => b.Checked).ToList();
            try
            {
                
                if (ModelState.IsValid)
                {
                    
                    if (TryUpdateModel(movie))
                    {
                        movie.Title = movieRequest.Title;
                        movie.Description = movieRequest.Description;
                        movie.Duration = movieRequest.Duration;
                        movie.ReleaseDate = movieRequest.ReleaseDate;
                        movie.MovieTypeId = movieRequest.MovieTypeId;

                        movie.Genres.Clear();
                        movie.Genres = new List<Genre>();

                        movie.Actors.Clear();
                        movie.Actors = new List<Actor>();

                        for (int i = 0; i < selectedGenres.Count(); i++)
                        {
                            // filmului pe care vrem sa o editam ii asignam genurile selectate 
                            Genre genre = ctx.Genres.Find(selectedGenres[i].Id);
                            movie.Genres.Add(genre);
                        }

                        for (int i = 0; i < selectedActors.Count(); i++)
                        {
                            // filmului pe care vrem sa o editam ii asignam actorii selectati 
                            Actor actor = ctx.Actors.Find(selectedActors[i].Id);
                            movie.Actors.Add(actor);
                        }

                        ctx.SaveChanges();
                    }
                    return RedirectToAction("Index");

                }
                return View(movieRequest);
            }
            catch (Exception e)
            {
                return View(movieRequest);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Movie movie = ctx.Movies.Find(id);
            if (movie != null)
            {
                ctx.Movies.Remove(movie);
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the movie with id " + id.ToString());
        }

        [NonAction]
        public List<CheckBoxViewModel> GetAllGenres()
        {
            var checkboxList = new List<CheckBoxViewModel>();
            foreach (var genre in ctx.Genres.ToList())
            {
                checkboxList.Add(new CheckBoxViewModel
                {
                    Id = genre.GenreId,
                    Name = genre.Name,
                    Checked = false
                });
            }
            return checkboxList;
        }

        [NonAction]
        public List<ActorCheckBoxViewModel> GetAllActors()
        {
            var checkboxList = new List<ActorCheckBoxViewModel>();
            foreach (var actor in ctx.Actors.ToList())
            {
                checkboxList.Add(new ActorCheckBoxViewModel
                {
                    Id = actor.ActorId,
                    Name = actor.Name,
                    Checked = false
                });
            }
            return checkboxList;
        }

        [NonAction] // specificam faptul ca nu este o actiune
        public IEnumerable<SelectListItem> GetAllMovieTypes()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            foreach (var type in ctx.MovieTypes.ToList())
            {
                // adaugam in lista elementele necesare pt dropdown
                selectList.Add(new SelectListItem
                {
                    Value = type.MovieTypeId.ToString(),
                    Text = type.Name
                });
            }
            // returnam lista pentru dropdown
            return selectList;
        }


    }
}