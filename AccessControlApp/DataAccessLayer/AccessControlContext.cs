using AccessControlApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web;

namespace AccessControlApp.DataAccessLayer
{
    public class AccessControlContext : DbContext
    {
        public AccessControlContext() : base("AccessControlContext") 
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<PointOfAccess> PointsOfAccess { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<EntryLog> EntryLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}