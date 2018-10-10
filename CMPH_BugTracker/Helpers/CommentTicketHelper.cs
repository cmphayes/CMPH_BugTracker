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
    [Authorize]
    public class TicketCommentsHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        [ValidateAntiForgeryToken]
        public bool IsUserOnTicketComment(string userId, int TicketCommentId)
        {
            var TicketComments = db.Projects.Find(TicketCommentId);
            var flag = TicketComments.Users.Any(u => u.Id == userId);
            return (flag);
        }

        public ICollection<TicketComment> ListUserCreatedTicketComments(string userId)
        {
            return db.TicketComments.Where(p => p.OwnerUserId == userId).ToList();
        }

        public ICollection<TicketComment> ListUserTicketComments(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var TicketComments = user.TicketComments.ToList();
            return (TicketComments);
        }

        public static string GetTicketCommentOwner(int TicketCommentId)
        {
            var none = "No TicketComment Owner Listed";
            var TicketCommentOwnerId = db.TicketComments.Find(TicketCommentId).OwnerUserId;
            var TicketCommentOwner = db.Users.Find(TicketCommentOwnerId).DisplayName;
            if (TicketCommentOwner == null)
            {
                return none;
            }
            if (string.IsNullOrEmpty(TicketCommentOwner))
            {
                return none;
            }
            return TicketCommentOwner;
        }
    }
}
 