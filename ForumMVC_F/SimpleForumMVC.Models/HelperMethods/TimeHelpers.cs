using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleForumMVC.Models.HelperMethods
{
    public class TimeHelpers
    {
       public static string GetTimeDiff(DateTime timePost)
        {
            string result = string.Empty;
            DateTime timeNow = DateTime.Now;
            TimeSpan timeDiff = timeNow.Subtract(timePost);
            if (timeDiff.Days > 0)
            {
              return string.Format("{0} {1} ", timePost.ToString("MMM"), timePost.Day);
            }

            if (timeDiff.Hours > 0)
            {
                return timeDiff.Hours.ToString() + " hours ago ";
            }

            if (timeDiff.Minutes > 0)
            {
                return timeDiff.Minutes.ToString() + " minutes ago ";
            }

            if (timeDiff.Seconds > 0)
            {
                return timeDiff.Seconds.ToString() + " seconds ago ";
            }

            return result;
        }
    }
}
