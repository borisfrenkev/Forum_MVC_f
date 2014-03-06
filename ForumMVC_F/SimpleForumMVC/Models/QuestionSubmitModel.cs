using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimpleForumMVC.Models
{
    public class QuestionSubmitModel
    {

        [Required(ErrorMessage = "The titile is required!")]
        [StringLength(200,MinimumLength=20)]
        public string Title { get; set; }

        [Required(ErrorMessage = "The titile is required!")]
        [StringLength(1000, MinimumLength = 50)]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required(ErrorMessage="You must select category!")]
        public int CategoryId { get; set; }

        public string Tags { get; set; }

    }
}