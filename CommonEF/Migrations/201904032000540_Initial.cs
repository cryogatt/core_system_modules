namespace CommonEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Aliquots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BatchId = c.Int(nullable: false),
                        Serial = c.String(maxLength: 50),
                        ContainerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Batches", t => t.BatchId, cascadeDelete: true)
                .ForeignKey("dbo.Containers", t => t.ContainerId, cascadeDelete: true)
                .Index(t => t.BatchId)
                .Index(t => t.ContainerId);
            
            CreateTable(
                "dbo.Batches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 450),
                        Date = c.DateTime(nullable: false),
                        GroupId = c.Int(nullable: false),
                        Notes = c.String(maxLength: 450),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.Name, unique: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.AttributeValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        AttributeFieldId = c.Int(nullable: false),
                        BatchId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AttributeFields", t => t.AttributeFieldId, cascadeDelete: true)
                .ForeignKey("dbo.Batches", t => t.BatchId, cascadeDelete: true)
                .Index(t => t.AttributeFieldId)
                .Index(t => t.BatchId);
            
            CreateTable(
                "dbo.AttributeFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 16),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 250),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Username = c.String(nullable: false, maxLength: 20),
                        Email = c.String(maxLength: 450),
                        Password = c.String(nullable: false, maxLength: 450),
                        LastLogin = c.DateTime(),
                        RememberToken = c.String(),
                        UpdatedAt = c.DateTime(),
                        CreatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Description = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address = c.String(maxLength: 450),
                        Name = c.String(nullable: false),
                        ContainerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Containers", t => t.ContainerId, cascadeDelete: true)
                .Index(t => t.ContainerId);
            
            CreateTable(
                "dbo.Containers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uid = c.String(maxLength: 450),
                        Description = c.String(nullable: false),
                        ContainerIdentId = c.Int(nullable: false),
                        ParentContainerStorageId = c.Int(),
                        StorageIndex = c.Int(nullable: false),
                        ParentContainerLocationId = c.Int(),
                        LocationIndex = c.Int(nullable: false),
                        Flags = c.Int(nullable: false),
                        InceptDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContainerIdents", t => t.ContainerIdentId, cascadeDelete: true)
                .ForeignKey("dbo.Containers", t => t.ParentContainerStorageId)
                .Index(t => t.Uid, unique: true)
                .Index(t => t.ContainerIdentId)
                .Index(t => t.ParentContainerStorageId);
            
            CreateTable(
                "dbo.ContainerIdents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        TagIdent = c.Int(nullable: false),
                        ContainerTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContainerTypes", t => t.ContainerTypeId, cascadeDelete: true)
                .Index(t => t.ContainerTypeId);
            
            CreateTable(
                "dbo.ContainerTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShipmentId = c.Int(nullable: false),
                        ContainerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Containers", t => t.ContainerId, cascadeDelete: true)
                .ForeignKey("dbo.Shipments", t => t.ShipmentId, cascadeDelete: true)
                .Index(t => t.ShipmentId)
                .Index(t => t.ContainerId);
            
            CreateTable(
                "dbo.Shipments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConsignmentNo = c.String(nullable: false, maxLength: 450),
                        SenderSiteId = c.Int(nullable: false),
                        RecipientId = c.Int(nullable: false),
                        OrderedDate = c.DateTime(nullable: false),
                        DispatchedDate = c.DateTime(),
                        ArrivedDate = c.DateTime(),
                        Notes = c.String(),
                        SampleQty = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .Index(t => t.ConsignmentNo, unique: true)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Couriers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContainerId = c.Int(nullable: false),
                        ShipmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Containers", t => t.ContainerId, cascadeDelete: true)
                .ForeignKey("dbo.Shipments", t => t.ShipmentId, cascadeDelete: true)
                .Index(t => t.ContainerId)
                .Index(t => t.ShipmentId);
            
            CreateTable(
                "dbo.PickListItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PickListId = c.Int(nullable: false),
                        ContainerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Containers", t => t.ContainerId, cascadeDelete: true)
                .ForeignKey("dbo.PickLists", t => t.PickListId, cascadeDelete: true)
                .Index(t => t.PickListId)
                .Index(t => t.ContainerId);
            
            CreateTable(
                "dbo.PickLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Points",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContainerId = c.Int(nullable: false),
                        Uid = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        ContainerIdentId = c.Int(nullable: false),
                        ParentContainerStorageId = c.Int(),
                        StorageIndex = c.Int(nullable: false),
                        ParentContainerLocationId = c.Int(),
                        LocationIndex = c.Int(nullable: false),
                        Flags = c.Int(nullable: false),
                        InceptDate = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        Mark = c.DateTime(nullable: false),
                        Reason = c.String(),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
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
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Group_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Group_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.SiteUsers",
                c => new
                    {
                        Site_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Site_Id, t.User_Id })
                .ForeignKey("dbo.Sites", t => t.Site_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Site_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Senders", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.Senders", "Id", "dbo.Shipments");
            DropForeignKey("dbo.Recipients", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.Recipients", "ShipmentId", "dbo.Shipments");
            DropForeignKey("dbo.Points", "UserId", "dbo.Users");
            DropForeignKey("dbo.PickListItems", "PickListId", "dbo.PickLists");
            DropForeignKey("dbo.PickLists", "UserId", "dbo.Users");
            DropForeignKey("dbo.PickListItems", "ContainerId", "dbo.Containers");
            DropForeignKey("dbo.Couriers", "ShipmentId", "dbo.Shipments");
            DropForeignKey("dbo.Couriers", "ContainerId", "dbo.Containers");
            DropForeignKey("dbo.Contents", "ShipmentId", "dbo.Shipments");
            DropForeignKey("dbo.Shipments", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Contents", "ContainerId", "dbo.Containers");
            DropForeignKey("dbo.Aliquots", "ContainerId", "dbo.Containers");
            DropForeignKey("dbo.Aliquots", "BatchId", "dbo.Batches");
            DropForeignKey("dbo.Batches", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.SiteUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.SiteUsers", "Site_Id", "dbo.Sites");
            DropForeignKey("dbo.Sites", "ContainerId", "dbo.Containers");
            DropForeignKey("dbo.Containers", "ParentContainerStorageId", "dbo.Containers");
            DropForeignKey("dbo.ContainerIdents", "ContainerTypeId", "dbo.ContainerTypes");
            DropForeignKey("dbo.Containers", "ContainerIdentId", "dbo.ContainerIdents");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserGroups", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.UserGroups", "User_Id", "dbo.Users");
            DropForeignKey("dbo.AttributeValues", "BatchId", "dbo.Batches");
            DropForeignKey("dbo.AttributeValues", "AttributeFieldId", "dbo.AttributeFields");
            DropIndex("dbo.SiteUsers", new[] { "User_Id" });
            DropIndex("dbo.SiteUsers", new[] { "Site_Id" });
            DropIndex("dbo.UserGroups", new[] { "Group_Id" });
            DropIndex("dbo.UserGroups", new[] { "User_Id" });
            DropIndex("dbo.Senders", new[] { "SiteId" });
            DropIndex("dbo.Senders", new[] { "Id" });
            DropIndex("dbo.Recipients", new[] { "SiteId" });
            DropIndex("dbo.Recipients", new[] { "ShipmentId" });
            DropIndex("dbo.Points", new[] { "UserId" });
            DropIndex("dbo.PickLists", new[] { "UserId" });
            DropIndex("dbo.PickListItems", new[] { "ContainerId" });
            DropIndex("dbo.PickListItems", new[] { "PickListId" });
            DropIndex("dbo.Couriers", new[] { "ShipmentId" });
            DropIndex("dbo.Couriers", new[] { "ContainerId" });
            DropIndex("dbo.Shipments", new[] { "StatusId" });
            DropIndex("dbo.Shipments", new[] { "ConsignmentNo" });
            DropIndex("dbo.Contents", new[] { "ContainerId" });
            DropIndex("dbo.Contents", new[] { "ShipmentId" });
            DropIndex("dbo.ContainerIdents", new[] { "ContainerTypeId" });
            DropIndex("dbo.Containers", new[] { "ParentContainerStorageId" });
            DropIndex("dbo.Containers", new[] { "ContainerIdentId" });
            DropIndex("dbo.Containers", new[] { "Uid" });
            DropIndex("dbo.Sites", new[] { "ContainerId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Groups", new[] { "Name" });
            DropIndex("dbo.AttributeValues", new[] { "BatchId" });
            DropIndex("dbo.AttributeValues", new[] { "AttributeFieldId" });
            DropIndex("dbo.Batches", new[] { "GroupId" });
            DropIndex("dbo.Batches", new[] { "Name" });
            DropIndex("dbo.Aliquots", new[] { "ContainerId" });
            DropIndex("dbo.Aliquots", new[] { "BatchId" });
            DropTable("dbo.SiteUsers");
            DropTable("dbo.UserGroups");
            DropTable("dbo.Senders");
            DropTable("dbo.Recipients");
            DropTable("dbo.Points");
            DropTable("dbo.PickLists");
            DropTable("dbo.PickListItems");
            DropTable("dbo.Couriers");
            DropTable("dbo.Status");
            DropTable("dbo.Shipments");
            DropTable("dbo.Contents");
            DropTable("dbo.ContainerTypes");
            DropTable("dbo.ContainerIdents");
            DropTable("dbo.Containers");
            DropTable("dbo.Sites");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Groups");
            DropTable("dbo.AttributeFields");
            DropTable("dbo.AttributeValues");
            DropTable("dbo.Batches");
            DropTable("dbo.Aliquots");
        }
    }
}
