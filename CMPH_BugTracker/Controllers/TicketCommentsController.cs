using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CMPH_BugTracker.Helpers;
using CMPH_BugTracker.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;

namespace CMPH_BugTracker.Controllers
{
    [Authorize]
    public class TicketCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projectHelper = new ProjectsHelper();
        private TicketsHelper ticketHelper = new TicketsHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        // GET: TicketComments
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Index(int? page, string searchStr)
        {
            ViewBag.Search = searchStr; var TicketCommentsList = IndexSearch(searchStr);

            int pageSize = 5; // the number of posts you want to display per page             
            int pageNumber = (page ?? 1); 

            return View(TicketCommentsList.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public IQueryable<TicketComment> IndexSearch(string searchStr)
        {
            IQueryable<TicketComment> result = null;
            if (searchStr != null)

            { result = db.TicketComments.AsQueryable();
                result = result.Where(p => p.Title.Contains(searchStr)|| p.Body.Contains(searchStr));
            }
            else
            {
                result = db.TicketComments.AsQueryable();
            }

            return result.OrderByDescending(p => p.Created);
        }

        // GET: TicketComments/Details/5
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userId = User.Identity.GetUserId();
            TicketComment ticketComment = db.TicketComments.Find(id);

            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            return View(ticketComment);
        }

        // GET: TicketComments/Create
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Create()
        {
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            return View();
        }

        // POST: TicketComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketId")] TicketComment ticketComment, string ticketCommentTitle, string ticketCommentBody)
        {
            if (ModelState.IsValid)
            {
                ticketComment.Title = ticketCommentTitle;
                ticketComment.Body = ticketCommentBody;
                ticketComment.OwnerUserId = User.Identity.GetUserId();
                ticketComment.Created = DateTimeOffset.Now;
                db.TicketComments.Add(ticketComment);
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = ticketComment.TicketId });
            }

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComment.TicketId);
            return RedirectToAction("Details", "Tickets", new { id = ticketComment.TicketId });
        }

        // GET: TicketComments/Edit/5
        [Authorize(Roles = "Admin,ProjectManager,")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketComment = db.TicketComments.Find(id);
            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComment.TicketId);
            return View(ticketComment);
        }

        // POST: TicketComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,Body,Abstract,AuthorID,Created,Updated")] TicketComment ticketComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComment.TicketId);
            return View(ticketComment);
        }

        // GET: TicketComments/Delete/5
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketComment = db.TicketComments.Find(id);
            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            return View(ticketComment);
        }

        // POST: TicketComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketComment ticketComment = db.TicketComments.Find(id);
            db.TicketComments.Remove(ticketComment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
