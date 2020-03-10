using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(PasteBin.Areas.Identity.IdentityHostingStartup))]
namespace PasteBin.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}