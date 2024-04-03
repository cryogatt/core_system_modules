namespace CommonEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Statuses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContainerStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContainerUid = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ContainerStatus");
        }
    }
}
