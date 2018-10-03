using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMPH_BugTracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public DateTimeOffset Created { get; set; }
        public string UserId { get; set; }

        public int OldValue { get; set; }
        public int NewValue { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}