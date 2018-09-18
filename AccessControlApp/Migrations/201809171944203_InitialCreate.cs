namespace AccessControlApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Device",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Code = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        PersonID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Person", t => t.PersonID, cascadeDelete: true)
                .Index(t => t.PersonID);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        EnrollmentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Registration",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        PointOfAccessID = c.Int(nullable: false),
                        DeviceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Device", t => t.DeviceID, cascadeDelete: true)
                .ForeignKey("dbo.PointOfAccess", t => t.PointOfAccessID, cascadeDelete: true)
                .Index(t => t.PointOfAccessID)
                .Index(t => t.DeviceID);
            
            CreateTable(
                "dbo.PointOfAccess",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.EntryLog",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Success = c.Boolean(nullable: false),
                        PointOfAccessID = c.Int(nullable: false),
                        DeviceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Device", t => t.DeviceID, cascadeDelete: true)
                .ForeignKey("dbo.PointOfAccess", t => t.PointOfAccessID, cascadeDelete: true)
                .Index(t => t.PointOfAccessID)
                .Index(t => t.DeviceID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Registration", "PointOfAccessID", "dbo.PointOfAccess");
            DropForeignKey("dbo.EntryLog", "PointOfAccessID", "dbo.PointOfAccess");
            DropForeignKey("dbo.EntryLog", "DeviceID", "dbo.Device");
            DropForeignKey("dbo.Registration", "DeviceID", "dbo.Device");
            DropForeignKey("dbo.Device", "PersonID", "dbo.Person");
            DropIndex("dbo.EntryLog", new[] { "DeviceID" });
            DropIndex("dbo.EntryLog", new[] { "PointOfAccessID" });
            DropIndex("dbo.Registration", new[] { "DeviceID" });
            DropIndex("dbo.Registration", new[] { "PointOfAccessID" });
            DropIndex("dbo.Device", new[] { "PersonID" });
            DropTable("dbo.EntryLog");
            DropTable("dbo.PointOfAccess");
            DropTable("dbo.Registration");
            DropTable("dbo.Person");
            DropTable("dbo.Device");
        }
    }
}
