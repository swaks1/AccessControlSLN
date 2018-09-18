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
    public class EntryLogsController : Controller
    {
        private AccessControlContext db = new AccessControlContext();

        // GET: EntryLogs
        public ActionResult Index()
        {
            var entryLogs = db.EntryLogs.Include(e => e.Device).Include(e => e.PointOfAccess);
            return View(entryLogs.ToList());
        }

        // GET: EntryLogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntryLog entryLog = db.EntryLogs.Find(id);
            if (entryLog == null)
            {
                return HttpNotFound();
            }
            return View(entryLog);
        }

        // GET: EntryLogs/Create
        public ActionResult Create()
        {
            ViewBag.DeviceID = new SelectList(db.Devices, "ID", "Code");
            ViewBag.PointOfAccessID = new SelectList(db.PointsOfAccess, "ID", "Name");
            return View();
        }

        // POST: EntryLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DateCreated,Success,PointOfAccessID,DeviceID")] EntryLog entryLog)
        {
            if (ModelState.IsValid)
            {
                db.EntryLogs.Add(entryLog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DeviceID = new SelectList(db.Devices, "ID", "Code", entryLog.DeviceID);
            ViewBag.PointOfAccessID = new SelectList(db.PointsOfAccess, "ID", "Name", entryLog.PointOfAccessID);
            return View(entryLog);
        }

        // GET: EntryLogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntryLog entryLog = db.EntryLogs.Find(id);
            if (entryLog == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeviceID = new SelectList(db.Devices, "ID", "Code", entryLog.DeviceID);
            ViewBag.PointOfAccessID = new SelectList(db.PointsOfAccess, "ID", "Name", entryLog.PointOfAccessID);
            return View(entryLog);
        }

        // POST: EntryLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DateCreated,Success,PointOfAccessID,DeviceID")] EntryLog entryLog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entryLog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DeviceID = new SelectList(db.Devices, "ID", "Code", entryLog.DeviceID);
            ViewBag.PointOfAccessID = new SelectList(db.PointsOfAccess, "ID", "Name", entryLog.PointOfAccessID);
            return View(entryLog);
        }

        // GET: EntryLogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntryLog entryLog = db.EntryLogs.Find(id);
            if (entryLog == null)
            {
                return HttpNotFound();
            }
            return View(entryLog);
        }

        // POST: EntryLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EntryLog entryLog = db.EntryLogs.Find(id);
            db.EntryLogs.Remove(entryLog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
