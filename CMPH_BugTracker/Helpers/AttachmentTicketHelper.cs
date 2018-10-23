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
    public class TicketAttachmentsHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        [ValidateAntiForgeryToken]
        public bool IsUserOnTicketAttachment(string userId, int TicketAttachmentId)
        {
            var TicketAttachments = db.Projects.Find(TicketAttachmentId);
            var flag = TicketAttachments.Users.Any(u => u.Id == userId);
            return (flag);
        }

        public ICollection<TicketAttachment> ListUserCreatedTicketAttachments(string userId)
        {
            return db.TicketAttachments.Where(p => p.OwnerUserId == userId).ToList();
        }

        public ICollection<TicketAttachment> ListUserTicketAttachments(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var TicketAttachments = user.TicketAttachments.ToList();
            return (TicketAttachments);
        }

        public static string GetTicketAttachmentOwner(int TicketAttachmentId)
        {
            var none = "No TicketAttachment Owner Listed";
            var TicketAttachmentOwnerId = db.TicketAttachments.Find(TicketAttachmentId).OwnerUserId;
            var TicketAttachmentOwner = db.Users.Find(TicketAttachmentOwnerId).DisplayName;
            if (TicketAttachmentOwner == null)
            {
                return none;
            }
            if (string.IsNullOrEmpty(TicketAttachmentOwner))
            {
                return none;
            }
            return TicketAttachmentOwner;
        }

        //[Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        //Im going to change this to a switch statement
        public static string GetUploadIcon(int ticketAttachmentId)
        {
            var defaultIcon = "/img/if_url_65946.png";
            var ticketAttachmentPath = db.TicketAttachments.Find(ticketAttachmentId).FilePath;

            string fileExt = System.IO.Path.GetExtension(ticketAttachmentPath);

            if (fileExt == ".jpeg")
            {
                return "/img/if_jpeg_65908.png";
            }
            else if (fileExt == ".pdf")
            {
                return "/img/if_pdf_65920.png";
            }
            else if (fileExt == ".doc")
            {
                return "/img/if_docx_win_65892.png";
            }
            else if (fileExt == ".png")
            {
                return "/img/if_png_65922.png";
            }
            else if (fileExt == ".rar")
            {
                return "/img/if_rar_65936.png";
            }
            else
            {
                return defaultIcon;
            }
        }
    }
}
 