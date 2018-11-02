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
    [Authorize]
    public class UserHelper
    {
        private UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private static ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public static string GetProfileImagePath()
        {
                var userId = HttpContext.Current.User.Identity.GetUserId();
                var profileImagePath = db.Users.Find(userId).ProfileImagePath;

                var defaultPath = "~/Uploads/DefaultProfilePic.jpg";
            //if (string.IsNullOrEmpty(userId))
            //    return defaultPath;

            if (string.IsNullOrEmpty(profileImagePath))
            {
                return defaultPath;
            }
                return profileImagePath;
        }

        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public static string GetName(string userId)
        {
            var defaultName = "Guest";
            var user = db.Users.Find(userId).DisplayName;
            if (user == null)
            {
                return defaultName;
            }
            if (string.IsNullOrEmpty(user))
            {
                return user;
            }
            return user;
        }

        [ValidateAntiForgeryToken]
        public ICollection<TicketNotification> ListUserNotifications(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var ticketNotifications = user.TicketNotifications.ToList();
            return (ticketNotifications);
        }

        [ValidateAntiForgeryToken]
        public ICollection<TicketNotification> ListUserUnreadTicketNotifications(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var ticketNotifications = user.TicketNotifications.Where(t => t.Read == false).ToList();
            return (ticketNotifications);
        }

        [ValidateAntiForgeryToken]
        public ICollection<TicketNotification> ListUserReadTicketNotifications(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var ticketNotifications = user.TicketNotifications.Where(t => t.Read == true).ToList();
            return (ticketNotifications);
        }

    }
}