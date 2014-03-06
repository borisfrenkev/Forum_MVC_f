using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleForumMVC.Models
{
    public class CommentModel
    {
        public string TargetId { get; set; }
        public Comment Comment { get; set; }
    }
}