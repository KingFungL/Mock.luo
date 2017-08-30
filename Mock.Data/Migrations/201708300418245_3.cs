namespace Mock.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemsDetail", "IsEnableMark", c => c.Boolean());
            DropColumn("dbo.ItemsDetail", "PId");
            DropColumn("dbo.ItemsDetail", "IsDefault");
            DropColumn("dbo.ItemsDetail", "Layers");
            DropColumn("dbo.ItemsDetail", "EnabledMark");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ItemsDetail", "EnabledMark", c => c.Boolean());
            AddColumn("dbo.ItemsDetail", "Layers", c => c.Int());
            AddColumn("dbo.ItemsDetail", "IsDefault", c => c.Boolean());
            AddColumn("dbo.ItemsDetail", "PId", c => c.Int(nullable: false));
            DropColumn("dbo.ItemsDetail", "IsEnableMark");
        }
    }
}
