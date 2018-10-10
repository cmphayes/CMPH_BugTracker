using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMPH_BugTracker.Models
{
    public class TicketAttachment
    {
        //PrimaryKey
        public int Id { get; set; }
        //ForiegnKey
        public int TicketId { get; set; }
        public string OwnerUserId { get; set; }

        //Properties
        public string MediaURL { get; set; }
        public string Title { get; set; }
        //Parent
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
    }
}