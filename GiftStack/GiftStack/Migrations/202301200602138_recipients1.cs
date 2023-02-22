namespace GiftStack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class recipients1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Gifts",
                c => new
                    {
                        GiftId = c.Int(nullable: false, identity: true),
                        GiftName = c.String(),
                        GiftLocation = c.String(),
                        IsGiftPurchaced = c.Boolean(nullable: false),
                        RecipientId = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GiftId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.Recipients", t => t.RecipientId, cascadeDelete: true)
                .Index(t => t.RecipientId)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Gifts", "RecipientId", "dbo.Recipients");
            DropForeignKey("dbo.Gifts", "EventId", "dbo.Events");
            DropIndex("dbo.Gifts", new[] { "EventId" });
            DropIndex("dbo.Gifts", new[] { "RecipientId" });
            DropTable("dbo.Gifts");
        }
    }
}
