using proiect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiect.Controllers
{
    public class ContactInfoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ContactInfo
        public ActionResult Index()
        {
            List<ContactInfo> contactInfos = db.ContactInfos.ToList();
            ViewBag.ContactInfos = contactInfos;
            return View();
        }

        [Authorize(Roles = "Admin, Actor")]
        public ActionResult New()
        {
            ContactInfo contact = new ContactInfo();
            ViewBag.GenderList = GetAllGenderTypes();
            return View();
        }

        [Authorize(Roles = "Admin, Actor")]
        [HttpPost]
        public ActionResult New(ContactInfo contactRequest)
        {

            ViewBag.GenderList = GetAllGenderTypes();
            try
            {
                if (ModelState.IsValid)
                {
                    db.ContactInfos.Add(contactRequest);
                    db.SaveChanges();
                    return RedirectToAction("Index", "ContactInfo");
                }
                return View(contactRequest);
            }
            catch (Exception e)
            {
                return View(contactRequest);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                ContactInfo contactInfo = db.ContactInfos.Find(id);
                if (contactInfo != null)
                {
                    var list = db.Actors.Where(t => t.ContactInfo.ContactInfoId == contactInfo.ContactInfoId);
                    foreach (var ib in list)
                    {
                        db.Actors.Remove(ib);
                    }
                    db.ContactInfos.Remove(contactInfo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return HttpNotFound("Couldn't find the movie type with id " + id.ToString() + "!");
            }
            return HttpNotFound("Movie type id parameter is missing!");
        }

        [Authorize(Roles = "Admin, Actor")]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                ContactInfo contactInfo = db.ContactInfos.Find(id);
                if (contactInfo == null)
                {
                    return HttpNotFound("Couldn't find the contact with id " + id.ToString() + "!");
                }
                return View(contactInfo);
            }
            return HttpNotFound("Couldn't find the contact with id " + id.ToString() + "!");
        }

        [Authorize(Roles = "Admin, Actor")]
        [HttpPut]
        public ActionResult Edit(int id, ContactInfo contactInfoRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContactInfo contactInfo = db.ContactInfos.Find(id);
                    if (TryUpdateModel(contactInfo))
                    {
                        contactInfo.PhoneNumber = contactInfoRequest.PhoneNumber;
                        contactInfo.BirthYear = contactInfoRequest.BirthYear;
                        contactInfo.BirthMonth = contactInfoRequest.BirthMonth;
                        contactInfo.BirthDay = contactInfoRequest.BirthDay;
                        contactInfo.Email = contactInfoRequest.Email;
                        contactInfo.GenderType = contactInfoRequest.GenderType;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index","ContactInfo");
                }
                return View(contactInfoRequest);
            }
            catch (Exception e)
            {
                return View(contactInfoRequest);
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllGenderTypes()
        {
            var selectList = new List<SelectListItem>();
            selectList.Add(new SelectListItem
            {
                Value = Gender.Male.ToString(),
                Text = "Male"
            });
            selectList.Add(new SelectListItem
            {
                Value = Gender.Female.ToString(),
                Text = "Female"
            });
            return selectList;
        }


    }
}