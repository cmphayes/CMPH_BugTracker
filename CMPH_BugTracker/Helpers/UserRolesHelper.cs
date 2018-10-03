using CMPH_BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CMPH_BugTracker.Helpers
{
    [Authorize]
    public class UserRolesHelper
    {
        private UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserInRole(string userId, string roleName)
        {
            return UserManager.IsInRole(userId, roleName);
        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var result = UserManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = UserManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }
        
        public ICollection<ApplicationUser> UsersInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = UserManager.Users.ToList();
            foreach (var User in List) { if (IsUserInRole(User.Id, roleName)) resultList.Add(User); }

            return resultList;
        }

        public ICollection<ApplicationUser> UsersNotInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = UserManager.Users.ToList();
            foreach (var User in List)
            {
                if (!IsUserInRole(User.Id, roleName))
                    resultList.Add(User);
            }

            return resultList;
        }

        public ICollection<string> ListUserRoles(string userId)
        {
            return UserManager.GetRoles(userId);
        }

    }
}


