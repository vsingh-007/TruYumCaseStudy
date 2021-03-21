namespace truYum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MenuItems_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MenuItems", t => t.MenuItems_ID)
                .Index(t => t.MenuItems_ID);
            
            CreateTable(
                "dbo.MenuItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Price = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        DateOfLaunch = c.DateTime(nullable: false),
                        FreeDelivery = c.Boolean(nullable: false),
                        Categories_CategoryID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.Categories_CategoryID)
                .Index(t => t.Categories_CategoryID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carts", "MenuItems_ID", "dbo.MenuItems");
            DropForeignKey("dbo.MenuItems", "Categories_CategoryID", "dbo.Categories");
            DropIndex("dbo.MenuItems", new[] { "Categories_CategoryID" });
            DropIndex("dbo.Carts", new[] { "MenuItems_ID" });
            DropTable("dbo.Categories");
            DropTable("dbo.MenuItems");
            DropTable("dbo.Carts");
        }
    }
}
