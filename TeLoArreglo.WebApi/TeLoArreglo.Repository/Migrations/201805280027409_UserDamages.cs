namespace TeLoArreglo.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDamages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DamageReports", "User_Id", c => c.Int());
            CreateIndex("dbo.DamageReports", "User_Id");
            AddForeignKey("dbo.DamageReports", "User_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DamageReports", "User_Id", "dbo.Users");
            DropIndex("dbo.DamageReports", new[] { "User_Id" });
            DropColumn("dbo.DamageReports", "User_Id");
        }
    }
}
