using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimpleForumMVC.Models
{
    public class CommentSubmitModel
    {
        [Required]
        public int AnswerId { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage="The Commnet content is required") ]
        public string CommentContent { get; set; }

        public string TargetId { get; set; }
      
        public int CommentId { get; set; }
        public IEnumerable<Comment> Comments { get; set; }

    }
}