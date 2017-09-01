namespace Mock.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
            AddColumn("dbo.Review", "PId", c => c.Int());
        }
    }
}
