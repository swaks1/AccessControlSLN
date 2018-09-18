using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessControlApp.Models.Entities
{
    public class Device
    {
        public int ID { get; set; }
        public DeviceType Type { get; set; }
        public string Code { get; set; }
        public DateTime DateCreated { get; set; }
        public string TypeCode { get { return Type + " - " + Code; } }

        //Navigational Properties
        public int PersonID { get; set; }
        public virtual Person Person { get; set; }

        public virtual ICollection<Registration> Registrations { get; set; }
    }
}