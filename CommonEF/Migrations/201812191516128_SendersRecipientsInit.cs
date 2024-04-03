namespace CommonEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SendersRecipientsInit : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sites", "Shipment_Id", "dbo.Shipments");
            DropForeignKey("dbo.Sites", "Shipment_Id1", "dbo.Shipments");
            DropIndex("dbo.Sites", new[] { "Shipment_Id" });
            DropIndex("dbo.Sites", new[] { "Shipment_Id1" });
            CreateTable(
                "dbo.Recipients",
                c => new
                    {
                        ShipmentId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ShipmentId)
                .ForeignKey("dbo.Shipments", t => t.ShipmentId)
                .ForeignKey("dbo.Sites", t => t.SiteId, cascadeDelete: true)
                .Index(t => t.ShipmentId)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.Senders",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shipments", t => t.Id)
                .ForeignKey("dbo.Sites", t => t.SiteId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.SiteId);
            
            AddColumn("dbo.Shipments", "enameme", c => c.Int(nullable: false));
            AddColumn("dbo.Shipments", "renameme2", c => c.Int(nullable: false));
            DropColumn("dbo.Sites", "Shipment_Id");
            DropColumn("dbo.Sites", "Shipment_Id1");
            DropColumn("dbo.Shipments", "SenderSiteId");
            DropColumn("dbo.Shipments", "RecipientSiteId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shipments", "RecipientSiteId", c => c.Int(nullable: false));
            AddColumn("dbo.Shipments", "SenderSiteId", c => c.Int(nullable: false));
            AddColumn("dbo.Sites", "Shipment_Id1", c => c.Int());
            AddColumn("dbo.Sites", "Shipment_Id", c => c.Int());
            DropForeignKey("dbo.Senders", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.Senders", "Id", "dbo.Shipments");
            DropForeignKey("dbo.Recipients", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.Recipients", "ShipmentId", "dbo.Shipments");
            DropIndex("dbo.Senders", new[] { "SiteId" });
            DropIndex("dbo.Senders", new[] { "Id" });
            DropIndex("dbo.Recipients", new[] { "SiteId" });
            DropIndex("dbo.Recipients", new[] { "ShipmentId" });
            DropColumn("dbo.Shipments", "renameme2");
            DropColumn("dbo.Shipments", "enameme");
            DropTable("dbo.Senders");
            DropTable("dbo.Recipients");
            CreateIndex("dbo.Sites", "Shipment_Id1");
            CreateIndex("dbo.Sites", "Shipment_Id");
            AddForeignKey("dbo.Sites", "Shipment_Id1", "dbo.Shipments", "Id");
            AddForeignKey("dbo.Sites", "Shipment_Id", "dbo.Shipments", "Id");
        }
    }
}
