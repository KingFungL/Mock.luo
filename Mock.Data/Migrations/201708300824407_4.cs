namespace Mock.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "IsEnableMark", c => c.Boolean());
            DropColumn("dbo.Items", "EnabledMark");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "EnabledMark", c => c.Boolean());
            DropColumn("dbo.Items", "IsEnableMark");
        }
    }
}
