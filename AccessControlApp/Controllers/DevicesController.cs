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
    public class DevicesController : Controller
    {
        private AccessControlContext db = new AccessControlContext();

        public ActionResult Index()
        {
            var devices = db.Devices.Include(d => d.Person);
            return View(devices.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        public ActionResult Create(int? PersonId)
        {
            if (PersonId == null)
            {
                ViewBag.PersonID = new SelectList(db.Persons, "ID", "FirstNameLastName");
            }
            else
            {
                ViewBag.PersonID = new SelectList(db.Persons.Where(item => item.ID == PersonId), "ID", "FirstNameLastName");
            }

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Type,Code,DateCreated,PersonID")] Device device, string ReturnUrl, int? PersonId)
        {
            if (ModelState.IsValid)
            {
                db.Devices.Add(device);
                db.SaveChanges();

                if (string.IsNullOrEmpty(ReturnUrl))
                    return RedirectToAction("Index");
                else
                    return Redirect(ReturnUrl);
            }

            if (PersonId == null)
            {
                ViewBag.PersonID = new SelectList(db.Persons, "ID", "FirstNameLastName");
            }
            else
            {
                ViewBag.PersonID = new SelectList(db.Persons.Where(item => item.ID == PersonId), "ID", "FirstNameLastName");
            }

            return View(device);
        }

        // GET: Devices/Edit/5
        public ActionResult Edit(int? id,string ReturnUrl)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonID = new SelectList(db.Persons, "ID", "FirstName", device.PersonID);
            return View(device);
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Type,Code,DateCreated,PersonID")] Device device, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();

                if(string.IsNullOrEmpty(ReturnUrl))
                    return RedirectToAction("Index");
                else
                    return Redirect(ReturnUrl);
            }
            ViewBag.PersonID = new SelectList(db.Persons, "ID", "FirstName", device.PersonID);

            return View(device);            
        }

        // GET: Devices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string ReturnUrl)
        {
            Device device = db.Devices.Find(id);
            db.Devices.Remove(device);
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


        private void PopulateSelectList(dynamic viewBag, int? PersonId)
        {
            if (PersonId == null)
            {
                ViewBag.PersonID = new SelectList(db.Persons, "ID", "FirstNameLastName");
            }
            else
            {
                ViewBag.PersonID = new SelectList(db.Persons.Where(item => item.ID == PersonId), "ID", "FirstNameLastName");
            }
        }
    }
}
