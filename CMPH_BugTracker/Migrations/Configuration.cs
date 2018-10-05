namespace CMPH_BugTracker.Migrations
{
    using CMPH_BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CMPH_BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CMPH_BugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //create roles
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            //create users
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            //create user to assign to admin role
            if (!context.Users.Any(u => u.Email == "Developer@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Developer@Mailinator.com",
                    Email = "Developer@Mailinator.com",
                    FirstName = "Dev",
                    LastName = "eloper",
                    DisplayName = "Developer"
                }, "Abcd1234!");
            }
            var DeveloperId = userManager.FindByEmail("Developer@Mailinator.com").Id;
            userManager.AddToRole(DeveloperId, "Developer");

            if (!context.Users.Any(u => u.Email == "Submitter@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Submitter@Mailinator.com",
                    Email = "Submitter@Mailinator.com",
                    FirstName = "Sub",
                    LastName = "mitter",
                    DisplayName = "Submitter"
                }, "Abcd1234!");
            }
            var SubmitterId = userManager.FindByEmail("Submitter@Mailinator.com").Id;
            userManager.AddToRole(SubmitterId, "Submitter");

            if (!context.Users.Any(u => u.Email == "Submitter@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Admin@Mailinator.com",
                    Email = "Admin@Mailinator.com",
                    FirstName = "Admin",
                    LastName = "istrator",
                    DisplayName = "Administrator"
                }, "Abcd1234!");
            }
            var AdminId = userManager.FindByEmail("Admin@Mailinator.com").Id;
            userManager.AddToRole(AdminId, "Admin");

            //create user to assign to mod role
            if (!context.Users.Any(u => u.Email == "ProjectManager@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ProjectManager@Mailinator.com",
                    Email = "ProjectManager@Mailinator.com",
                    FirstName = "Project",
                    LastName = "Manager",
                    DisplayName = "ProjectManager"
                }, "Abcd1234!");
            }
            //assign users to roles
            var ProjectManagerId = userManager.FindByEmail("ProjectManager@Mailinator.com").Id;
            userManager.AddToRole(ProjectManagerId, "ProjectManager");

            if (!context.Users.Any(u => u.Email == "CMPH@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "CMPH@Mailinator.com",
                    Email = "CMPH@Mailinator.com",
                    FirstName = "C",
                    LastName = "H",
                    DisplayName = "CMPH"
                }, "Abcd1234!");
            }
            //assign users to roles
            var CMPHId = userManager.FindByEmail("CMPH@Mailinator.com").Id;
            userManager.AddToRole(CMPHId, "Admin");
        }
    }
}


//     Context.TicketStatus.AddOrUpdate(ts => ts.Name)
//    new TicketStatus {Name = "Status1"};
//    new TicketStatus {Name = "Status1"};
//    new TicketStatus {Name = "Status2"};
//    new TicketStatus {Name = "Status3"};            
//);

//    Context.TicketType.AddOrUpdate(tt => tt.Name)
//    new TicketType {Name = "Bug"};
//    new TicketType {Name = "Bug"};
//    new TicketType {Name = "Bug"};
//    new TicketType {Name = "Bug"};                 
//);

//    Context.TicketPriority.AddOrUpdate(tp => tp.Name)
//    new TicketPriority {Name = "Priority1"};
//    new TicketPriority {Name = "Priority2"};
//    new TicketPriority {Name = "Priority3"};
//    new TicketPriority {Name = "Priority4"};            
//);
