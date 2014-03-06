using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleForumMVC.Models
{
    public class UserActivityViewModel
    {
        public int QuestionId { get; set; }

        public string QuestionTitle { get; set; }

        public DateTime MostRecentDate { get; set; }

        public string TypeActivity { get; set; }

        public string CategoryName { get; set; }

        public string UserName { get; set; }

        public int AnswersCount { get; set; }
    }
}