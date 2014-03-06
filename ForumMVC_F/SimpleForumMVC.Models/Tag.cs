using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SimpleForumMVC.Models
{
    public class Tag
    {
        public int Id { get; set; }

       [Required(ErrorMessage = "Category Name is Rquired")]
        public string Name { get; set; }

        private ICollection<Question> questions;

        public virtual ICollection<Question> Questions
        {
            get { return this.questions; }
            set { this.questions = value; }
        }
        

    }
}
