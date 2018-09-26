using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CMPH_BugTracker.Startup))]
namespace CMPH_BugTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
