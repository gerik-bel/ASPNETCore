﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using System.IO;
using VG_AspNetCore_Data.Data;
using VG_AspNetCore_Web.Middleware;
using VG_AspNetCore_Web.Middleware.CacheImages;
using VG_AspNetCore_Web.Services;

namespace VG_AspNetCore_Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public Startup(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.RollingFile(Path.Combine(hostingEnvironment.ContentRootPath, "Logs", "VG_AspNetCore_Web-{Date}.txt"))
                .CreateLogger();
            Log.Information($"App Startup called, Application location: {hostingEnvironment.ContentRootPath}");
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Information("ConfigureServices called");
            services.AddDbContext<NorthwindDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IProductsService, SqlProductsService>().Configure<SqlProductsOptions>(configureOptions => configureOptions.MaxShownDisplayCount = GetMaxShownDisplayCount());
            services.AddScoped<ICategoriesService, SqlCategories>();
            services.AddScoped<IHomeService, DefaultHomeService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling =
                        ReferenceLoopHandling.Ignore;
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VG_AspNetCore_Web", Version = "v1" });
                c.IncludeXmlComments(Path.Combine(HostingEnvironment.ContentRootPath, "VG_AspNetCore_Web.xml"));
            });
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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "VG_AspNetCore_Web V1");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseMiddleware<CacheImagesMiddleware>(new CacheImageOptions { CacheExpirationInMilliseconds = 50000, CachePath = Path.Combine(env.ContentRootPath, "cache"), MaxImageCount = 2 });
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