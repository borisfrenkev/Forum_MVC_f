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
    public class HomeController : Controller
    {
        private  int indexPageSize=10;
        private ForumContext db = new ForumContext();
        public ActionResult Index()
        {
            var questions = db.Questions.Include(q => q.ApplicationUser).Include(q => q.Category)
                .Take(indexPageSize).OrderBy(q => q.LastAnswerDate);
            return View(questions.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}