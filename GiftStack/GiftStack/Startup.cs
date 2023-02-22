using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GiftStack.Startup))]
namespace GiftStack
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
