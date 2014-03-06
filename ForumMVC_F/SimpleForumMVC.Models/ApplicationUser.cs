using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SimpleForumMVC.Models
{
    public class ApplicationUser:User 
    {
        public byte[] ImageDate { get; set; }

        public string ImageMimeType { get; set; }
    }
}
