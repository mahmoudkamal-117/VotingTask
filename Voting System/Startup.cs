using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Voting_System.Startup))]
namespace Voting_System
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
