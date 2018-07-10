namespace TeLoArreglo.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserThatRepairedDamageReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DamageReports", "CrewMemberThatRepairedTheDamage_Id", c => c.Int());
            CreateIndex("dbo.DamageReports", "CrewMemberThatRepairedTheDamage_Id");
            AddForeignKey("dbo.DamageReports", "CrewMemberThatRepairedTheDamage_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DamageReports", "CrewMemberThatRepairedTheDamage_Id", "dbo.Users");
            DropIndex("dbo.DamageReports", new[] { "CrewMemberThatRepairedTheDamage_Id" });
            DropColumn("dbo.DamageReports", "CrewMemberThatRepairedTheDamage_Id");
        }
    }
}
