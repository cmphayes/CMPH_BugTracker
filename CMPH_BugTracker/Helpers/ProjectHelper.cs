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

namespace CMPH_BugTracker.Helpers
{
    public class ProjectsHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
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

        [ValidateAntiForgeryToken]
        public ICollection<Project> ListUserProjects(string UserId)
        {
            ApplicationUser user = db.Users.Find(UserId);
            var projects = user.Projects.ToList();
            return (projects);
        }

        public ICollection<ApplicationUser> UsersOnProject(int ProjectId)
        {
            return db.Projects.Find(ProjectId).Users;
        }

        public ICollection<ApplicationUser> UsersNotOnProject(int ProjectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != ProjectId)).ToList();
        }

        public ICollection<Project> ListUsersOnProject(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var projects = user.Projects.ToList();
            return (projects);
        }




    }
}