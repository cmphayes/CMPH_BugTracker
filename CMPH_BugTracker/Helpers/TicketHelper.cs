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
        ApplicationDbContext db = new ApplicationDbContext();
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
    }
}
 