namespace GiftStack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class recipients : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Recipients",
                c => new
                    {
                        RecipientId = c.Int(nullable: false, identity: true),
                        RecipientName = c.String(),
                    })
                .PrimaryKey(t => t.RecipientId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Recipients");
        }
    }
}
