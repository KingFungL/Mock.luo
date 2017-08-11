namespace Mock.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApiMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppMenu",
                c => new
                    {
                        AppMenuId = c.Int(nullable: false, identity: true),
                        MenuName = c.String(maxLength: 50),
                        SortCode = c.Int(),
                    })
                .PrimaryKey(t => t.AppMenuId);
            
            CreateTable(
                "dbo.AppRole",
                c => new
                    {
                        AppRoleId = c.Int(nullable: false, identity: true),
                        Guid = c.String(maxLength: 50),
                        RoleName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.AppRoleId);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        UserRoleId = c.Int(nullable: false, identity: true),
                        AppRoleId = c.Int(nullable: false),
                        AppUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserRoleId)
                .ForeignKey("dbo.AppRole", t => t.AppRoleId, cascadeDelete: true)
                .ForeignKey("dbo.AppUser", t => t.AppUserId, cascadeDelete: true)
                .Index(t => t.AppRoleId)
                .Index(t => t.AppUserId);
            
            CreateTable(
                "dbo.AppUser",
                c => new
                    {
                        AppUserId = c.Int(nullable: false, identity: true),
                        Guid = c.String(maxLength: 50),
                        UserName = c.String(maxLength: 50),
                        LoginName = c.String(maxLength: 50),
                        LoginPassword = c.String(maxLength: 50),
                        Phone = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        Sex = c.Int(),
                        BranchOfficeId = c.Int(),
                    })
                .PrimaryKey(t => t.AppUserId);
            
            CreateTable(
                "dbo.Upload",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddTime = c.DateTime(),
                        UserName = c.String(maxLength: 50),
                        SortCode = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UploadEntry",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FId = c.Int(nullable: false),
                        AddTime = c.DateTime(),
                        Url = c.String(maxLength: 50),
                        FileName = c.String(maxLength: 50),
                        FileSize = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Upload", t => t.FId, cascadeDelete: true)
                .Index(t => t.FId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UploadEntry", "FId", "dbo.Upload");
            DropForeignKey("dbo.UserRole", "AppUserId", "dbo.AppUser");
            DropForeignKey("dbo.UserRole", "AppRoleId", "dbo.AppRole");
            DropIndex("dbo.UploadEntry", new[] { "FId" });
            DropIndex("dbo.UserRole", new[] { "AppUserId" });
            DropIndex("dbo.UserRole", new[] { "AppRoleId" });
            DropTable("dbo.UploadEntry");
            DropTable("dbo.Upload");
            DropTable("dbo.AppUser");
            DropTable("dbo.UserRole");
            DropTable("dbo.AppRole");
            DropTable("dbo.AppMenu");
        }
    }
}
