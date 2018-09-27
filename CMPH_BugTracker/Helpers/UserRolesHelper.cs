using CMPH_BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMPH_BugTracker.Helpers
{
    public class UserRolesHelper
    {
        private UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        [ValidateAntiForgeryToken]
        public bool IsUserInRole(string UserId, string RoleName)
        {
            return UserManager.IsInRole(UserId, RoleName);
        }

        [ValidateAntiForgeryToken]
        public bool AddUserToRole(string Users, string Roles)
        {
            var result = UserManager.AddToRole(Users, Roles);
            return result.Succeeded;
        }

        [ValidateAntiForgeryToken]
        public bool RemoveUserFromRole(string UserId, string RoleName)
        {
            var result = UserManager.RemoveFromRole(UserId, RoleName);
            return result.Succeeded;
        }


        [ValidateAntiForgeryToken]
        public ICollection<string> ListUserRoles(string UserId)
        {
            return UserManager.GetRoles(UserId);
        }


        [ValidateAntiForgeryToken]
        public ICollection<ApplicationUser> UsersInRole(string RoleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = UserManager.Users.ToList();
            foreach (var user in List) { if (IsUserInRole(user.Id, RoleName)) resultList.Add(user); }

            return resultList;
        }

        [ValidateAntiForgeryToken]
        public ICollection<ApplicationUser> UsersNotInRole(string RoleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = UserManager.Users.ToList();
            foreach (var user in List) { if (!IsUserInRole(user.Id, RoleName)) resultList.Add(user); }

            return resultList;
        }
    }
}