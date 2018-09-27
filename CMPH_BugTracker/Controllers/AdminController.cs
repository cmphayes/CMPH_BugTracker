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


        // GET: Admin
        public ActionResult RoleAssignment()
        {
            //Load up select list data structure into a view bag property
            ViewBag.Users = new SelectList(db.Users.ToList(), "Id", "Email");
            ViewBag.Roles = new SelectList(db.Roles.ToList(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAssignment(string Users, string Roles)
        {
            //Remove any and all current role assignment
            var currentRoles = roleHelper.ListUserRoles(Users);
            if (currentRoles.Count > 0)
            {
                foreach (var role in currentRoles)
                {
                    roleHelper.RemoveUserFromRole(Users, Roles);
                }
            }

            //Assign the selected role to the user

            roleHelper.AddUserToRole(Users, Roles);

            //Return
            return RedirectToAction("Index", "Home");

        }

        public ActionResult ProjectAssignment()
        {
            ViewBag.Projects = new SelectList(db.Projects.ToList(), "Id", "Name");
            ViewBag.Users = new MultiSelectList(db.Users.ToList(), "Id", "FirstName");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectAssignment(int Projects, List<string> Users)
        {
            //Remove any and all current role assignment
            var UsersOnProject = projectsHelper.UsersOnProject(Projects);
            if (UsersOnProject.Count > 0)
            {
                foreach (var user in UsersOnProject)
                {
                    projectsHelper.RemoveUserFromProject(user.Id, Projects);
                }
            }

            //Assign the selected role to the user
            foreach (var user in Users)
            {
                projectsHelper.AddUserToProject(user, Projects);
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
        public ActionResult TicketAssignment(string Users, string Tickets)
        {
            //Remove any and all current role assignment
            var currentTicket = roleHelper.ListUserRoles(Users);
            if (currentTicket.Count > 0)
            {
                foreach (var role in currentTicket)
                {
                    roleHelper.RemoveUserFromRole(Users, Tickets);
                }
            }

            //Assign the selected role to the user

            roleHelper.AddUserToRole(Users, Tickets);

            //Return
            return RedirectToAction("Index", "Home");

        }
    }
}