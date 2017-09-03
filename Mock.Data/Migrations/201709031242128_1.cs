namespace Mock.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AppMenu", newName: "AppModule");
            DropForeignKey("dbo.RoleMenu", "MenuId", "dbo.AppMenu");
            DropForeignKey("dbo.RoleMenu", "RoleId", "dbo.AppRole");
            DropIndex("dbo.RoleMenu", new[] { "RoleId" });
            DropIndex("dbo.RoleMenu", new[] { "MenuId" });
            CreateTable(
                "dbo.RoleModule",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        ModuleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppModule", t => t.ModuleId, cascadeDelete: true)
                .ForeignKey("dbo.AppRole", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.ModuleId);
            
            AddColumn("dbo.AppModule", "EnCode", c => c.String(maxLength: 50));
            DropTable("dbo.RoleMenu");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RoleMenu",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        MenuId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.RoleModule", "RoleId", "dbo.AppRole");
            DropForeignKey("dbo.RoleModule", "ModuleId", "dbo.AppModule");
            DropIndex("dbo.RoleModule", new[] { "ModuleId" });
            DropIndex("dbo.RoleModule", new[] { "RoleId" });
            DropColumn("dbo.AppModule", "EnCode");
            DropTable("dbo.RoleModule");
            CreateIndex("dbo.RoleMenu", "MenuId");
            CreateIndex("dbo.RoleMenu", "RoleId");
            AddForeignKey("dbo.RoleMenu", "RoleId", "dbo.AppRole", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RoleMenu", "MenuId", "dbo.AppMenu", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.AppModule", newName: "AppMenu");
        }
    }
}
