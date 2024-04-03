namespace CommonEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShipmentTableMod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "SenderSiteId", c => c.Int(nullable: false));
            AddColumn("dbo.Shipments", "RecipientId", c => c.Int(nullable: false));
            DropColumn("dbo.Shipments", "enameme");
            DropColumn("dbo.Shipments", "renameme2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shipments", "renameme2", c => c.Int(nullable: false));
            AddColumn("dbo.Shipments", "enameme", c => c.Int(nullable: false));
            DropColumn("dbo.Shipments", "RecipientId");
            DropColumn("dbo.Shipments", "SenderSiteId");
        }
    }
}
