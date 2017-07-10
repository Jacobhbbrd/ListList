using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ListListDev.Startup))]
namespace ListListDev
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
