using SimpleForumMVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleForumMVC.Models;

namespace SimpleForumMVC.Controllers
{
    public class UserController : Controller
    {
        private int pageSize = 10;
        private ForumContext db = new ForumContext();

        public ActionResult List(int? page)
        {
            var users = db.Users;
            int pageCurr = page != null ? (int)page : 1;
            var usernames = users.Select(x => x.UserName).OrderBy(x => x)
                .Skip((pageCurr - 1) * pageSize)
                .Take(pageSize);
            PagingInfo pageInfo = new PagingInfo
            {
                CurrentPage = pageCurr,
                TotalItems = db.Questions.Count(),
                ItemsPerPage = pageSize
            };
            ViewBag.PageInfo = pageInfo;
          
            return View(usernames);
        }



        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase image)
        {
            string userName = User.Identity.Name;
            ApplicationUser user = db.Users.Where(u => u.UserName == userName).SingleOrDefault();
            if (image != null && image.ContentLength > 0)
            {
                user.ImageMimeType = image.ContentType;
                user.ImageDate = new byte[image.ContentLength];
                image.InputStream.Read(user.ImageDate, 0, image.ContentLength);
                db.SaveChanges();
            }

            return RedirectToAction("Manage", "Account");
        }

       // [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete()
        {
            string userName = User.Identity.Name;
            ApplicationUser user = db.Users.Where(u => u.UserName == userName).SingleOrDefault();
            user.ImageMimeType = null;
            user.ImageDate = null;
            db.SaveChanges();
            return RedirectToAction("Manage", "Account");
        }

        public FileContentResult GetImage(string username)
        {
            //string userName = User.Identity.Name;
            ApplicationUser user = db.Users.Where(u => u.UserName == username).SingleOrDefault();
            if (user != null && user.ImageDate!=null && user.ImageMimeType!=null)
            {
                Request.RequestContext.HttpContext.Response.AddHeader("Content-Disposition", "Attachment;filename=image.jpg");
                return File(user.ImageDate, user.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        public ActionResult GetImageRendered()
        {
            string userName = User.Identity.Name;
            ApplicationUser user = db.Users.Where(u => u.UserName == userName).SingleOrDefault();
            ImageModel imageModel = new ImageModel();
            Request.RequestContext.HttpContext.Response.AddHeader("Content-Disposition", "Attachment;filename=image.jpg");
            if (user != null && user.ImageDate != null && user.ImageMimeType != null)
            {
                FileResult result = File(user.ImageDate, user.ImageMimeType);
                imageModel.imageFile = result;
                Request.RequestContext.HttpContext.Response.AddHeader("Content-Disposition", "Attachment;filename=image.jpg");
            }

            return PartialView("_PartialImageView", imageModel);
        }

        //
        // GET: /User/
        public ActionResult UserActivity(string userName)
        {

            Dictionary<int, UserActivityViewModel> questionIDs = new Dictionary<int, UserActivityViewModel>();
            ApplicationUser user = db.Users.Where(u => u.UserName == userName).SingleOrDefault();

           
          
            var userComments = from c in db.Comments
                               where c.ApplicationUser.UserName == user.UserName
                               select new UserActivityViewModel
                               {
                                   QuestionId=c.Answer.QuestionId,
                                   QuestionTitle=c.Answer.Question.Title,
                                   MostRecentDate=c.CreationDate,
                                   TypeActivity = "commented",
                                   UserName=c.ApplicationUser.UserName,
                                   CategoryName=c.Answer.Question.Category.Name,
                                   AnswersCount=c.Answer.Question.Answers.Count
                               };

            int allCountComments = 0;
            foreach (var comment in userComments)
            {
                allCountComments++;
                int questionId = comment.QuestionId;                                         
                if (questionIDs.ContainsKey(questionId) && questionIDs[questionId].TypeActivity != "asked")
                {
                    if (questionIDs[questionId].MostRecentDate < comment.MostRecentDate)
                    {
                        questionIDs[questionId].MostRecentDate = comment.MostRecentDate;
                        questionIDs[questionId].TypeActivity = "commented";
                    }
                }
                else
                {
                    questionIDs[questionId] = comment;
                }
            }

           
            var userAnswers = from a in db.Answers
                               where a.ApplicationUser.UserName == user.UserName
                               select new UserActivityViewModel
                               {
                                   QuestionId = a.QuestionId,
                                   QuestionTitle = a.Question.Title,
                                   MostRecentDate = a.CreationDate,
                                   TypeActivity = "answered",
                                   UserName = a.ApplicationUser.UserName,
                                   CategoryName = a.Question.Category.Name,
                                   AnswersCount = a.Question.Answers.Count
                               };

            int allCountAnswers = 0;
            foreach (var answer in userAnswers)
            {
                allCountAnswers++;
                int questionId = answer.QuestionId;
                if (questionIDs.ContainsKey(questionId) && questionIDs[questionId].TypeActivity != "asked")
                {
                    if (questionIDs[questionId].MostRecentDate < answer.MostRecentDate)
                    {
                        questionIDs[questionId].MostRecentDate = answer.MostRecentDate;
                        questionIDs[questionId].TypeActivity = "answered";
                    }
                }
                else
                {
                    questionIDs[questionId] = answer;
                }
            }

            var userQuestions = from q in db.Questions
                                where q.ApplicationUser.UserName == user.UserName
                                select new UserActivityViewModel
                                {
                                    QuestionId = q.Id,
                                    QuestionTitle = q.Title,
                                    MostRecentDate = q.CreationDate,
                                    TypeActivity = "asked",
                                    UserName = q.ApplicationUser.UserName,
                                    CategoryName = q.Category.Name,
                                    AnswersCount = q.Answers.Count
                                };
            int allCountQuestions = 0;
            foreach (var question in userQuestions)
            {
                allCountQuestions++;
                int questionId = question.QuestionId;
                questionIDs[questionId] = question;
            }

            IList<UserActivityViewModel> listActivityViewModel = new List<UserActivityViewModel>();
            foreach (var item in questionIDs)
            {
                listActivityViewModel.Add(item.Value);
            }
            listActivityViewModel.OrderBy(x => x.MostRecentDate);

            ViewBag.allCountQuestions = allCountQuestions;
            ViewBag.allCountAnswers = allCountAnswers;
            ViewBag.allCountComments = allCountComments;
            ViewBag.userName = userName;
            return View(listActivityViewModel);
        }
	}
}