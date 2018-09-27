using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMPH_BugTracker.Models
{
    public class Ticket
    {
        //PrimaryKey
        public int Id { get; set; }

        //ForiegnKeys
        public int ProjectId { get; set; }
        public int TicketPriorityId { get; set; }
        public int TicketStatusId { get; set; }
        public int TicketTypeId { get; set; }
        public string AssignedUserID { get; set; }
        public string OwnerUserID { get; set; }

        //Properties
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        //Childern
        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
        public virtual ICollection<TicketComment> TicketComments { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketNotification> TicketNotifications { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }


        //Parents
        public virtual Project Project { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual ApplicationUser AssignedUser { get; set; }
        public virtual ApplicationUser OwnerUser { get; set; }

        //Instantiating
        public Ticket()
        {
            TicketComments = new HashSet<TicketComment>();
            TicketAttachments = new HashSet<TicketAttachment>();
            TicketHistories = new HashSet<TicketHistory>();
            TicketNotifications = new HashSet<TicketNotification>();

        }


}
}