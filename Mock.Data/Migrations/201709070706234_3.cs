namespace Mock.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AppModule", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppModule", "Status", c => c.Boolean());
        }
    }
}
