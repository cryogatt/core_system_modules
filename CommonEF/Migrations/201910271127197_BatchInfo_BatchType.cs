namespace CommonEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BatchInfo_BatchType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AttributeValues", "BatchId", "dbo.Batches");
            DropForeignKey("dbo.Batches", "GroupId", "dbo.Groups");
            DropIndex("dbo.Batches", new[] { "Name" });
            DropIndex("dbo.Batches", new[] { "GroupId" });
            DropIndex("dbo.AttributeValues", new[] { "BatchId" });
            CreateTable(
                "dbo.BatchInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Notes = c.String(maxLength: 450),
                        GroupId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.Name, unique: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.BatchTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            AddColumn("dbo.Aliquots", "BatchInfo_Id", c => c.Int());
            AddColumn("dbo.Batches", "BatchTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.Batches", "BatchInfoId", c => c.Int(nullable: false));
            AddColumn("dbo.AttributeValues", "BatchInfoId", c => c.Int(nullable: false));
            CreateIndex("dbo.Aliquots", "BatchInfo_Id");
            CreateIndex("dbo.Batches", "BatchTypeId");
            CreateIndex("dbo.Batches", "BatchInfoId");
            CreateIndex("dbo.AttributeValues", "BatchInfoId");
            AddForeignKey("dbo.Aliquots", "BatchInfo_Id", "dbo.BatchInfoes", "Id");
            AddForeignKey("dbo.AttributeValues", "BatchInfoId", "dbo.BatchInfoes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Batches", "BatchInfoId", "dbo.BatchInfoes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Batches", "BatchTypeId", "dbo.BatchTypes", "Id", cascadeDelete: true);
            DropColumn("dbo.Batches", "Name");
            DropColumn("dbo.Batches", "Date");
            DropColumn("dbo.Batches", "GroupId");
            DropColumn("dbo.Batches", "Notes");
            DropColumn("dbo.AttributeValues", "BatchId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AttributeValues", "BatchId", c => c.Int(nullable: false));
            AddColumn("dbo.Batches", "Notes", c => c.String(maxLength: 450));
            AddColumn("dbo.Batches", "GroupId", c => c.Int(nullable: false));
            AddColumn("dbo.Batches", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Batches", "Name", c => c.String(maxLength: 450));
            DropForeignKey("dbo.Batches", "BatchTypeId", "dbo.BatchTypes");
            DropForeignKey("dbo.Batches", "BatchInfoId", "dbo.BatchInfoes");
            DropForeignKey("dbo.BatchInfoes", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.AttributeValues", "BatchInfoId", "dbo.BatchInfoes");
            DropForeignKey("dbo.Aliquots", "BatchInfo_Id", "dbo.BatchInfoes");
            DropIndex("dbo.BatchTypes", new[] { "Name" });
            DropIndex("dbo.AttributeValues", new[] { "BatchInfoId" });
            DropIndex("dbo.BatchInfoes", new[] { "GroupId" });
            DropIndex("dbo.BatchInfoes", new[] { "Name" });
            DropIndex("dbo.Batches", new[] { "BatchInfoId" });
            DropIndex("dbo.Batches", new[] { "BatchTypeId" });
            DropIndex("dbo.Aliquots", new[] { "BatchInfo_Id" });
            DropColumn("dbo.AttributeValues", "BatchInfoId");
            DropColumn("dbo.Batches", "BatchInfoId");
            DropColumn("dbo.Batches", "BatchTypeId");
            DropColumn("dbo.Aliquots", "BatchInfo_Id");
            DropTable("dbo.BatchTypes");
            DropTable("dbo.BatchInfoes");
            CreateIndex("dbo.AttributeValues", "BatchId");
            CreateIndex("dbo.Batches", "GroupId");
            CreateIndex("dbo.Batches", "Name", unique: true);
            AddForeignKey("dbo.Batches", "GroupId", "dbo.Groups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AttributeValues", "BatchId", "dbo.Batches", "Id", cascadeDelete: true);
        }
    }
}
