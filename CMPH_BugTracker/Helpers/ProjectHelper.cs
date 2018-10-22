using CMPH_BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

using CMPH_BugTracker.Helpers;


namespace CMPH_BugTracker.Helpers
{
    [Authorize]
    public class ProjectsHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        [ValidateAntiForgeryToken]
        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var flag = project.Users.Any(u => u.Id == userId);
            return (flag);
        }

        [ValidateAntiForgeryToken]
        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var newUser = db.Users.Find(userId);

                proj.Users.Add(newUser);
                db.SaveChanges();
            }
        }

        [ValidateAntiForgeryToken]
        public void RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var delUser = db.Users.Find(userId);

                proj.Users.Remove(delUser);
                db.Entry(proj).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ICollection<Project> ListUserProjects(string userId)
        {
            
            ApplicationUser user = db.Users.Find(userId);
            var projects = user.Projects.ToList();
            return (projects);
        }

        public ICollection<Project> ListUserCreatedProjects(string userId)
        {
            return db.Projects.Where(p => p.OwnerUserId == userId).ToList();
        }

        //partials

        public ICollection<Project> ListUserProjectsPartial(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var projects = user.Projects.ToList();
            return (projects);
        }

        public ICollection<Project> ListUserCreatedProjectsPartial(string userId)
        {
            return db.Projects.Where(p => p.OwnerUserId == userId).ToList();
        }

        public ICollection<ApplicationUser> ListUsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Users;
        }

        public ICollection<ApplicationUser> ListUsersNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }

        //public ICollection<Project> ListUsersOnProject(string userId)
        //{
        //    ApplicationUser user = db.Users.Find(userId);
        //    var projects = user.Projects.ToList();
        //    return (projects);
        //}


        public static List<Ticket> TicketsOnProject(int id)
        {
            return db.Tickets.Where(t => t.ProjectId == id).ToList();
        }

        public static string GetProjectOwner(int projectId)
        {
            var none = "No Project Owner Listed";
            var projectOwnerId = db.Projects.Find(projectId).OwnerUserId;
            var projectOwner = db.Users.Find(projectOwnerId).DisplayName;
            if (projectOwner == null)
            {
                return none;
            }
            if (string.IsNullOrEmpty(projectOwner))
            {
                return none;
            }
            return projectOwner;
        }
    }
}