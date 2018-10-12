using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Title { get; set; }
        //Parent
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
    }
}