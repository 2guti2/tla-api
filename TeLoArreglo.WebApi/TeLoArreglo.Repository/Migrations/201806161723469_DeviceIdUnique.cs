namespace TeLoArreglo.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeviceIdUnique : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Devices", "DeviceToken", c => c.String(maxLength: 999));
            CreateIndex("dbo.Devices", "DeviceToken", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Devices", new[] { "DeviceToken" });
            AlterColumn("dbo.Devices", "DeviceToken", c => c.String());
        }
    }
}
