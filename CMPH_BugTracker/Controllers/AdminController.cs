using CMPH_BugTracker.Helpers;
using CMPH_BugTracker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CMPH_BugTracker.Controllers;
using CMPH_BugTracker.Extensions;
using System.Threading.Tasks;

namespace CMPH_BugTracker.Controllers
{
    [Authorize]
    public class AdminController : Controller

    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projectsHelper = new ProjectsHelper();
        private TicketsHelper ticketsHelper = new TicketsHelper();


        // GET: Admin
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult RoleAssignment()
        {
            //Load up select list data structure into a view bag property
            ViewBag.Users = new SelectList(db.Users.ToList(), "Id", "Email");
            ViewBag.Roles = new SelectList(db.Roles.ToList(), "Name", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,ProjectManager")]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAssignment(string users, string roles)
        {
            //Remove any and all current role assignment
            var currentRoles = roleHelper.ListUserRoles(users);
            if (currentRoles.Count > 0)
            {
                foreach (var role in currentRoles)
                {
                    roleHelper.RemoveUserFromRole(users, role);
                }
            }
            //Assign the selected role to the user
            roleHelper.AddUserToRole(users, roles);
            ////Update cookie 
            //SignInManager.SignIn(users false false)
            //Return

            return RedirectToAction("ProfileView", "Account");
        }

        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult ProjectAssignment()
        {
            ViewBag.Projects = new MultiSelectList(db.Projects.ToList(), "Id", "Title");
            ViewBag.Users = new MultiSelectList(db.Users.ToList(), "Id", "Email");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,ProjectManager")]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectAssignment(int projects, List<string> users)
        {
            //Remove any and all current role assignment
            var projectUsers = projectsHelper.ListUsersOnProject(projects);
            if (projectUsers.Count > 0)
            {
                foreach (var user in projectUsers.ToList())
                {
                    projectsHelper.RemoveUserFromProject(user.Id, projects);
                }
            }        
            foreach (var userId in users)
            {
                projectsHelper.AddUserToProject(userId, projects);
            }
            return RedirectToAction("Index", "Projects");
        }

        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult TicketAssignment()
        {
            ViewBag.Tickets = new MultiSelectList(db.Tickets.ToList(), "Id", "Title");
            ViewBag.Users = new MultiSelectList(db.Users.ToList(), "Id", "Email");
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin,ProjectManager")]
        //public async Task<ActionResult> TicketAssignment(int ticket, string users, Ticket oldTicket)
        //{
        //    //var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

        //        var ticketUsers = ticketsHelper.ListUsersOnTicket(ticket);
        //        if (ticketUsers.Count > 0)
        //        {
        //            foreach (var user in ticketUsers)
        //            {
        //                ticketsHelper.RemoveUserFromTicket(user.Id, ticket);
        //            }
        //        }
        //        ticketsHelper.AddUserToTicket(users, ticket);

        //        TicketExtensions.RecordChanges(string ticket, oldTicket);
        //        await TicketNotificationExtensions.TriggerNotifications(ticket, oldTicket);


        //    return RedirectToAction("ProfileView", "Account");
        //}
    }
}