using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessControlApp.Models.Entities
{
    public class EntryLog
    {
        public int ID { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Success { get; set; }

        //Navigational Properties
        public int PointOfAccessID { get; set; }
        public virtual PointOfAccess PointOfAccess { get; set; }

        public int DeviceID { get; set; }        
        public virtual Device Device { get; set; }
    }
}