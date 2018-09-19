using AccessControlApp.DataAccessLayer;
using AccessControlApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccessControlApp.Controllers
{
    public class HomeController : Controller
    {
        private AccessControlContext db = new AccessControlContext();

        public ActionResult Index()
        {
            return View();
        }

        public string GetAccess(string PoiId, string Code)
        {
            try
            {
                int _PoiId = Int32.Parse(PoiId);

                var poi = db.PointsOfAccess.FirstOrDefault(item => item.ID == _PoiId);
                var device = db.Devices.FirstOrDefault(item => item.Code == Code);
                    
                var success = poi.RegisteredDevices.FirstOrDefault(reg => reg.Device.Code == Code) != null;

                var entryLog = new EntryLog
                {
                    DateCreated = DateTime.Now,
                    DeviceID = device.ID,
                    PointOfAccessID = poi.ID,
                    Success = success
                };
                db.EntryLogs.Add(entryLog);
                db.SaveChanges();

                if (success)
                {
                    return "TRUE";
                }                    
                else
                {
                    return "FALSE";
                }
            }
            catch (Exception e)
            {
                return "EXCEPTION";
            }
        }
    }
}