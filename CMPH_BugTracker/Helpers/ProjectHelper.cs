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
        public bool IsUserOnProject(string User, int ProjectId)
        {
            var project = db.Projects.Find(ProjectId);
            var flag = project.Users.Any(u => u.Id == User);
            return (flag);
        }


        [ValidateAntiForgeryToken]

        public void AddUserToProject(string UserId, int ProjectId)
        {
            if (!IsUserOnProject(UserId, ProjectId))
            {
                Project proj = db.Projects.Find(ProjectId);
                var newUser = db.Users.Find(UserId);

                proj.Users.Add(newUser);
                db.SaveChanges();
            }
        }

        [ValidateAntiForgeryToken]
        public void RemoveUserFromProject(string UserId, int ProjectId)
        {
            if (IsUserOnProject(UserId, ProjectId))
            {
                Project proj = db.Projects.Find(ProjectId);
                var newUser = db.Users.Find(UserId);

                proj.Users.Remove(newUser);
                db.SaveChanges();
            }
        }

        [ValidateAntiForgeryToken]
        public ICollection<Project> ListUserProjects(int User)
        {
            ApplicationUser user = db.Users.Find(User);
            var projects = user.Projects.ToList();
            return (projects);
        }

        [ValidateAntiForgeryToken]
        public ICollection<ApplicationUser> UsersOnProject(int ProjectId)
        {
            return db.Projects.Find(ProjectId).Users;
        }

        [ValidateAntiForgeryToken]
        public ICollection<ApplicationUser> UsersNotOnProject(int ProjectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != ProjectId)).ToList();

        }

        //        [ValidateAntiForgeryToken]
        //public ICollection<ApplicationUser> ListUsersOnProject(int User)
        //{
        //    ApplicationUser User = db.Users.Find(User);
        //    var projects = User.Projects.ToList();
        //    return (projects);
        //}

        //        [ValidateAntiForgeryToken]

        //public ICollection<ApplicationUser> ListProjectsForUser(int Projects)
        //{
        //    ApplicationUser user = db.Users.Find(Projects);
        //    var users = user.Projects.ToList();
        //    return (users);
        //}

        //        [ValidateAntiForgeryToken]

        //public ICollection<ApplicationUser> ListProjects(int User)
        //{
        //    ApplicationUser user = db.Users.Find(User);
        //    var projects = user.Projects.ToList();
        //    return (projects);
        //}
    }
}