namespace Mock.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _123 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Review", new[] { "CreatorUserId" });
            DropIndex("dbo.ReLeave", new[] { "CreatorUserId" });
            DropPrimaryKey("dbo.Review");
            DropPrimaryKey("dbo.ReLeave");
            CreateTable(
                "dbo.Reply",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PId = c.Int(),
                        Text = c.String(maxLength: 500),
                        Ip = c.String(maxLength: 50),
                        Agent = c.String(maxLength: 50),
                        AuName = c.String(maxLength: 50),
                        AuEmail = c.String(maxLength: 50),
                        IsAduit = c.Boolean(),
                        CreatorUserId = c.Int(),
                        CreatorTime = c.DateTime(),
                        DeleteMark = c.Boolean(),
                        DeleteUserId = c.Int(),
                        DeleteTime = c.DateTime(),
                        LastModifyUserId = c.Int(),
                        LastModifyTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CreatorUserId);
            
            AlterColumn("dbo.Review", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.ReLeave", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Review", "Id");
            AddPrimaryKey("dbo.ReLeave", "Id");
            CreateIndex("dbo.Review", "Id");
            CreateIndex("dbo.ReLeave", "Id");
            AddForeignKey("dbo.Review", "Id", "dbo.Reply", "Id");
            AddForeignKey("dbo.ReLeave", "Id", "dbo.Reply", "Id");
            DropColumn("dbo.Review", "PId");
            DropColumn("dbo.Review", "Text");
            DropColumn("dbo.Review", "Ip");
            DropColumn("dbo.Review", "Agent");
            DropColumn("dbo.Review", "AuName");
            DropColumn("dbo.Review", "AuEmail");
            DropColumn("dbo.Review", "IsAduit");
            DropColumn("dbo.Review", "CreatorTime");
            DropColumn("dbo.Review", "DeleteMark");
            DropColumn("dbo.Review", "DeleteUserId");
            DropColumn("dbo.Review", "DeleteTime");
            DropColumn("dbo.Review", "LastModifyUserId");
            DropColumn("dbo.Review", "LastModifyTime");
            DropColumn("dbo.ReLeave", "PId");
            DropColumn("dbo.ReLeave", "Text");
            DropColumn("dbo.ReLeave", "Ip");
            DropColumn("dbo.ReLeave", "Agent");
            DropColumn("dbo.ReLeave", "AuName");
            DropColumn("dbo.ReLeave", "AuEmail");
            DropColumn("dbo.ReLeave", "IsAduit");
            DropColumn("dbo.ReLeave", "CreatorTime");
            DropColumn("dbo.ReLeave", "DeleteMark");
            DropColumn("dbo.ReLeave", "DeleteUserId");
            DropColumn("dbo.ReLeave", "DeleteTime");
            DropColumn("dbo.ReLeave", "LastModifyUserId");
            DropColumn("dbo.ReLeave", "LastModifyTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReLeave", "LastModifyTime", c => c.DateTime());
            AddColumn("dbo.ReLeave", "LastModifyUserId", c => c.Int());
            AddColumn("dbo.ReLeave", "DeleteTime", c => c.DateTime());
            AddColumn("dbo.ReLeave", "DeleteUserId", c => c.Int());
            AddColumn("dbo.ReLeave", "DeleteMark", c => c.Boolean());
            AddColumn("dbo.ReLeave", "CreatorTime", c => c.DateTime());
            AddColumn("dbo.ReLeave", "CreatorUserId", c => c.Int());
            AddColumn("dbo.ReLeave", "IsAduit", c => c.Boolean());
            AddColumn("dbo.ReLeave", "AuEmail", c => c.String(maxLength: 50));
            AddColumn("dbo.ReLeave", "AuName", c => c.String(maxLength: 50));
            AddColumn("dbo.ReLeave", "Agent", c => c.String(maxLength: 50));
            AddColumn("dbo.ReLeave", "Ip", c => c.String(maxLength: 50));
            AddColumn("dbo.ReLeave", "Text", c => c.String(maxLength: 500));
            AddColumn("dbo.ReLeave", "PId", c => c.Int());
            AddColumn("dbo.Review", "LastModifyTime", c => c.DateTime());
            AddColumn("dbo.Review", "LastModifyUserId", c => c.Int());
            AddColumn("dbo.Review", "DeleteTime", c => c.DateTime());
            AddColumn("dbo.Review", "DeleteUserId", c => c.Int());
            AddColumn("dbo.Review", "DeleteMark", c => c.Boolean());
            AddColumn("dbo.Review", "CreatorTime", c => c.DateTime());
            AddColumn("dbo.Review", "CreatorUserId", c => c.Int());
            AddColumn("dbo.Review", "IsAduit", c => c.Boolean());
            AddColumn("dbo.Review", "AuEmail", c => c.String(maxLength: 50));
            AddColumn("dbo.Review", "AuName", c => c.String(maxLength: 50));
            AddColumn("dbo.Review", "Agent", c => c.String(maxLength: 50));
            AddColumn("dbo.Review", "Ip", c => c.String(maxLength: 50));
            AddColumn("dbo.Review", "Text", c => c.String(maxLength: 500));
            AddColumn("dbo.Review", "PId", c => c.Int());
            DropForeignKey("dbo.ReLeave", "Id", "dbo.Reply");
            DropForeignKey("dbo.Review", "Id", "dbo.Reply");
            DropIndex("dbo.ReLeave", new[] { "Id" });
            DropIndex("dbo.Review", new[] { "Id" });
            DropIndex("dbo.Reply", new[] { "CreatorUserId" });
            DropPrimaryKey("dbo.ReLeave");
            DropPrimaryKey("dbo.Review");
            AlterColumn("dbo.ReLeave", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Review", "Id", c => c.Int(nullable: false, identity: true));
            DropTable("dbo.Reply");
            AddPrimaryKey("dbo.ReLeave", "Id");
            AddPrimaryKey("dbo.Review", "Id");
            CreateIndex("dbo.ReLeave", "CreatorUserId");
            CreateIndex("dbo.Review", "CreatorUserId");
        }
    }
}
