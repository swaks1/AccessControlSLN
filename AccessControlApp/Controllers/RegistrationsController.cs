using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AccessControlApp.DataAccessLayer;
using AccessControlApp.Models.Entities;

namespace AccessControlApp.Controllers
{
    public class RegistrationsController : Controller
    {
        private AccessControlContext db = new AccessControlContext();

        public ActionResult Index()
        {
            var registrations = db.Registrations.Include(r => r.Device).Include(r => r.PointOfAccess);
            return View(registrations.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        public ActionResult Create(int? PersonId)
        {
            if (PersonId == null)
            {
                ViewBag.DeviceID = new SelectList(db.Devices, "ID", "TypeCode");
                ViewBag.PointOfAccessID = new SelectList(db.PointsOfAccess, "ID", "Name");
            }
            else
            {
                ViewBag.PointOfAccessID = new SelectList(db.PointsOfAccess.Where(item => !item.RegisteredDevices.Any(reg => reg.Device.PersonID == PersonId)), "ID", "Name");
                ViewBag.DeviceID = new SelectList(db.Devices.Where(item => item.PersonID == PersonId).ToList(), "ID", "TypeCode");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DateCreated,PointOfAccessID,DeviceID")] Registration registration, string ReturnUrl, int? PersonId)
        {
            if (ModelState.IsValid)
            {
                db.Registrations.Add(registration);
                db.SaveChanges();

                if (string.IsNullOrEmpty(ReturnUrl))
                    return RedirectToAction("Index");
                else
                    return Redirect(ReturnUrl);
            }

            PopulateSelectList(ViewBag, PersonId, registration);

            return View(registration);
        }

        public ActionResult AddAccessWithNewDevice(int PersonId, int PoiID)
        {
            var validPointOfAccesses = db.PointsOfAccess.Where(item => item.ID == PoiID).ToList();
            var poi = validPointOfAccesses.FirstOrDefault();

            var userDevices = db.Devices.Where(item => item.PersonID == PersonId).ToList();
            var validUserDevices = userDevices.Where(item => !poi.RegisteredDevices.Any(reg => reg.DeviceID == item.ID)).ToList();

            ViewBag.PointOfAccessID = new SelectList(validPointOfAccesses, "ID", "Name");
            ViewBag.DeviceID = new SelectList(validUserDevices, "ID", "TypeCode");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAccessWithNewDevice([Bind(Include = "ID,DateCreated,PointOfAccessID,DeviceID")] Registration registration, string ReturnUrl, int? PersonId, int PoiID)
        {
            if (ModelState.IsValid)
            {
                db.Registrations.Add(registration);
                db.SaveChanges();

                if (string.IsNullOrEmpty(ReturnUrl))
                    return RedirectToAction("Index");
                else
                    return Redirect(ReturnUrl);
            }

            var validPointOfAccesses = db.PointsOfAccess.Where(item => item.ID == PoiID).ToList();
            var poi = validPointOfAccesses.FirstOrDefault();

            var userDevices = db.Devices.Where(item => item.PersonID == PersonId).ToList();
            var validUserDevices = userDevices.Where(item => !poi.RegisteredDevices.Any(reg => reg.DeviceID == item.ID)).ToList();

            ViewBag.PointOfAccessID = new SelectList(validPointOfAccesses, "ID", "Name");
            ViewBag.DeviceID = new SelectList(validUserDevices, "ID", "TypeCode");

            return View(registration);
        }

        public ActionResult AddAccessPerson(int PoiID)
        {
            var validPointOfAccesses = db.PointsOfAccess.Where(item => item.ID == PoiID).ToList();
            var poi = validPointOfAccesses.FirstOrDefault();

            var registedUsers = poi.RegisteredDevices.Select(reg => reg.Device.PersonID);
            var validPeople = db.Persons.Where(person => !registedUsers.Contains(person.ID)).ToList();
            validPeople = validPeople.Where(person => person.Devices.Count > 0).ToList();

            ViewBag.PointOfAccessID = new SelectList(validPointOfAccesses, "ID", "Name");
            ViewBag.PersonId = new SelectList(validPeople, "ID", "FirstNameLastName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAccessPerson(string ReturnUrl, int? PersonId, int PoiID)
        {
            var person = db.Persons.FirstOrDefault(item => item.ID == PersonId);
            var device = person.Devices.FirstOrDefault();

            var registration = new Registration { PointOfAccessID = PoiID, DeviceID = device.ID, DateCreated = DateTime.Now };
            {
                db.Registrations.Add(registration);
                db.SaveChanges();

                if (string.IsNullOrEmpty(ReturnUrl))
                    return RedirectToAction("Index");
                else
                    return Redirect(ReturnUrl);
            }
        }


        public ActionResult Edit(int? id, string ReturnUrl, int? PersonId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }


            return View(registration);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DateCreated,PointOfAccessID,DeviceID")] Registration registration, string ReturnUrl, int? PersonId)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registration).State = EntityState.Modified;
                db.SaveChanges();

                if (string.IsNullOrEmpty(ReturnUrl))
                    return RedirectToAction("Index");
                else
                    return Redirect(ReturnUrl);
            }

            if (PersonId == null)
            {
                ViewBag.PointOfAccessID = new SelectList(db.PointsOfAccess, "ID", "Name", registration.PointOfAccessID);
                ViewBag.DeviceID = new SelectList(db.Devices, "ID", "TypeCode", registration.DeviceID);
            }
            else
            {
                ViewBag.PointOfAccessID = new SelectList(db.PointsOfAccess.Where(item => !item.RegisteredDevices.Any(reg => reg.Device.PersonID == PersonId)),
                                            "ID", "Name", registration.PointOfAccessID);
                ViewBag.DeviceID = new SelectList(db.Devices.Where(item => item.PersonID == PersonId).ToList(), "ID", "TypeCode", registration.DeviceID);
            }


            return View(registration);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string ReturnUrl)
        {
            Registration registration = db.Registrations.Find(id);
            db.Registrations.Remove(registration);
            db.SaveChanges();

            if (string.IsNullOrEmpty(ReturnUrl))
                return RedirectToAction("Index");
            else
                return Redirect(ReturnUrl);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void PopulateSelectList(dynamic viewBag, int? PersonId, Registration registration)
        {
            if (PersonId == null)
            {
                ViewBag.PointOfAccessID = new SelectList(db.PointsOfAccess, "ID", "Name", registration.PointOfAccessID);
                ViewBag.DeviceID = new SelectList(db.Devices, "ID", "TypeCode", registration.DeviceID);
            }
            else
            {
                ViewBag.PointOfAccessID = new SelectList(db.PointsOfAccess.Where(item => !item.RegisteredDevices.Any(reg => reg.Device.PersonID == PersonId)),
                                            "ID", "Name", registration.PointOfAccessID);
                ViewBag.DeviceID = new SelectList(db.Devices.Where(item => item.PersonID == PersonId).ToList(), "ID", "TypeCode", registration.DeviceID);
            }
        }
    }
}
