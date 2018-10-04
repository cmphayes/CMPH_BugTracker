using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMPH_BugTracker.Models
{
    public class Project
    {
        //PrimaryKey
        public int Id { get; set; }

        //Properties
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Title { get; set; }
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Body { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public string AssignedUserId { get; set; }
        public string OwnerUserId { get; set; }

        //Children
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }


        //Instantiating
        public Project()
        {
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();


        }
    }
}