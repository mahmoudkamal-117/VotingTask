namespace Voting_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnumberofvotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VoteMains", "NumberOfVotes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VoteMains", "NumberOfVotes");
        }
    }
}
