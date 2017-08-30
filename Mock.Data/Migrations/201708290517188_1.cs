namespace Mock.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppMenu", "Name", c => c.String(maxLength: 50));
            AddColumn("dbo.AppMenu", "Expanded", c => c.Boolean());
            AddColumn("dbo.AppMenu", "Folder", c => c.Boolean());
            DropColumn("dbo.AppMenu", "MenuName");
            DropColumn("dbo.AppMenu", "State");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppMenu", "State", c => c.String(maxLength: 20));
            AddColumn("dbo.AppMenu", "MenuName", c => c.String(maxLength: 50));
            DropColumn("dbo.AppMenu", "Folder");
            DropColumn("dbo.AppMenu", "Expanded");
            DropColumn("dbo.AppMenu", "Name");
        }
    }
}
