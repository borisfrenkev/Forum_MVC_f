using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleForumMVC.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The titile is required!")]
        [StringLength(200, MinimumLength = 20)]
        public string Title { get; set; }

        [Required(ErrorMessage = "The titile is required!")]
        [StringLength(1000, MinimumLength = 50)]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required(ErrorMessage = "You must select category!")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

       // [Required(ErrorMessage = "Application user is required!")]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string LastApplicationUserId { get; set; }
        public virtual ApplicationUser LastApplicationUser { get; set; }

        private ICollection<Answer> answers;

        public virtual ICollection<Answer> Answers
        {
            get { return this.answers; }
            set { this.answers = value; }
        }

        private ICollection<Tag> tags;

        public virtual ICollection<Tag> Tags
        {
            get { return this.tags; }
            set { this.tags = value; }
        }

        [Required(ErrorMessage = "CreationDate is required!")]
        public DateTime CreationDate { get; set; }
        [Required(ErrorMessage = "LastAnswerDate is required!")]
        public DateTime LastAnswerDate { get; set; }

        public Question()
        {
            this.answers = new List<Answer>();
            this.tags = new List<Tag>();
            this.CreationDate = DateTime.Now;
            this.LastAnswerDate = DateTime.Now;
        }



    }
}
