using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ToolsRUsWebsite.Startup))]
namespace ToolsRUsWebsite
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
