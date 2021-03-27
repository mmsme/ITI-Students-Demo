using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ITI_Students_Demo.Startup))]
namespace ITI_Students_Demo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
