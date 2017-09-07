namespace Mock.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AppModule", "Folder");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppModule", "Folder", c => c.Boolean());
        }
    }
}
