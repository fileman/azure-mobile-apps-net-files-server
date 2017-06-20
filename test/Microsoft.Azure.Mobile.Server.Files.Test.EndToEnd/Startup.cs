using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Microsoft.Azure.Mobile.Server.Files.Test.EndToEnd.Startup))]

namespace Microsoft.Azure.Mobile.Server.Files.Test.EndToEnd
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}