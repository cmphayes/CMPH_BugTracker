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
using System.IO;
using System.Threading.Tasks;
using CMPH_BugTracker.Extensions;

namespace CMPH_BugTracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projectHelper = new ProjectsHelper();
        private TicketsHelper ticketHelper = new TicketsHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();



        // GET: Tickets
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Index()
        {
            return View(db.Tickets.ToList());
        }

        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult MyTickets()
        {
            var userId = User.Identity.GetUserId();
            return View(ticketHelper.ListUserTickets(userId));
        }

        [Authorize(Roles = "Admin,Submitter")]
        public ActionResult CreatedTickets()
        {
            var userId = User.Identity.GetUserId();
            return View(ticketHelper.ListUserCreatedTickets(userId));
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
            var userId = User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).ToList().FirstOrDefault();
            switch (myRole)
            {
                case "ProjectManager":
                    if (ticket.AssignedUserId != userId && ticket.OwnerUserId != userId)
                        return RedirectToAction("ProfileView", "Account");
                    break;
                case "Developer":
                    if (ticket.AssignedUserId != userId)
                        return RedirectToAction("ProfileView", "Account");
                    break;
                case "Submitter":
                    if (ticket.OwnerUserId != userId)
                        return RedirectToAction("ProfileView", "Account");
                    break;
                default:
                    break;
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Admin,Submitter")]
        public ActionResult Create(int Id)
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Value");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Value");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Value");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Submitter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId, TicketPriorityId, TicketStatusId, TicketTypeId")] Ticket ticket, string Title, string Body, int Id)
        {
            if (ModelState.IsValid)
            {
                ticket.Title = Title;
                ticket.Body = Body;
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.Created = DateTimeOffset.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Details", "Projects", new { id = Id });
            }

            //ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedUserId);
            //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            //ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", ticket.ProjectId);
            //ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Id", ticket.TicketPriorityId);
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Id", ticket.TicketStatusId);
            //ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Id", ticket.TicketTypeId);
            //return View(ticket);

            //Load up a ViewData with some error message(s)
            if(string.IsNullOrEmpty(ticket.Title))
            {
                ViewData["TitleError"] = "The Title of a Ticket cannot be empty";
            }
            if(string.IsNullOrEmpty(ticket.Body))
            {
                ViewData["BodyError"] = "The Body of a Ticket cannot be empty";
            }
            return RedirectToAction("Details", "Projects", new { id = ticket.ProjectId });
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Edit(int? id)
        {
            var ticket = db.Tickets.Find(id);
            var userId = User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).ToList().FirstOrDefault();
            switch (myRole)
            {
                case "Developer":
                    if (ticket.AssignedUserId != userId)
                        return RedirectToAction("ProfileView", "Account");
                    break;
                case "Submitter":
                    if (ticket.OwnerUserId != userId)
                        return RedirectToAction("ProfileView", "Account");
                    break;
                case "Project Manager":
                    if (!projectHelper.IsUserOnProject(userId, ticket.ProjectId))
                        return RedirectToAction("ProfileView", "Account");
                    break;
                default:
                    break;
            }

            if (ticket == null)
            {
                return HttpNotFound();
            }

            var projectDevelopers = new List<ApplicationUser>();
            var projectUsers = projectHelper.ListUsersOnProject(ticket.ProjectId);
            foreach (var user in projectUsers)
            {
                if (roleHelper.IsUserInRole(user.Id, "Admin,Developer"))
                {
                    projectDevelopers.Add(user);
                }
            }
            ViewBag.Tickets = new MultiSelectList(db.Tickets.ToList(), "Id", "Title");
            ViewBag.Users = new MultiSelectList(db.Users.ToList(), "Id", "Email");
            ViewBag.AssignedUserId = new SelectList(projectDevelopers, "Id", "Email", ticket.AssignedUserId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Value", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Value", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Value", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> Edit([Bind(Include = "Id,Title,Body,Description,Created,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedUserId")] Ticket ticket)
        {
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

            if (ModelState.IsValid)
            {
                ticket.Updated = DateTimeOffset.Now;
                db.Tickets.Add(ticket);
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                if (string.IsNullOrEmpty(oldTicket.AssignedUserId) && !string.IsNullOrEmpty(ticket.AssignedUserId))
                {
                    ticket.TicketStatusId = db.TicketStatus.FirstOrDefault(t => t.Value == "Assigned").Id;
                }

                ticket.RecordChanges(oldTicket);
                await ticket.TriggerNotifications(oldTicket);
                return RedirectToAction("Details", "Tickets", new { id = ticket.Id });
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", ticket.ProjectId);
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedUserId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);

            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin,ProjectManager,Submitter")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userId = User.Identity.GetUserId();
            var ticket = db.Tickets.Find(id);
            var myRole = roleHelper.ListUserRoles(userId).ToList().FirstOrDefault();
            switch (myRole)
            {
                case "ProjectManager":
                    if (ticket.AssignedUserId != userId && ticket.OwnerUserId != userId)
                        return RedirectToAction("ProfileView", "Account");
                    break;
                case "Developer":
                    if (ticket.AssignedUserId != userId)
                        return RedirectToAction("ProfileView", "Account");
                    break;
                case "Submitter":
                    if (ticket.OwnerUserId != userId)
                        return RedirectToAction("ProfileView", "Account");
                    break;
                default:
                    break;
            }
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
