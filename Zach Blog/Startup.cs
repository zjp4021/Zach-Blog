using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Zach_Blog.Startup))]
namespace Zach_Blog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
