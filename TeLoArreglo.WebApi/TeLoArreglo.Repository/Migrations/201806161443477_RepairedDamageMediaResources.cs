namespace TeLoArreglo.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RepairedDamageMediaResources : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Media", "DamageReport_Id1", c => c.Int());
            CreateIndex("dbo.Media", "DamageReport_Id1");
            AddForeignKey("dbo.Media", "DamageReport_Id1", "dbo.DamageReports", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Media", "DamageReport_Id1", "dbo.DamageReports");
            DropIndex("dbo.Media", new[] { "DamageReport_Id1" });
            DropColumn("dbo.Media", "DamageReport_Id1");
        }
    }
}
