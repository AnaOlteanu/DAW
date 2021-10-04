using proiect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiect.Controllers
{
    public class ActorController : Controller
    {
        // GET: Actor
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            List<Actor> actors = db.Actors.ToList();
            ViewBag.Actors = actors;
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Actor actor = db.Actors.Find(id);
                if (actor != null)
                {
                    return View(actor);
                }
                return HttpNotFound("Couldn't find the actor with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing actor id parameter!");
        }

        [Authorize]
        [HttpGet]
        public ActionResult New()
        {
            Actor actor = new Actor();
            actor.Movies = new List<Movie>();
            return View(actor);
        }

        [Authorize]
        [HttpPost]
        public ActionResult New(Actor actorRequest)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    ContactInfo contactinfonew = new ContactInfo
                    {
                        PhoneNumber = actorRequest.ContactInfo.PhoneNumber
                    };
               
                    db.Actors.Add(actorRequest);
                    db.SaveChanges();
                    return RedirectToAction("Index","Actor");
                }
                return View(actorRequest);
            }
            catch (Exception e)
            {
                return View(actorRequest);
            }
        }

        [Authorize(Roles = "Admin, Actor")]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Actor actor = db.Actors.Find(id);
            
                if (actor == null)
                {
                    return HttpNotFound("Coludn't find the actor with id " + id.ToString() + "!");
                }
                return View(actor);
            }
            return HttpNotFound("Missing actor id parameter!");
        }

        [Authorize(Roles = "Admin, Actor")]
        [HttpPut]
        public ActionResult Edit(int id, Actor actorRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Actor actor = db.Actors.Find(id);
                    ContactInfo contact = db.ContactInfos.Find(actor.ContactInfo.ContactInfoId);

                    if (TryUpdateModel(actor))
                    {
                        actor.Name = actorRequest.Name;
                        actor.ContactInfo.PhoneNumber = actorRequest.ContactInfo.PhoneNumber;
                        contact.PhoneNumber = actorRequest.ContactInfo.PhoneNumber;
                       
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(actorRequest);
            }
            catch (Exception)
            {
                return View(actorRequest);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Actor actor = db.Actors.Find(id);
            ContactInfo contact = db.ContactInfos.Find(actor.ContactInfo.ContactInfoId);

            if (actor != null)
            {
                db.Actors.Remove(actor);
                db.ContactInfos.Remove(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the actor with id " + id.ToString() + "!");
        }


    }
}