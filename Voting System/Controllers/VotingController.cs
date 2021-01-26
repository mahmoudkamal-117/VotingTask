using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Voting_System.Models;

namespace Voting_System.Controllers
{
    public class VotingController : Controller
    {
        private ApplicationDbContext _context;
        public VotingController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Voting
        public ActionResult Index()
        {

            var allvotes = _context.VoteMain.ToList();

            return View(allvotes);
        }


        public ActionResult GetallVotes()
        {
          var all=  _context.VoteMain.ToList();

            return Json(new  { all = all }) ;
        }

        public ActionResult AddVote()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult AddVote(string MainQ,string ValuesDetails)
        {

            try
            {
                var detailslist = ValuesDetails.Split(',');
                var model = new VoteMain();

                model.MainQuestion = MainQ;

                model.NumberOfVotes = 0;

                foreach (var item in detailslist)
                {
                    if (item != "")
                    {
                        model.voterDetails.Add(new voterDetails
                        {
                            Answer = item,
                            VoteCount=0
                        });
                    }

                }
                _context.VoteMain.Add(model);
                _context.SaveChanges();

                return Json("Added Succefuly");
            }
            catch (Exception)
            {

                return Json("something wrong");
            }
            
        }


        public async Task<ActionResult> delete(int ID)
        {

            try
            {
                var gettodelete = _context.VoteMain.FirstOrDefault(d => d.ID == ID);
                if (gettodelete != null)
                {
                    var details = gettodelete.voterDetails.ToList();
                    _context.voterDetails.RemoveRange(details);
                   await _context.SaveChangesAsync();
                    _context.VoteMain.Remove(gettodelete);
                  await  _context.SaveChangesAsync();

                    return Json("Delete success");
                }
                else
                {
                    return Json("something wrong");
                }

            }
            catch (Exception)
            {

                return Json("something wrong");
            }

         
        }
    }
}