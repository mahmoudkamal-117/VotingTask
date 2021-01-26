using Voting_System.Models;
using Voting_System.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Voting_System.Controllers
{
    public class PollsVotesController : Controller
    {
        private readonly PollsVotesRepository _repo = new PollsVotesRepository();
        private readonly PollsRepository repo_polls = new PollsRepository();

        // GET: PollsVotes
        public ActionResult Index(int? page, int? Id)
        {
            AccessHelper.AuthorizeAction("الإقتراعات");

            if (Id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.PollId = Id;
            //TempData["PollId"] = Id;
            var poll = repo_polls.GetPoll(Id.Value);
            ViewBag.PollTitle = poll.Title;

            int pageNo = (page ?? 1);
            int startPosition = (pageNo - 1) * Properties.Settings.Default.PageSize;
            ViewBag.ItemsCount = _repo.VotesCount(Id.Value);
            ViewBag.PagesCount = MathHelper.GetPageCount(ViewBag.ItemsCount);
            ViewBag.PageNo = pageNo;

            TempData["PageNo"] = pageNo;
            if (pageNo > ViewBag.PagesCount && ViewBag.PagesCount != 0)
                return RedirectToAction("Index", new { page = ViewBag.PagesCount });
            else if (pageNo < 1 && ViewBag.PagesCount != 0)
                return RedirectToAction("Index", new { page = 1 });

            if (Session["PollsVotesSortColumn"] == null)
            {
                Session["PollsVotesSortColumn"] = "OrderIndex";
                Session["PollsVotesSortOrder"] = "asc";
            }

            var pollsVote = _repo.GetPollsVotes(startPosition, Session["PollsVotesSortColumn"].ToString(), Session["PollsVotesSortOrder"].ToString(), Id.Value);
            return View(pollsVote);
        }

        public ActionResult Create(int id)
        {
            AccessHelper.AuthorizeAction("الإقتراعات_A");

            ViewBag.PollId = id;
            var poll = repo_polls.GetPoll(id);
            ViewBag.PollTitle = poll.Title;
            ViewBag.OrderIndex = _repo.GetNextOrderIndex(id);
            ViewBag.PollsVotes = _repo.GetPollsVotes(id).Select(p => p.Answer).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PollsVote pollsVote, int id, HttpPostedFileBase file, string addButton)
        {
            AccessHelper.AuthorizeAction("الإقتراعات_A");

            if (ModelState.IsValid)
            {
                pollsVote.HasImage = (file != null);
                pollsVote.PollId = id;
                pollsVote.Count = 0;
                int num = _repo.AddPollVote(pollsVote);

                if (file != null)
                    FileHelper.UploadImage(file, "PollsVotes", pollsVote.Id);
                if (!ErrorHelper.HasError())
                {
                    if (addButton == "إضافة وإنهاء")
                        return RedirectToAction("Index", "PollsVotes", new { Id = id });
                    else
                        return RedirectToAction("Create");
                }
            }
            return View(pollsVote);
        }

        // GET: PollsVote/Edit/5
        public ActionResult Edit(int? id)
        {
            AccessHelper.AuthorizeAction("الإقتراعات_U");

            if (id == null)
                return Redirect(ErrorHelper.ShowError(HttpStatusCode.BadRequest));

            var pollsVote = _repo.GetPollVote(id.Value);
            if (pollsVote == null)
                return Redirect(ErrorHelper.ShowError(HttpStatusCode.NotFound));

            if (Request.QueryString["ImageDeleted"] != null)
                pollsVote.HasImage = false;

            var poll = repo_polls.GetPoll(pollsVote.PollId);
            ViewBag.PollTitle = poll.Title;
            ViewBag.PollsVotes = _repo.GetPollsVotes(pollsVote.PollId).Select(p => p.Answer).ToList();
            return View(pollsVote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PollsVote pollsVote, HttpPostedFileBase file)
        {
            AccessHelper.AuthorizeAction("الإقتراعات_U");

            if (ModelState.IsValid)
            {
                pollsVote.HasImage = (file != null || pollsVote.HasImage);
                _repo.UpdatePollVote(pollsVote);

                if (file != null)
                    FileHelper.UploadImage(file, "PollsVotes", pollsVote.Id);
                if (!ErrorHelper.HasError())
                    return RedirectToAction("Index", "PollsVotes", new { Id = pollsVote.PollId });
            }
            return View(pollsVote);
        }

        // GET: PollsVotes/Delete/5
        public ActionResult Delete(int? id)
        {
            AccessHelper.AuthorizeAction("الإقتراعات_D");

            if (id == null)
                return Redirect(ErrorHelper.ShowError(HttpStatusCode.BadRequest));

            var pollVote = _repo.GetPollVote(id.Value);
            if (pollVote == null)
                return Redirect(ErrorHelper.ShowError(HttpStatusCode.NotFound));

            if (!_repo.DeletePollVote(id.Value))
                return Redirect(ErrorHelper.ShowError(HttpStatusCode.NotFound));

            if (pollVote.HasImage == true)
                FileHelper.DeleteImage("PollsVotes", pollVote.Id);

            return RedirectToAction("Index", "PollsVotes", new { page = Convert.ToInt32(TempData["PageNo"]), Id = pollVote.PollId });
        }

        [HttpPost]
        public ActionResult DeleteSelected(string[] selectedIds)
        {
            AccessHelper.AuthorizeAction("الإقتراعات_D");

            foreach (var id in selectedIds)
            {
                var pollVote = _repo.GetPollVote(Convert.ToInt32(id));
                if (pollVote.HasImage == true)
                    FileHelper.DeleteImage("PollsVotes", pollVote.Id);
            }

            _repo.DeletePollVotes(selectedIds);
            return Json(new { pageNo = Convert.ToInt32(TempData["PageNo"]) });
        }

        // GET: PollsVotes/Sort?sort=Name
        public ActionResult Sort(string sort, int id)
        {
            SortHelper.Sort("PollsVotes", sort);
            return RedirectToAction("Index", new { Id = id });
        }

        [HttpPost]
        public ActionResult Filter(string filterAnswer, string filterHasImage, string filterOrderIndexFrom, string filterOrderIndexTo,
                                  string filterCountFrom, string filterCountTo, int? id, int? page)
        {
            AccessHelper.AuthorizeAction("الإقتراعات");

            var filter = new Dictionary<string, object>
            {
                { "Answer", filterAnswer },
                { "HasImage", filterHasImage },
                { "CountFrom", filterCountFrom },
                { "CountTo", filterCountTo },
                { "OrderIndexFrom", filterOrderIndexFrom },
                { "OrderIndexTo", filterOrderIndexTo }
            };

            Session["PollsVoteFilter"] = filter;

            return RedirectToAction("Index", new { Id = id });
        }

        public ActionResult ClearFilter(int? id)
        {
            Session["PollsVoteFilter"] = null;
            return RedirectToAction("Index", new { Id = id });
        }

    }
}
