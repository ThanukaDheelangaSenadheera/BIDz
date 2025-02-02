using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BidZ.lk.Startup))]
namespace BidZ.lk
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
