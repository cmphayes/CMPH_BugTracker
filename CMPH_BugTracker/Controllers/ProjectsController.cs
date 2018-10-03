using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CMPH_BugTracker.Helpers;
using CMPH_BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace CMPH_BugTracker.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();
        private TicketsHelper ticketHelper = new TicketsHelper();



        // GET: Projects
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Index()
        {
                return View(db.Projects.ToList());
          
            //ViewBag.Search = searchStr; var ProjectsList = IndexSearch(searchStr);

            //int pageSize = 5; // the number of posts you want to display per page             
            //int pageNumber = (page ?? 1); 

            //return View(ProjectsList.ToPagedList(pageNumber, pageSize));
        }
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult MyProjects()
        {
            var userId = User.Identity.GetUserId();
            return View(projectHelper.ListUserProjects(userId));
        }

        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult CreatedProjects()
        {
            var userId = User.Identity.GetUserId();
            return View(projectHelper.ListUserCreatedProjects(userId));
        }

        // GET: Projects/Details/5
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult Create([Bind(Include = "Id,Title,Body,User")] Project project)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrWhiteSpace(project.Title))
                {
                    ModelState.AddModelError("Title", "Invalid title");
                    return View(project);
                }
                if (db.Projects.Any(p => p.Id == project.Id))
                {
                    ModelState.AddModelError("Title", "The title must be unique");
                    return View(project);
                }

                //var project = new Project { Title = model.Title, Body = model.Body};
                string userId = User.Identity.GetUserId(); 
                project.AssignedUserId = userId;
                project.Created = DateTimeOffset.Now;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }        
            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,UserId,Created,Updated")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Updated = DateTimeOffset.Now;
                db.Projects.Add(project);
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



    }
}
