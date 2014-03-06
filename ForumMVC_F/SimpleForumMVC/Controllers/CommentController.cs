using SimpleForumMVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleForumMVC.Models;

namespace SimpleForumMVC.Controllers
{
    public class CommentController : Controller
    {
        private ForumContext db = new ForumContext();
        //
        // GET: /Comment/
     
        public ActionResult CommentForm(int id, string targetId, int questionId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return PartialView("_RedirectView",questionId);
            }
            var allComments = db.Comments.Where(c => c.AnswerId == id).ToList();
            CommentSubmitModel commentModel = new CommentSubmitModel
            {
               AnswerId=id,
               TargetId=targetId,
               Comments = allComments
            };

            return PartialView("_CommentAllForm", commentModel);
        }

        public ActionResult CommentFormEdit(string targetId, int commentId)
        {
            var comment = db.Comments.Find(commentId);
            CommentSubmitModel commentModel = new CommentSubmitModel
            {
                TargetId = targetId,
                CommentContent=comment.Content,
                CommentId=commentId
            };

            return PartialView("_CommentEditForm", commentModel);
        }

     
     
            [ValidateInput(false)] 
            public ActionResult InputComment(CommentSubmitModel commentModel)
            {
                if (ModelState.IsValid)
                {
                    string currentUserName = User.Identity.Name;
                    ApplicationUser appUser = db.Users.Where(x => x.UserName == currentUserName).FirstOrDefault();
                    Comment comment = new Comment
                    {
                        Content = commentModel.CommentContent,
                        CreationDate = DateTime.Now,
                        AnswerId = commentModel.AnswerId,
                        ApplicationUser = appUser
                    };
                    var answer = db.Answers.Find(commentModel.AnswerId);
                    answer.Comments.Add(comment);
                    db.SaveChanges();
                    return PartialView("_CommentAllPartial", answer.Comments.ToList());
                }
              
                return PartialView("_CommentAllForm", commentModel);
            }

            public ActionResult InputCancelComment(int  answerId)
            {
                var answer = db.Answers.Find(answerId);
                db.SaveChanges();
                return PartialView("_CommentAllPartial", answer.Comments.ToList());
            }

            [ValidateInput(false)] 
            public ActionResult EditComment(string targetId, CommentSubmitModel commnetSubmitModel)
            {
                if (ModelState.IsValid)
                {
                    int commnetId = commnetSubmitModel.CommentId;
                    var comment = db.Comments.Find(commnetId);
                    comment.Content = commnetSubmitModel.CommentContent;
                    db.SaveChanges();
                    CommentModel commentModel = new CommentModel
                    {
                        TargetId = targetId,
                        Comment = comment
                    };
                    return PartialView("_CommentPartial", commentModel);
                }

                return PartialView("_CommentEditForm", commnetSubmitModel);
            }

            public ActionResult EditCommentCancel(string targetId, int commentId)
            {
                var comment = db.Comments.Find(commentId);
                CommentModel commnetModel = new CommentModel
                {
                    TargetId = targetId,
                    Comment=comment
                };
                return PartialView("_CommentPartial", commnetModel);
            }
	}
}