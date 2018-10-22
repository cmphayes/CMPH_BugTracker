using CMPH_BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using CMPH_BugTracker.Helpers;
using System.Text;
using System.Threading.Tasks;
using CMPH_BugTracker.Controllers;

namespace CMPH_BugTracker.Extensions
{
    public static class TicketNotificationExtensions
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static async Task TriggerNotifications(this Ticket ticket, Ticket oldTicket)
        {
            //Let's begin with notifications for Ticket Assignment/UnAssignment
            var newAssignment = (ticket.AssignedUserId != null && oldTicket.AssignedUserId == null);
            var unAssignment = (ticket.AssignedUserId == null && oldTicket.AssignedUserId != null);
            var reAssignment = ((ticket.AssignedUserId != null && oldTicket.AssignedUserId != null) &&
                               (ticket.AssignedUserId != oldTicket.AssignedUserId));

            //Compose the body of the email
            var body = new StringBuilder();
            body.AppendFormat("<p>Email From: <bold>{0}</bold>({1})</p>", "Administrator", WebConfigurationManager.AppSettings["emailfrom"]);
            body.AppendLine("<br/><p><u><b>Message:</b></u></p>");
            body.AppendFormat("<p><b>Project Name:</b> {0}</p>", db.Projects.FirstOrDefault(p => p.Id == ticket.ProjectId).Title);
            body.AppendFormat("<p><b>Ticket Title:</b> {0} | Id: {1}</p>", ticket.Title, ticket.Id);
            body.AppendFormat("<p><b>Ticket Created:</b> {0}</p>", ticket.Created);
            body.AppendFormat("<p><b>Ticket Type:</b> {0}</p>", db.TicketTypes.Find(ticket.TicketTypeId).Value);
            body.AppendFormat("<p><b>Ticket Status:</b> {0}</p>", db.TicketStatus.Find(ticket.TicketStatusId).Value);
            body.AppendFormat("<p><b>Ticket Priority:</b> {0}</p>", db.TicketPriorities.Find(ticket.TicketPriorityId).Value);

            //Generate email
            IdentityMessage email = null;
            if (newAssignment)
            {
                //Generate 1 email to the new Developer letting them know they have been assigned
                email = new IdentityMessage()
                {
                    Subject = "Notification: A Ticket has been assigned to you...",
                    Body = body.ToString(),
                    Destination = db.Users.Find(ticket.AssignedUserId).Email
                };

                var svc = new EmailService();
                await svc.SendAsync(email);
            }
            else if (unAssignment)
            {
                //Generate 1 email to the old Developer letting them know they have been unassigned
                email = new IdentityMessage()
                {
                    Subject = "Notification: You have been taken off of a Ticket...",
                    Body = body.ToString(),
                    Destination = db.Users.Find(oldTicket.AssignedUserId).Email
                };

                var svc = new EmailService();
                await svc.SendAsync(email);
            }
            else if (reAssignment)
            {
                //Generate 1 email to the new Developer letting them know they have been assigned
                email = new IdentityMessage()
                {
                    Subject = "Notification: A Ticket has been assigned to you...",
                    Body = body.ToString(),
                    Destination = db.Users.Find(ticket.AssignedUserId).Email
                };

                var svc = new EmailService();
                await svc.SendAsync(email);

                //Generate 1 email to the old Developer letting them know they have been unassigned
                email = new IdentityMessage()
                {
                    Subject = "Notification: You have been taken off of a Ticket...",
                    Body = body.ToString(),
                    Destination = db.Users.Find(oldTicket.AssignedUserId).Email
                };

                svc = new EmailService();
                await svc.SendAsync(email);
            }

            //Generate Notification
            TicketNotification notification = null;
            if (newAssignment)
            {
                notification = new TicketNotification
                {
                    Body = "Notification: A Ticket has been assigned to you...<br />" + body.ToString(),
                    RecipientId = ticket.AssignedUserId,
                    TicketId = ticket.Id
                };
                db.TicketNotifications.Add(notification);
            }
            else if (unAssignment)
            {
                notification = new TicketNotification
                {
                    Body = "Notification: You have been taken off of a Ticket...<br />" + body.ToString(),
                    RecipientId = oldTicket.AssignedUserId,
                    TicketId = ticket.Id
                };
                db.TicketNotifications.Add(notification);
            }
            else if (reAssignment)
            {
                notification = new TicketNotification
                {
                    Body = "Notification: A Ticket has been assigned to you...<br />" + body.ToString(),
                    RecipientId = ticket.AssignedUserId,
                    TicketId = ticket.Id
                };
                db.TicketNotifications.Add(notification);

                notification = new TicketNotification
                {
                    Body = "Notification: You have been taken off of a Ticket...<br />" + body.ToString(),
                    RecipientId = oldTicket.AssignedUserId,
                    TicketId = ticket.Id
                };
                db.TicketNotifications.Add(notification);
            }

            db.SaveChanges();
        }
    }
}