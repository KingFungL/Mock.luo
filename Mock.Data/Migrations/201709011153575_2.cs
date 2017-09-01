namespace Mock.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Review", name: "AuthorId", newName: "CreatorUserId");
            RenameIndex(table: "dbo.Review", name: "IX_AuthorId", newName: "IX_CreatorUserId");
            AddColumn("dbo.Review", "AuName", c => c.String(maxLength: 50));
            AddColumn("dbo.Review", "IsAduit", c => c.Boolean());
            AddColumn("dbo.Review", "CreatorTime", c => c.DateTime());
            AddColumn("dbo.Review", "DeleteMark", c => c.Boolean());
            AddColumn("dbo.Review", "DeleteUserId", c => c.Int());
            AddColumn("dbo.Review", "DeleteTime", c => c.DateTime());
            AddColumn("dbo.Review", "LastModifyUserId", c => c.Int());
            AddColumn("dbo.Review", "LastModifyTime", c => c.DateTime());
            DropColumn("dbo.Review", "AddTime");
            DropColumn("dbo.Review", "Author");
            DropColumn("dbo.Review", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Review", "Status", c => c.String(maxLength: 50));
            AddColumn("dbo.Review", "Author", c => c.String(maxLength: 50));
            AddColumn("dbo.Review", "AddTime", c => c.DateTime());
            DropColumn("dbo.Review", "LastModifyTime");
            DropColumn("dbo.Review", "LastModifyUserId");
            DropColumn("dbo.Review", "DeleteTime");
            DropColumn("dbo.Review", "DeleteUserId");
            DropColumn("dbo.Review", "DeleteMark");
            DropColumn("dbo.Review", "CreatorTime");
            DropColumn("dbo.Review", "IsAduit");
            DropColumn("dbo.Review", "AuName");
            RenameIndex(table: "dbo.Review", name: "IX_CreatorUserId", newName: "IX_AuthorId");
            RenameColumn(table: "dbo.Review", name: "CreatorUserId", newName: "AuthorId");
        }
    }
}
