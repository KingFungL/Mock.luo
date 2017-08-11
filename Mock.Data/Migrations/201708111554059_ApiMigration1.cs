namespace Mock.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApiMigration1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UploadEntry", "Url", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UploadEntry", "Url", c => c.String(maxLength: 50));
        }
    }
}
