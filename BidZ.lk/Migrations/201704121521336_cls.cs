namespace BidZ.lk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cls : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RegisterViewModels",
                c => new
                    {
                        Email = c.String(nullable: false, maxLength: 128),
                        userFname = c.String(nullable: false),
                        userLname = c.String(nullable: false),
                        address = c.String(nullable: false),
                        userContactNo = c.String(nullable: false),
                        commercialName = c.String(),
                        commercialAddress = c.String(),
                        commercialEmail = c.String(),
                        Password = c.String(nullable: false, maxLength: 100),
                        ConfirmPassword = c.String(),
                        userType = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Email);
            
            AddColumn("dbo.AspNetUsers", "userFname", c => c.String());
            AddColumn("dbo.AspNetUsers", "userLname", c => c.String());
            AddColumn("dbo.AspNetUsers", "address", c => c.String());
            AddColumn("dbo.AspNetUsers", "userContactNo", c => c.String());
            AddColumn("dbo.AspNetUsers", "commercialName", c => c.String());
            AddColumn("dbo.AspNetUsers", "commercialAddress", c => c.String());
            AddColumn("dbo.AspNetUsers", "commercialEmail", c => c.String());
            AddColumn("dbo.AspNetUsers", "userType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "userType");
            DropColumn("dbo.AspNetUsers", "commercialEmail");
            DropColumn("dbo.AspNetUsers", "commercialAddress");
            DropColumn("dbo.AspNetUsers", "commercialName");
            DropColumn("dbo.AspNetUsers", "userContactNo");
            DropColumn("dbo.AspNetUsers", "address");
            DropColumn("dbo.AspNetUsers", "userLname");
            DropColumn("dbo.AspNetUsers", "userFname");
            DropTable("dbo.RegisterViewModels");
        }
    }
}
