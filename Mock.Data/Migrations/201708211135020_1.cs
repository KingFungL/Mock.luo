namespace Mock.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppRole", "IsEnableMark", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppRole", "IsEnableMark");
        }
    }
}
