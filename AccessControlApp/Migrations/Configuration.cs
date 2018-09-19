namespace AccessControlApp.Migrations
{
    using AccessControlApp.DataAccessLayer;
    using AccessControlApp.Models.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AccessControlApp.DataAccessLayer.AccessControlContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AccessControlContext context)
        {
            var persons = new List<Person>
            {
            new Person{FirstName="Riste",LastName="Poposki",DateOfBirth=DateTime.Parse("2018-09-17"),EnrollmentDate = DateTime.Now},
            new Person{FirstName="Monkas",LastName="Noob",DateOfBirth=DateTime.Parse("2018-06-01"),EnrollmentDate = DateTime.Now}
            };

            persons.ForEach(s => context.Persons.Add(s));
            context.SaveChanges();


            var devices = new List<Device>
            {
            new Device{Type = DeviceType.Card,Code="12345678",DateCreated = DateTime.Now,PersonID = 1},
            new Device{Type = DeviceType.KeyPadCode,Code="1234",DateCreated = DateTime.Now,PersonID = 2}
            };

            devices.ForEach(s => context.Devices.Add(s));
            context.SaveChanges();


            var pointOfAccess = new List<PointOfAccess>
            {
            new PointOfAccess{Name = "DomaVrata",Location = "Skopje",DateCreated = DateTime.Now,},
            new PointOfAccess{Name = "Garaza",Location = "Struga",DateCreated = DateTime.Now.AddDays(-5)},
            };
            pointOfAccess.ForEach(s => context.PointsOfAccess.Add(s));
            context.SaveChanges();


            var registrations = new List<Registration>
            {
            new Registration{PointOfAccessID = 1, DeviceID = 1,DateCreated = DateTime.Now,},
            new Registration{PointOfAccessID = 1, DeviceID = 2,DateCreated = DateTime.Now,},
            new Registration{PointOfAccessID = 2, DeviceID = 1,DateCreated = DateTime.Now,},
            };
            registrations.ForEach(s => context.Registrations.Add(s));
            context.SaveChanges();


            var logs = new List<EntryLog>
            {
            new EntryLog{PointOfAccessID = 1, DeviceID = 2,DateCreated = DateTime.Now.AddDays(-1),Success = true},
            new EntryLog{PointOfAccessID = 2, DeviceID = 1,DateCreated = DateTime.Now.AddDays(-2),Success = false},
            };
            logs.ForEach(s => context.EntryLogs.Add(s));
            context.SaveChanges();


        }
    }
}
