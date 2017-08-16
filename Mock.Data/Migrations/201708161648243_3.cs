namespace Mock.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UploadEntry", "FId", "dbo.Upload");
            DropIndex("dbo.UploadEntry", new[] { "FId" });
            RenameColumn(table: "dbo.UserRole", name: "AppRoleId", newName: "RoleId");
            RenameColumn(table: "dbo.UserRole", name: "AppUserId", newName: "UserId");
            RenameIndex(table: "dbo.UserRole", name: "IX_AppRoleId", newName: "IX_RoleId");
            RenameIndex(table: "dbo.UserRole", name: "IX_AppUserId", newName: "IX_UserId");
            DropPrimaryKey("dbo.UserRole");
            CreateTable(
                "dbo.Article",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FTypeCode = c.String(maxLength: 50),
                        Title = c.String(maxLength: 50),
                        Keywords = c.String(maxLength: 200),
                        Source = c.String(maxLength: 200),
                        Excerpt = c.String(maxLength: 200),
                        Content = c.String(),
                        ViewHits = c.Int(),
                        CommentQuantity = c.Int(),
                        PointQuantity = c.Int(),
                        thumbnail = c.String(maxLength: 200),
                        IsAudit = c.Boolean(),
                        Recommend = c.Boolean(),
                        IsStickie = c.Boolean(),
                        CreatorUserId = c.Int(),
                        CreatorTime = c.DateTime(),
                        DeleteMark = c.Boolean(),
                        DeleteUserId = c.Int(),
                        DeleteTime = c.DateTime(),
                        LastModifyUserId = c.Int(),
                        LastModifyTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUser", t => t.CreatorUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PId = c.Int(nullable: false),
                        EnCode = c.String(maxLength: 50),
                        FullName = c.String(maxLength: 50),
                        IsTree = c.Boolean(),
                        Layers = c.Int(),
                        SortCode = c.Int(),
                        EnabledMark = c.Boolean(),
                        Description = c.String(maxLength: 200),
                        CreatorTime = c.DateTime(),
                        CreatorUserId = c.Int(),
                        LastModifyTime = c.DateTime(),
                        LastModifyUserId = c.Int(),
                        DeleteTime = c.DateTime(),
                        DeleteUserId = c.Int(),
                        DeleteMark = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ItemsDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FId = c.Int(nullable: false),
                        PId = c.Int(nullable: false),
                        ItemCode = c.String(maxLength: 50),
                        ItemName = c.String(maxLength: 50),
                        IsDefault = c.Boolean(),
                        Layers = c.Int(),
                        SortCode = c.Int(),
                        EnabledMark = c.Boolean(),
                        Remark = c.String(maxLength: 200),
                        CreatorTime = c.DateTime(),
                        CreatorUserId = c.Int(),
                        LastModifyTime = c.DateTime(),
                        LastModifyUserId = c.Int(),
                        DeleteTime = c.DateTime(),
                        DeleteUserId = c.Int(),
                        DeleteMark = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.FId, cascadeDelete: true)
                .Index(t => t.FId);
            
            CreateTable(
                "dbo.PointArticle",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArticleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        AddTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUser", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Article", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Review",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AId = c.Int(),
                        PId = c.Int(),
                        Text = c.String(maxLength: 500),
                        Ip = c.String(maxLength: 50),
                        Agent = c.String(maxLength: 50),
                        AddTime = c.DateTime(),
                        AuthorId = c.Int(),
                        Author = c.String(maxLength: 50),
                        AuEmail = c.String(maxLength: 50),
                        Status = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUser", t => t.AuthorId)
                .ForeignKey("dbo.Article", t => t.PId)
                .Index(t => t.PId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.RoleMenu",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        MenuId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppMenu", t => t.MenuId, cascadeDelete: true)
                .ForeignKey("dbo.AppRole", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.MenuId);
            
            CreateTable(
                "dbo.ThirdUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BindSource = c.String(maxLength: 50),
                        BindUserCode = c.String(maxLength: 50),
                        UserId = c.Int(),
                        AddTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUser", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.AppRole", "Remark", c => c.String(maxLength: 50));
            AddColumn("dbo.AppRole", "SortCode", c => c.Int());
            DropColumn("dbo.UserRole", "UserRoleId");
            AddColumn("dbo.UserRole", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Upload", "FId", c => c.Int());
            AddColumn("dbo.Upload", "Url", c => c.String(maxLength: 100));
            AddColumn("dbo.Upload", "FileName", c => c.String(maxLength: 50));
            AddColumn("dbo.Upload", "FileSize", c => c.String(maxLength: 50));
            AddColumn("dbo.Upload", "Type", c => c.String(maxLength: 20));
            AddColumn("dbo.Upload", "Mime", c => c.String(maxLength: 20));
            AddPrimaryKey("dbo.UserRole", "Id");
            CreateIndex("dbo.Upload", "CreatorUserId");
            AddForeignKey("dbo.Upload", "CreatorUserId", "dbo.AppUser", "Id");
            DropColumn("dbo.AppRole", "Guid");
            DropColumn("dbo.AppUser", "Guid");
            DropColumn("dbo.Upload", "AddTime");
            DropColumn("dbo.Upload", "UserName");
            DropColumn("dbo.Upload", "SortCode");
            DropTable("dbo.UploadEntry");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UploadEntry",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FId = c.Int(nullable: false),
                        AddTime = c.DateTime(),
                        Url = c.String(maxLength: 200),
                        FileName = c.String(maxLength: 50),
                        FileSize = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Upload", "SortCode", c => c.Int());
            AddColumn("dbo.Upload", "UserName", c => c.String(maxLength: 50));
            AddColumn("dbo.Upload", "AddTime", c => c.DateTime());
            AddColumn("dbo.AppUser", "Guid", c => c.String(maxLength: 50));
            AddColumn("dbo.AppRole", "Guid", c => c.String(maxLength: 50));
            DropForeignKey("dbo.Upload", "CreatorUserId", "dbo.AppUser");
            DropForeignKey("dbo.ThirdUser", "UserId", "dbo.AppUser");
            DropForeignKey("dbo.RoleMenu", "RoleId", "dbo.AppRole");
            DropForeignKey("dbo.RoleMenu", "MenuId", "dbo.AppMenu");
            DropForeignKey("dbo.Review", "PId", "dbo.Article");
            DropForeignKey("dbo.Review", "AuthorId", "dbo.AppUser");
            DropForeignKey("dbo.PointArticle", "ArticleId", "dbo.Article");
            DropForeignKey("dbo.PointArticle", "UserId", "dbo.AppUser");
            DropForeignKey("dbo.ItemsDetail", "FId", "dbo.Items");
            DropForeignKey("dbo.Article", "CreatorUserId", "dbo.AppUser");
            DropIndex("dbo.Upload", new[] { "CreatorUserId" });
            DropIndex("dbo.ThirdUser", new[] { "UserId" });
            DropIndex("dbo.RoleMenu", new[] { "MenuId" });
            DropIndex("dbo.RoleMenu", new[] { "RoleId" });
            DropIndex("dbo.Review", new[] { "AuthorId" });
            DropIndex("dbo.Review", new[] { "PId" });
            DropIndex("dbo.PointArticle", new[] { "UserId" });
            DropIndex("dbo.PointArticle", new[] { "ArticleId" });
            DropIndex("dbo.ItemsDetail", new[] { "FId" });
            DropIndex("dbo.Article", new[] { "CreatorUserId" });
            DropPrimaryKey("dbo.UserRole");
            DropColumn("dbo.Upload", "Mime");
            DropColumn("dbo.Upload", "Type");
            DropColumn("dbo.Upload", "FileSize");
            DropColumn("dbo.Upload", "FileName");
            DropColumn("dbo.Upload", "Url");
            DropColumn("dbo.Upload", "FId");
            DropColumn("dbo.UserRole", "Id");
            DropColumn("dbo.AppRole", "SortCode");
            DropColumn("dbo.AppRole", "Remark");
            DropTable("dbo.ThirdUser");
            DropTable("dbo.RoleMenu");
            DropTable("dbo.Review");
            DropTable("dbo.PointArticle");
            DropTable("dbo.ItemsDetail");
            DropTable("dbo.Items");
            DropTable("dbo.Article");
            RenameIndex(table: "dbo.UserRole", name: "IX_UserId", newName: "IX_AppUserId");
            RenameIndex(table: "dbo.UserRole", name: "IX_RoleId", newName: "IX_AppRoleId");
            RenameColumn(table: "dbo.UserRole", name: "UserId", newName: "AppUserId");
            RenameColumn(table: "dbo.UserRole", name: "RoleId", newName: "AppRoleId");
            CreateIndex("dbo.UploadEntry", "FId");
            AddForeignKey("dbo.UploadEntry", "FId", "dbo.Upload", "Id", cascadeDelete: true);
        }
    }
}
