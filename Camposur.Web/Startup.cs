using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Camposur.Web.Startup))]
namespace Camposur.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
