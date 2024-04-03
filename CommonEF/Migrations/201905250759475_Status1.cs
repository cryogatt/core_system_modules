namespace CommonEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Status1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Points", "Uid", c => c.String(nullable: false));
            AlterColumn("dbo.Points", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Points", "Description", c => c.String());
            AlterColumn("dbo.Points", "Uid", c => c.String());
        }
    }
}
