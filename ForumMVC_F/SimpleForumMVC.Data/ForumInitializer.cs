using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SimpleForumMVC.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.ObjectModel;

namespace SimpleForumMVC.Data
{
    public class ForumInitializer:DropCreateDatabaseIfModelChanges<ForumContext>
    {
        
        protected override void Seed(ForumContext context)
        {


            var userAdmin = new ApplicationUser()
            {
                UserName = "gosho",
                Logins = new Collection<UserLogin>()
                {
                    new UserLogin()
                    {
                        LoginProvider = "Local",
                        ProviderKey = "gosho",
                    }
                },
                Roles = new Collection<UserRole>()
                {
                    new UserRole()
                    {
                        Role = new Role("Admin")
                    }
                }
            };
          
            context.Users.Add(userAdmin);
            context.UserSecrets.Add(new UserSecret("gosho",
                "ADXTGjbMwBBorBcW00Ec5WxnzAFauKKkXiTb/oUDnXBPnNAcKcaa0AXyUempybf61A=="));//admin123


            var userSimple1 = new ApplicationUser()
            {
                UserName = "ivan",
                Logins = new Collection<UserLogin>()
                {
                    new UserLogin()
                    {
                        LoginProvider = "Local",
                        ProviderKey = "ivan",
                    }
                }
            };
            context.Users.Add(userSimple1);
            context.UserSecrets.Add(new UserSecret("ivan",
                "AOZwlxPO+kFWcmQC4ovZypZ6iyQX0mhacK5lnS9UNNF1z1673AkSTid8NdcGKxt2Bg=="));//123456

            var userSimple2 = new ApplicationUser()
            {
                UserName = "pesho",
                Logins = new Collection<UserLogin>()
                {
                    new UserLogin()
                    {
                        LoginProvider = "Local",
                        ProviderKey = "pesho",
                    }
                }
            };
            context.Users.Add(userSimple2);
            context.UserSecrets.Add(new UserSecret("pesho",
                "AFbfCboNGoBlnJ3EqNYkPKUy7rKa6t+tVcEd7j8HjCfFBW2w1KrV96Y400DEpSkhfA=="));//pesho123
            context.SaveChanges();

            var categories = new List<Category>()
            {
                new Category {Name="C#"},
                new Category {Name="CSS"},
                new Category {Name="Javascript"},
                new Category {Name="Jquery"},
                new Category {Name="PHP"}
            };

            categories.ForEach(c => context.Categories.Add(c));
            context.SaveChanges();

            var tags = new List<Tag>()
            {
                new Tag {Name="Lecture"},
                new Tag {Name="Exam"},
                new Tag {Name="Homework"},
                new Tag {Name="Problem"},
                new Tag {Name="Algorithm"},
                new Tag {Name="Database"},
            };

            tags.ForEach(t => context.Tags.Add(t));
            context.SaveChanges();

        }
    }
}
