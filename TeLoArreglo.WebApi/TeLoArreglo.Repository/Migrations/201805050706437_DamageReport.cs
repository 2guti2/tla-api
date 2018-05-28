namespace TeLoArreglo.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DamageReport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DamageReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Media",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MediaType = c.Int(nullable: false),
                        Path = c.String(),
                        OriginalName = c.String(),
                        DamageReport_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DamageReports", t => t.DamageReport_Id)
                .Index(t => t.DamageReport_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Media", "DamageReport_Id", "dbo.DamageReports");
            DropIndex("dbo.Media", new[] { "DamageReport_Id" });
            DropTable("dbo.Media");
            DropTable("dbo.DamageReports");
        }
    }
}
