using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CMPH_BugTracker.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;


namespace CMPH_BugTracker.Controllers
{
    [Authorize]
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttachments
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Index(int? page, string searchStr)
        {
            ViewBag.Search = searchStr; var TicketAttachmentsList = IndexSearch(searchStr);

            int pageSize = 5; // the number of posts you want to display per page             
            int pageNumber = (page ?? 1);

            return View(TicketAttachmentsList.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public IQueryable<TicketAttachment> IndexSearch(string searchStr)
        {
            IQueryable<TicketAttachment> result = null;
            if (searchStr != null) { result = db.TicketAttachments.AsQueryable(); result = result.Where(p => p.Title.Contains(searchStr));
            }
            else { result = db.TicketAttachments.AsQueryable(); }

            return result.OrderByDescending(p => p.Created);
        }

        // GET: TicketAttachments/Details/5
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Create
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Create()
        {
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            return View();
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,Title")] TicketAttachment ticketAttachment, HttpPostedFileBase image, int Id)
        {
            if (ModelState.IsValid)
            {
                //ticketAttachment.OwnerUserId = User.Identity.GetUserId();
                //ticketAttachment.Created = DateTimeOffset.Now;
                //db.TicketAttachments.Add(ticketAttachment);
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }

            if (UploadValidator.IsWebFriendlyImage(image))
            {
                var fileName = Path.GetFileName(image.FileName);
                image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));

                var myAttachment = new TicketAttachment
                {
                    MediaURL = "/Uploads/" + fileName,
                    TicketId = ticketAttachment.TicketId
                };
                ticketAttachment.OwnerUserId = User.Identity.GetUserId();
                ticketAttachment.Created = DateTimeOffset.Now;
                db.TicketAttachments.Add(ticketAttachment);
                db.SaveChanges();
            }

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachment.TicketId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Edit/5
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachment.TicketId);
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,MediaURL,Title")] TicketAttachment ticketAttachment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttachment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachment.TicketId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Delete/5
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketAttachment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Download(string ticketAttachment)
        {
            string path = Server.MapPath("~/Images/");
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] files = dirInfo.GetFiles("*.*");
            List<string> 1st = new List<string>(files.Length);
            foreach (var item in files)
            {
                1st.Add(item.Name);

            }
            return View(1st);
        }

        public ActionResult DownloadFile(string filename)
        {
            if (Path.GetExtension(filename) == ".png")
            {
                string fullPath = Path.Combine(Server.MapPath("~/Images/"), filename);
                return File(fullPath, "Images/png");
            }
            else
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
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
