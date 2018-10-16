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
    public class TicketsHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        [ValidateAntiForgeryToken]
        public bool IsUserOnTicket(string userId, int ticketId)
        {
            var tickets = db.Projects.Find(ticketId);
            var flag = tickets.Users.Any(u => u.Id == userId);
            return (flag);
        }

        [ValidateAntiForgeryToken]
        public void AddUserToTicket(string userId, int ticketId)
        {
            if (!IsUserOnTicket(userId, ticketId))
            {
                Ticket tick = db.Tickets.Find(ticketId);
                var newUser = db.Users.Find(userId);

                tick.Users.Add(newUser);
                db.SaveChanges();
            }
        }

        [ValidateAntiForgeryToken]
        public void RemoveUserFromTicket(string userId, int ticketId)
        {
            if (IsUserOnTicket(userId, ticketId))
            {
                Ticket tick = db.Tickets.Find(ticketId);
                var delUser = db.Users.Find(userId);

                tick.Users.Remove(delUser);
                db.Entry(tick).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        [ValidateAntiForgeryToken]
        public ICollection<ApplicationUser> ListUsersOnTicket(int ticketId)
        {
            return db.Tickets.Find(ticketId).Users;
        }

        public ICollection<Ticket> ListUserCreatedTickets(string userId)
        {
            return db.Tickets.Where(p => p.OwnerUserId == userId).ToList();
        }

        [ValidateAntiForgeryToken]
        public ICollection<ApplicationUser> ListUsersNotOnTicket(int ticketId)
        {
            return db.Users.Where(u => u.Tickets.All(p => p.Id != ticketId)).ToList();
        }

        public ICollection<Ticket> ListUserTickets(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var tickets = user.Tickets.ToList();
            return (tickets);
        }

        public static List<TicketComment> TicketCommentsOnTicket(int id)
        {
            return db.TicketComments.Where(c => c.TicketId == id).ToList();
        }

        public static List<TicketAttachment> TicketAttachmentsOnTicket(int id)
        {
            return db.TicketAttachments.Where(c => c.TicketId == id).ToList();
        }

        public static string GetTicketType(int ticketId)
        {
                var none = "No Type Listed";
                var ticketType = db.Tickets.Find(ticketId).TicketType.Value;
                if (ticketType == null)
                {
                    return none;
                }
                if (string.IsNullOrEmpty(ticketType))
                {
                    return none;
                }
                return ticketType;
        }

        public static string GetTicketStatus(int ticketId)
        {
            var none = "No Status Listed";
            var ticketStatus = db.Tickets.Find(ticketId).TicketStatus.Value;
            if (ticketStatus == null)
            {
                return none;
            }
            if (string.IsNullOrEmpty(ticketStatus))
            {
                return none;
            }
            return ticketStatus;
        }

        public static string GetTicketPriority(int ticketId)
        {
            var none = "No Priority Listed";
            var ticketPriority = db.Tickets.Find(ticketId).TicketPriority.Value;
            if (ticketPriority == null)
            {
                return none;
            }
            if (string.IsNullOrEmpty(ticketPriority))
            {
                return none;
            }
            return ticketPriority;
        }

        public static string GetTicketOwner(int ticketId)
        {
            var none = "No Ticket Owner Listed";
            var ticketOwnerId = db.Tickets.Find(ticketId).OwnerUserId;
            var ticketOwner = db.Users.Find(ticketOwnerId).DisplayName;
            if (ticketOwner == null)
            {
                return none;
            }
            if (string.IsNullOrEmpty(ticketOwner))
            {
                return none;
            }
            return ticketOwner;
        }

        public static string GetTicketEditor(string UserId)
        {
            var none = "No Ticket Editor Listed";
            var ticketEditor = db.Users.Find(UserId).DisplayName;
            if (ticketEditor == null)
            {
                return none;
            }
            if (string.IsNullOrEmpty(ticketEditor))
            {
                return none;
            }
            return ticketEditor;
        }
    }
}
 