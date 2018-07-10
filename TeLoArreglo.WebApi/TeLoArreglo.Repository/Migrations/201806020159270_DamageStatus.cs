namespace TeLoArreglo.Repository.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class DamageStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DamageReports", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DamageReports", "Status");
        }
    }
}
