namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Duration = c.Single(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        IsCancelled = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 200),
                        Password = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Phone = c.String(),
                        IsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.AppointmentActivities",
                c => new
                    {
                        Appointment_Id = c.Int(nullable: false),
                        Activity_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Appointment_Id, t.Activity_Id })
                .ForeignKey("dbo.Appointments", t => t.Appointment_Id, cascadeDelete: true)
                .ForeignKey("dbo.Activities", t => t.Activity_Id, cascadeDelete: true)
                .Index(t => t.Appointment_Id)
                .Index(t => t.Activity_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "UserId", "dbo.Users");
            DropForeignKey("dbo.AppointmentActivities", "Activity_Id", "dbo.Activities");
            DropForeignKey("dbo.AppointmentActivities", "Appointment_Id", "dbo.Appointments");
            DropIndex("dbo.AppointmentActivities", new[] { "Activity_Id" });
            DropIndex("dbo.AppointmentActivities", new[] { "Appointment_Id" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Appointments", new[] { "UserId" });
            DropTable("dbo.AppointmentActivities");
            DropTable("dbo.Users");
            DropTable("dbo.Appointments");
            DropTable("dbo.Activities");
        }
    }
}
