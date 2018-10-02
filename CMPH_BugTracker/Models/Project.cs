using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMPH_BugTracker.Models
{
    public class Project
    {
        //PrimaryKey
        public int Id { get; set; }

        //Properties
        public string Title { get; set; }
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