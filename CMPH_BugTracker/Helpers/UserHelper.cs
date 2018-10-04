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

        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public static string GetProfileImagePath(string userId)
            {
                var defaultPath = "/Uploads/default.jpg";
                if (string.IsNullOrEmpty(userId))
                    return defaultPath;

                var profileImagePath = db.Users.Find(userId).ProfileImagePath;
                if (string.IsNullOrEmpty(profileImagePath))
                    return profileImagePath;

                return profileImagePath;
            }

        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public static string GetName(string userId)
        {
            var defaultName = "User";
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
    }




    //public class GetImageHelper
    //{
    //    ApplicationDbContext db = new ApplicationDbContext();
    //    [ValidateAntiForgeryToken]
    //    public ActionResult GetImage(string userId)
    //    {
    //        if (userId == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        ApplicationUser user = db.Users.FirstOrDefault(a => a.Id == userId);
    //        if (user.ProfileImage == null)
    //        {
    //            return ();
    //        }
    //        return View(user.ProfileImage);
    //    }
    //}
}