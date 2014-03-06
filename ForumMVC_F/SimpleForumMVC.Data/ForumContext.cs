using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleForumMVC.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SimpleForumMVC.Data
{
    public class ForumContext : IdentityDbContextWithCustomUser<ApplicationUser>
    {
        public DbSet<Answer> Answers { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Tag> Tags { get; set; }

    }
}
