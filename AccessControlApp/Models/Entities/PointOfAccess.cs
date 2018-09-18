using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessControlApp.Models.Entities
{
    public class PointOfAccess
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime DateCreated { get; set; }

        //Navigational Properties
        public virtual ICollection<Registration> RegisteredDevices { get; set; }
        public virtual ICollection<EntryLog> EntryLogs { get; set; }
    }
}