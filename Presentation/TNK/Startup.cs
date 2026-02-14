using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TNK.Startup))]
namespace TNK
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
