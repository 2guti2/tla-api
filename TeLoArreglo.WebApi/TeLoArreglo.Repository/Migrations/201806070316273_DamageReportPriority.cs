namespace TeLoArreglo.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DamageReportPriority : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DamageReports", "Priority", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DamageReports", "Priority");
        }
    }
}
