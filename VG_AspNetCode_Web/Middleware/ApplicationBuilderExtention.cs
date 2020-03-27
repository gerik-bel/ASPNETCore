using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace VG_AspNetCore_Web.Middleware
{
    public static class ApplicationBuilderExtensions
    {
        private const string NodeDirectory = "node_modules";
        public static IApplicationBuilder UseNodeModules(this IApplicationBuilder app, string root)
        {
            var path = Path.Combine(root, NodeDirectory);
            var fileProvider = new PhysicalFileProvider(path);
            var options = new StaticFileOptions { RequestPath = $"/{NodeDirectory}", FileProvider = fileProvider };

            app.UseStaticFiles(options);
            return app;
        }
    }
}