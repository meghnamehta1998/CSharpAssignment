using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ManageProductDetails.Startup))]
namespace ManageProductDetails
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
