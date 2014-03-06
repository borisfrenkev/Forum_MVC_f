using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SimpleForumMVC.Models;
using SimpleForumMVC.Data;

namespace SimpleForumMVC.Controllers
{
    public class TagController : Controller
    {
        private ForumContext db = new ForumContext();
        private int pageSize = 10;

        // GET: /Tag/
        public ActionResult List()
        {
            ViewBag.isAuthenticatedAdmin = User.Identity.IsAuthenticated && User.IsInRole("Admin");
            return View(db.Tags.ToList());
        }

        public ActionResult QuestionList(int? page, int? tagId)
        {
            int pageCurr = page != null ? (int)page : 1;
           
            var questions = db.Questions.AsQueryable<Question>();
            questions=questions.Where(q=>(q.Tags.Any(t=>t.Id==tagId)));
            questions = questions.Include(q => q.ApplicationUser).Include(q => q.Category)
                .OrderBy(q => q.CreationDate)
                .Skip((pageCurr - 1) * pageSize).Take(pageSize);
            PagingInfo pageInfo = new PagingInfo
            {
                CurrentPage = pageCurr,
                TotalItems = db.Questions.Count(),
                ItemsPerPage = pageSize
            };

            ViewBag.PageInfo = pageInfo;

            return View(questions.ToList());
        }
        

        // GET: /Tag/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // GET: /Tag/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Tag/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin")]
        public ActionResult Create([Bind(Include="Id,Name")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Tags.Add(tag);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(tag);
        }

        // GET: /Tag/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: /Tag/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include="Id,Name")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(tag);
        }

        // GET: /Tag/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: /Tag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Tag tag = db.Tags.Find(id);
            db.Tags.Remove(tag);
            db.SaveChanges();
            return RedirectToAction("List");
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
