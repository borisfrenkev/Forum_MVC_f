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
using System.Web.Security;

namespace SimpleForumMVC.Controllers
{
    public class QuestionController : Controller
    {
        private ForumContext db = new ForumContext();
        private int pageSize = 10;

        // GET: /Question/
        public ActionResult List(int? page, string searchString)
        {
            var questions = db.Questions.Include(q => q.ApplicationUser).Include(q => q.Category);
            if (searchString!=null)
            {
                questions = questions.Where(q => q.Title.Contains(searchString));
            }
            int pageCurr=page!=null?(int)page:1;
            questions=questions.OrderBy(q=>q.CreationDate)
                .Skip((pageCurr-1)*pageSize).Take(pageSize);
            PagingInfo pageInfo = new PagingInfo
            {   
                CurrentPage=pageCurr,
                TotalItems=db.Questions.Count(),
                ItemsPerPage=pageSize
            };

            ViewBag.PageInfo = pageInfo;

            return View(questions.ToList());
        }

        // GET: /Question/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: /Question/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "UserName");
            var categories = db.Categories.OrderBy(c => c.Name);
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name");
            return View();
        }

        // POST: /Question/Create
		// To protect from over posting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		// 
		// Example: public ActionResult Update([Bind(Include="ExampleProperty1,ExampleProperty2")] Model model)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)] 
        public ActionResult Create(QuestionSubmitModel questionSubmitModel)
        {
            Question question = new Question();
            if (ModelState.IsValid)
            {
                question.CreationDate = DateTime.Now;
                question.LastAnswerDate = DateTime.Now;
                string currentUserName = User.Identity.Name;
               
                ApplicationUser appUser = db.Users.Where(x => x.UserName == currentUserName).FirstOrDefault();
                question.ApplicationUser = appUser;
                question.LastApplicationUser = appUser;
                question.Title = questionSubmitModel.Title;
                question.Content = questionSubmitModel.Content;
                question.CategoryId = questionSubmitModel.CategoryId;
                string tags = questionSubmitModel.Tags;

                if (tags!= null && !string.IsNullOrEmpty(tags.Trim()))
                {
                    string[] tagsArr = tags.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    HashSet<string> tempSet = new HashSet<string>(tagsArr);
                    foreach (var tagName in tempSet)
                    {
                        Tag tagDb = db.Tags.Where(x => x.Name == tagName).FirstOrDefault();
                        if (tagDb!=null)
                        {
                            question.Tags.Add(tagDb);
                        }
                    }
                }

                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("List");
            }

           
            var categories = db.Categories.OrderBy(c => c.Name);
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name");
            return View(questionSubmitModel);
        }

        // GET: /Question/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "UserName", question.ApplicationUserId);
            //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", question.CategoryId);
            var categories = db.Categories.OrderBy(c => c.Name);
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name");
            return View(question);
        }

        // POST: /Question/Edit/5
		// To protect from over posting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		// 
		// Example: public ActionResult Update([Bind(Include="ExampleProperty1,ExampleProperty2")] Model model)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)] 
        public ActionResult Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new  {id=question.Id});
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "UserName", question.ApplicationUserId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", question.CategoryId);
            return View(question);
        }

        // GET: /Question/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: /Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        public ActionResult LookupTags(string term)
        {   
            var retValue = db.Tags.AsQueryable();
            if (term!= null)
	        {
		        retValue=retValue.Where(x => x.Name.StartsWith(term));
	        }
            return Json(retValue.Select(a => new {value = a.Name}).ToList(), JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
