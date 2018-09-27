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
    public class TicketsHelper
    {

        ApplicationDbContext db = new ApplicationDbContext();

        [ValidateAntiForgeryToken]
        public bool IsUserOnTicket(string UserId, int TicketId)
        {
            var ticket = db.Tickets.Find(TicketId);
            var flag = ticket.Users.Any(u => u.Id == UserId);
            return (flag);
        }

        //public ICollection<Ticket> ListUserTickets(string userId)
        //{
        //    ApplicationUser user = db.Users.Find(userId);

        //    var tickets = user.Tickets.ToList();
        //    return (tickets);
        //}

        [ValidateAntiForgeryToken]
        public void AddUserToTicket(string UserId, int TicketId)
        {
            if (!IsUserOnTicket(UserId, TicketId))
            {
                Ticket tick = db.Tickets.Find(TicketId);
                var newUser = db.Users.Find(UserId);

                tick.Users.Add(newUser); db.SaveChanges();
            }
        }

        [ValidateAntiForgeryToken]
        public void RemoveUserFromTicket(string UserId, int TicketId)
        {
            if (IsUserOnTicket(UserId, TicketId))
            {
                Ticket tick = db.Tickets.Find(TicketId);
                var delUser = db.Users.Find(UserId);
                tick.Users.Remove(delUser);
                db.Entry(tick).State = EntityState.Modified;
                // just saves this obj instance.             
                db.SaveChanges();
            }
        }

        [ValidateAntiForgeryToken]
        public ICollection<ApplicationUser> UsersOnTicket(int TicketId)
        {
            return db.Tickets.Find(TicketId).Users;
        }

        [ValidateAntiForgeryToken]
        public ICollection<ApplicationUser> UsersNotOnTicket(int TicketId)
        {
            return db.Users.Where(u => u.Tickets.All(p => p.Id != TicketId)).ToList();

        }
    }
}


 