using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(VG_AspNetCore_Web.Areas.Identity.IdentityHostingStartup))]
namespace VG_AspNetCore_Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}