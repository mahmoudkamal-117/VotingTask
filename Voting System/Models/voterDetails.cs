using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Voting_System.Models
{
    public class voterDetails
    {
        public int ID { get; set; }
        public int MainID { get; set; }

        public int VoteCount { get; set; }
        public string Answer { get; set; }
        [ForeignKey("MainID")]
        public virtual VoteMain VoteMain { get; set; }
    }

}