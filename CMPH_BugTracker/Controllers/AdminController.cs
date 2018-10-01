using CMPH_BugTracker.Helpers;
using CMPH_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMPH_BugTracker.Controllers
{
    public class AdminController : Controller

    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projectsHelper = new ProjectsHelper();
        private TicketsHelper ticketsHelper = new TicketsHelper();


        // GET: Admin
        public ActionResult RoleAssignment()
        {
            //Load up select list data structure into a view bag property
            ViewBag.Users = new SelectList(db.Users.ToList(), "Id", "Email");
            ViewBag.Roles = new SelectList(db.Roles.ToList(), "Name", "Name");
            ViewBag.Tickets = new SelectList(db.Tickets.ToList(), "Id", "Name");
            ViewBag.Projects = new SelectList(db.Projects.ToList(), "Id", "Name");


            return View();
        }

        [HttpPost]
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

            //Return
            return RedirectToAction("Index", "Home");

        }

        public ActionResult ProjectAssignment()
        {
            ViewBag.Projects = new SelectList(db.Projects.ToList(), "Id", "Name");
            ViewBag.Users = new MultiSelectList(db.Users.ToList(), "Name", "Name");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectAssignment(int projects, List<string> users)
        {
            //Remove any and all current role assignment
            var projectUsers = projectsHelper.UsersOnProject(projects);
            if (projectUsers.Count > 0)
            {
                foreach (var user in projectUsers)
                {
                    projectsHelper.RemoveUserFromProject(user.Id, projects);
                }
            }

            //Assign the selected role to the user
            foreach (var userId in users)
            {
                projectsHelper.AddUserToProject(userId, projects);
            }

            //Return
            return RedirectToAction("Index", "Home");

        }

        public ActionResult TicketAssignment()
        {
            ViewBag.Users = new SelectList(db.Users.ToList(), "Id", "FirstName");
            ViewBag.Tickets = new SelectList(db.Tickets.ToList(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TicketAssignment(int tickets, List<string> users)
        {
            //Remove any and all current role assignment
            var ticketUsers = ticketsHelper.UsersOnProject(tickets);
            if (ticketUsers.Count > 0)
            {
                foreach (var user in ticketUsers)
                {
                    ticketsHelper.RemoveUserFromTicket(user.Id, tickets);
                }
            }

            //Assign the selected role to the user

            foreach (var userId in users)
            {
                ticketsHelper.AddUserToTicket(userId, tickets);
            }

            //Return
            return RedirectToAction("Index", "Home");
            
        }
    }
}