using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using System.Web.Mvc;
using Voting_System.Models;

namespace Voting_System.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext _context;
        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            return View(_context.VoteMain.ToList());
        }

        [HttpPost]
      
        public JsonResult Vote(int? QuestionId,int? AnswerID)
        {
            try
            {
                if (QuestionId == null || AnswerID == null)
                {
                    return Json( "Somthing Wrong Try Again Later");
                }
                var voted = Voting((int)QuestionId, (int)AnswerID);
                if (voted == 1)
                {
                    return Json( "Voted Succesfully");
                }
                else
                {
                    return Json("Warning you Voted Before");
                }
            }
            catch (Exception)
            {

                return Json("Somthing Wrong Try Again Later");
            }
            
            
        }



        public int Voting(int pollVoteId,int PollDetail)
        {
            // Increment Poll's Vote count if it is not answered before in the current browser
           
                var pollVote = _context.VoteMain.
                   AsNoTracking().
                   Where(pv => pv.ID == pollVoteId).
                   FirstOrDefault();

                if (pollVote != null)
                {
                    var pollId = pollVote.ID;
                    if (System.Web.HttpContext.Current.Request.Cookies["PollId_" + pollId]?.Value != "voted")
                    {
                    var gg= System.Web.HttpContext.Current.Response.Cookies["PollId_" + pollId].Values;
                    pollVote.NumberOfVotes++;
                    var det = pollVote.voterDetails.FirstOrDefault(s => s.ID == PollDetail);
                    if(det != null)
                    {
                       det.VoteCount ++;
                    }
                        _context.Entry(pollVote).State = EntityState.Modified;
                    _context.Entry(det).State = EntityState.Modified;
                    _context.SaveChanges();

                        var pollCookie = new System.Web.HttpCookie("PollId_" + pollId, "voted");
                        pollCookie.Expires = DateTime.Now.AddMonths(1);
                    System.Web.HttpContext.Current.Response.Cookies.Add(pollCookie);
                    return 1;
                    }
                    else
                    {
                    return 0;
                    }
                }
            return 0;
           
            
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}