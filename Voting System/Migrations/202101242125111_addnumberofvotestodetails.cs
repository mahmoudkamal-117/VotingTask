namespace Voting_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnumberofvotestodetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.voterDetails", "VoteCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.voterDetails", "VoteCount");
        }
    }
}
