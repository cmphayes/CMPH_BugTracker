using CMPH_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMPH_BugTracker.Helpers
{
    [Authorize]
    public static class UserHelper
    {
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