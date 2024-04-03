namespace CommonEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedQtys : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BatchInfoes", "CryoSeedQty", c => c.Int(nullable: false));
            AddColumn("dbo.BatchInfoes", "TestedSeedQty", c => c.Int(nullable: false));
            AddColumn("dbo.BatchInfoes", "SDSeedQty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BatchInfoes", "SDSeedQty");
            DropColumn("dbo.BatchInfoes", "TestedSeedQty");
            DropColumn("dbo.BatchInfoes", "CryoSeedQty");
        }
    }
}
