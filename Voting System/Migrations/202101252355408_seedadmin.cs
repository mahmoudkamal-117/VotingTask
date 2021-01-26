namespace Voting_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seedadmin : DbMigration
    {
        public override void Up()
        {
            Sql(@"
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'b9065f05-942b-48cb-ae8f-d976b0f46231', N'Admin@vote.com', 0, N'AHffmf0514/dFMgbCySaCf8vzGpgbLEaoILjDDnPmxk/Rppic6u+VDmYDDEmpFFRrQ==', N'e29a6c3c-5aae-45db-8c82-9b6be9e096cf', NULL, 0, 0, NULL, 1, 0, N'Admin@vote.com')

");

        }
        
        public override void Down()
        {
        }
    }
}
