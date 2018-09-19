using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessControlApp.Models.Entities
{
    public class Person
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string FirstNameLastName { get { return FirstName + " " + LastName; } }
        //Navigational Properties
        public virtual ICollection<Device> Devices { get; set; }
    }
}