namespace Mock.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Article", name: "TypeCode", newName: "FId");
            RenameIndex(table: "dbo.Article", name: "IX_TypeCode", newName: "IX_FId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Article", name: "IX_FId", newName: "IX_TypeCode");
            RenameColumn(table: "dbo.Article", name: "FId", newName: "TypeCode");
        }
    }
}
