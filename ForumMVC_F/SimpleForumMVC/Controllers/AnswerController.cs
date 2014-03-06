using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleForumMVC.Models;
using SimpleForumMVC.Data;

namespace SimpleForumMVC.Controllers
{
    public class AnswerController : Controller
    {

        private ForumContext db = new ForumContext();
        //
        // GET: /Answer/
        public ActionResult AnswerForm(int id)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return PartialView("_RedirectView", id);
            }
            AnswerSubmitModel answerModel= new AnswerSubmitModel
            {
                QuestionId=id
            };


            return PartialView("_AnswerForm", answerModel);
        }

        public ActionResult AnswerEditForm(int answerId, string answerTargetId)
        {
            var answer = db.Answers.Find(answerId);
            AnswerSubmitModel answerModel = new AnswerSubmitModel
            {
               AnswerId=answerId,
               AnswerContent=answer.Content,
               AnswerTargetId=answerTargetId,
               QuestionId = answer.QuestionId
            };

            return PartialView("_AnswerEditForm", answerModel);
        }

        public ActionResult AnswerNewEditForm(int answerId, string answerTargetId)
        {
            var answer = db.Answers.Find(answerId);
            AnswerSubmitModel answerModel = new AnswerSubmitModel
            {
                AnswerId = answerId,
                AnswerContent = answer.Content,
                AnswerTargetId = answerTargetId,
                QuestionId = answer.QuestionId
            };

            return PartialView("_AnswerNewEditForm", answerModel);
        }

        [ValidateInput(false)] 
        public ActionResult EditAnswer(AnswerSubmitModel answerModel)
        {
            if (ModelState.IsValid)
            {
                int answerId = answerModel.AnswerId;
                var answer = db.Answers.Find(answerId);
                answer.Content = answerModel.AnswerContent;
                db.SaveChanges();
                return PartialView("_AnswerPartial", answer);
            }

            return PartialView("_AnswerEditForm", answerModel);

        }

        public ActionResult EditNewAnswer(AnswerSubmitModel answerModel)
        {
            if (ModelState.IsValid)
            {
                int answerId = answerModel.AnswerId;
                var answer = db.Answers.Find(answerId);
                answer.Content = answerModel.AnswerContent;
                db.SaveChanges();
                return PartialView("_NewAnswerPartial", answer);
            }

            return PartialView("_AnswerNewEditForm", answerModel);

        }

        [ValidateInput(false)] 
        public ActionResult InputAnswer(AnswerSubmitModel answerModel)
        {

            if (ModelState.IsValid)
            {
                string currentUserName = User.Identity.Name;
                ApplicationUser appUser = db.Users.Where(x => x.UserName == currentUserName).FirstOrDefault();
                Answer answer = new Answer
                {
                    Content = answerModel.AnswerContent,
                    CreationDate = DateTime.Now,
                    QuestionId = answerModel.QuestionId,
                    ApplicationUser = appUser
                };
                db.Answers.Add(answer);
                db.SaveChanges();
                return PartialView("_NewAnswerPartial", answer);
            }

            return PartialView("_AnswerForm", answerModel);
        }
	}
}