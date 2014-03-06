using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimpleForumMVC.Models
{
    public class AnswerSubmitModel
    {
        [Required]
        public int QuestionId { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "The answer content is required")]
        public string AnswerContent { get; set; }
        public int AnswerId { get; set; }
        public string AnswerTargetId { get; set; }

    }
}