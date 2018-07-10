namespace TeLoArreglo.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DamageGeoCoordinate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DamageReports", "GeoCoordinate_Longitude", c => c.Double(nullable: false));
            AddColumn("dbo.DamageReports", "GeoCoordinate_Latitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DamageReports", "GeoCoordinate_Latitude");
            DropColumn("dbo.DamageReports", "GeoCoordinate_Longitude");
        }
    }
}
