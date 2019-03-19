using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(myweb.Startup))]
namespace myweb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
