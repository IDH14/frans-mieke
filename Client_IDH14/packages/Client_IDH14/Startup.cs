using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Client_IDH14.Startup))]
namespace Client_IDH14
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
