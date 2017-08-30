namespace Mock.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Open", c => c.Boolean());
            AddColumn("dbo.Items", "Remark", c => c.String(maxLength: 200));
            DropColumn("dbo.Items", "IsTree");
            DropColumn("dbo.Items", "Layers");
            DropColumn("dbo.Items", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "Description", c => c.String(maxLength: 200));
            AddColumn("dbo.Items", "Layers", c => c.Int());
            AddColumn("dbo.Items", "IsTree", c => c.Boolean());
            DropColumn("dbo.Items", "Remark");
            DropColumn("dbo.Items", "Open");
        }
    }
}
