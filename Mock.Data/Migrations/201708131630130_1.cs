namespace Mock.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppMenu", "PId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppMenu", "PId");
        }
    }
}
