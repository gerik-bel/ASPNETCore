﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.IO;
using WebApp01Introduction.Data;
using WebApp01Introduction.Middleware;
using WebApp01Introduction.Services;

namespace WebApp01Introduction
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile(Path.Combine(env.ContentRootPath, "Logs", "WebApp01Introduction-{Date}.txt"))
                .CreateLogger();
            Log.Information($"App Startup called, Application location: {env.ContentRootPath}");
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Information("ConfigureServices called");
            services.AddDbContext<NorthwindDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IProducts, SqlProducts>().Configure<SqlProductsOptions>(configureOptions => configureOptions.MaxShownDisplayCount = GetMaxShownDisplayCount());
            services.AddScoped<ICategories, SqlCategories>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private int GetMaxShownDisplayCount()
        {
            var maxShownDisplayCountString = Configuration.GetValue("MaxDisplayProductsCount", string.Empty);
            if (!int.TryParse(maxShownDisplayCountString, out var maxShownDisplayCount))
            {
                Log.Warning("MaxShownDisplayCount contains not numeric value, default value 0 is used");
            }
            return maxShownDisplayCount;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Log.Information("Configure called");
            Log.Information($"Current configuration:\n{GetSectionContent(Configuration)}");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseNodeModules(env.ContentRootPath);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private string GetSectionContent(IConfiguration configSection)
        {
            string sectionContent = string.Empty;
            foreach (var section in configSection.GetChildren())
            {
                sectionContent += "\"" + section.Key + "\":";
                if (section.Value == null)
                {
                    string subSectionContent = GetSectionContent(section);
                    sectionContent += "{\n" + subSectionContent + "},\n";
                }
                else
                {
                    sectionContent += "\"" + section.Value + "\",\n";
                }
            }
            return sectionContent;
        }
    }
}