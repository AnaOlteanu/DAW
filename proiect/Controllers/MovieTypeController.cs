using proiect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiect.Controllers
{
    public class MovieTypeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MovieType
        public ActionResult Index()
        {
            ViewBag.MovieTypes = db.MovieTypes.ToList();
            return View();
        }

        [Authorize]
        public ActionResult New()
        {
            MovieType movieType = new MovieType();
            return View(movieType);
        }

        [Authorize]
        [HttpPost]
        public ActionResult New(MovieType movieTypeRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.MovieTypes.Add(movieTypeRequest);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(movieTypeRequest);
            }
            catch (Exception e)
            {
                return View(movieTypeRequest);
            }
        }

        [Authorize(Roles = "Admin, Actor")]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                MovieType movieType = db.MovieTypes.Find(id);
                if (movieType == null)
                {
                    return HttpNotFound("Couldn't find the movie type with id " + id.ToString() + "!");
                }
                return View(movieType);
            }
            return HttpNotFound("Couldn't find the book movie with id " + id.ToString() + "!");
        }

        [Authorize(Roles = "Admin, Actor")]
        [HttpPut]
        public ActionResult Edit(int id, MovieType movieTypeRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MovieType movieType = db.MovieTypes.Find(id);
                    if (TryUpdateModel(movieType))
                    {
                        movieType.Name = movieTypeRequest.Name;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(movieTypeRequest);
            }
            catch (Exception e)
            {
                return View(movieTypeRequest);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                MovieType movieType = db.MovieTypes.Find(id);
                if (movieType != null)
                {
                    db.MovieTypes.Remove(movieType);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return HttpNotFound("Couldn't find the movie type with id " + id.ToString() + "!");
            }
            return HttpNotFound("Movie type id parameter is missing!");
        }



    }
}