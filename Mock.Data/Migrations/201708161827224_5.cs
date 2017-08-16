namespace Mock.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Review", "AId");
            RenameColumn(table: "dbo.Review", name: "PId", newName: "AId");
            RenameIndex(table: "dbo.Review", name: "IX_PId", newName: "IX_AId");
            AddColumn("dbo.Article", "TypeCode", c => c.Int());
            CreateIndex("dbo.Article", "TypeCode");
            AddForeignKey("dbo.Article", "TypeCode", "dbo.ItemsDetail", "Id");
            DropColumn("dbo.Article", "FTypeCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Article", "FTypeCode", c => c.String(maxLength: 50));
            DropForeignKey("dbo.Article", "TypeCode", "dbo.ItemsDetail");
            DropIndex("dbo.Article", new[] { "TypeCode" });
            DropColumn("dbo.Article", "TypeCode");
            RenameIndex(table: "dbo.Review", name: "IX_AId", newName: "IX_PId");
            RenameColumn(table: "dbo.Review", name: "AId", newName: "PId");
            AddColumn("dbo.Review", "AId", c => c.Int());
        }
    }
}
