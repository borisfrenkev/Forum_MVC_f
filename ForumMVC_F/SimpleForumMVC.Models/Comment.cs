using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleForumMVC.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; }
        public DateTime CreationDate { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int AnswerId { get; set; }

        public virtual Answer Answer { get; set; }



    }
}
