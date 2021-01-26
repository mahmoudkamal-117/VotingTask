using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Voting_System.Models
{
    public class VoteMain
    {
        public VoteMain()
        {
           voterDetails = new List<voterDetails>();
        }
        public int ID { get; set; }

        public string MainQuestion { get; set; }

        public int NumberOfVotes { get; set; }
        public virtual ICollection<voterDetails> voterDetails { get; set; }
    }
}