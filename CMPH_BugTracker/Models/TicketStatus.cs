using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMPH_BugTracker.Models
{
    public class TicketStatus
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int Value { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }

        public TicketStatus()
        {
            Tickets = new HashSet<Ticket>();

        }
    }
}