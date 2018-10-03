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
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();
        private TicketsHelper ticketHelper = new TicketsHelper();


        // GET: Tickets
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(c => c.Title).Include(c => c.Body).Include(c => c.OwnerUserId).Include(c => c.Created).Include(c => c.AssignedUserId);
            return View(tickets.ToList());
        }

        //public ActionResult MyTickets()
        //{
        //    var userId = User.Identity.GetUserId();
        //    var myRole = roleHelper.ListUserRoles(userId);
        //    var myTickets = new List<Ticket>();

        //    switch (myRole.FirstOrDefault())
        //    {
        //        case "ProjectManager":
        //            myTickets = db.Tickets.Where(t => t.AssignedUserId == userId.ToList<Ticket>());
        //            break;
        //        case "Developer":
        //            myTickets = db.Tickets.Where(t => t.AssignedUserId = userId.ToList<Ticket>());
        //            break;
        //        case "Submitter":
        //            myTickets = db.Tickets.Where(t => t.OwnerUserId = userId.ToList<Ticket>());
        //            break;
        //        case "Admin":
        //            myTickets = db.Tickets.Where(t => t.AssignedUserId = userId.ToList<Ticket>());
        //            break;
        //    }
        //    return View(projectHelper.ListUserProjects(userId));

        //}


        [HttpPost]
        public IQueryable<Ticket> IndexSearch(string searchStr)
        {
            IQueryable<Ticket> result = null; if (searchStr != null) { result = db.Tickets.AsQueryable(); result = result.Where(p => p.Title.Contains(searchStr) 
            || p.Body.Contains(searchStr) 
            || p.TicketComments.Any(c => c.Body.Contains(searchStr)));
            }
            else
            {
                result = db.Tickets.AsQueryable();
            }
            return result.OrderByDescending(p => p.Created);
        }

        // GET: Tickets/Details/5
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Id");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Id");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Id");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Create([Bind(Include = "Id,ProjectId,TicketPriorityId,TicketStatusId,TicketTypeId,AssignedUserID,OwnerUserID,Created,Updated,Title,Body")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Id", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Id", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Id", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Id", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Id", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Id", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,TicketPriorityId,TicketStatusId,TicketTypeId,AssignedUserID,OwnerUserID,Created,Updated,Title,Body")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Id", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Id", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Id", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
