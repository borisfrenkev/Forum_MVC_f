using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SimpleForumMVC.Startup))]
namespace SimpleForumMVC
{
    public partial class Startup 
    {
        public void Configuration(IAppBuilder app) 
        {
            ConfigureAuth(app);
        }
    }
}
