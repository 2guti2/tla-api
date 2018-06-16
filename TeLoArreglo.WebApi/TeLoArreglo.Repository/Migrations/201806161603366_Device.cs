namespace TeLoArreglo.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Device : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeviceToken = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Devices", "User_Id", "dbo.Users");
            DropIndex("dbo.Devices", new[] { "User_Id" });
            DropTable("dbo.Devices");
        }
    }
}
