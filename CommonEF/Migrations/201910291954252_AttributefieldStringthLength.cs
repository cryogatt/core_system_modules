namespace CommonEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttributefieldStringthLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AttributeFields", "Name", c => c.String(nullable: false, maxLength: 32));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AttributeFields", "Name", c => c.String(nullable: false, maxLength: 16));
        }
    }
}
