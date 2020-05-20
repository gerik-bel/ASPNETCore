using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System.IO;
using VG_AspNetCore_Web.Data;
using VG_AspNetCore_Web.Middleware;
using VG_AspNetCore_Web.Services;

namespace VG_AspNetCore_Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        public Startup(IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
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
            services.AddScoped<IdentityDbContext, NorthwindDbContext>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.SignIn.RequireConfirmedEmail = false;
                }).AddDefaultTokenProviders().AddDefaultUI().AddEntityFrameworkStores<NorthwindDbContext>().AddRoles<IdentityRole>();
            services.AddTransient<IEmailSender, EmailService>();

            services.AddAuthentication(AzureADDefaults.AuthenticationScheme).AddAzureAD(options =>
            {
                Configuration.Bind("AzureAd", options);
                options.CookieSchemeName = IdentityConstants.ExternalScheme;
            });

            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            {
                options.Authority = options.Authority + "/v2.0/";

            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VG_AspNetCore_Web", Version = "v1" });
                c.CustomOperationIds(d => (d.ActionDescriptor as ControllerActionDescriptor)?.ActionName);
                c.IncludeXmlComments(Path.Combine(HostingEnvironment.ContentRootPath, "VG_AspNetCore_Web.xml"));
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole",
                    policy => policy.RequireRole("Administrator"));
            });
            services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddRazorPages();
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            Log.Information("Configure called");
            Log.Information($"Current configuration:\n{GetSectionContent(Configuration)}");
            NorthwindDbInitializer.SeedRoles(roleManager);
            NorthwindDbInitializer.SeedUsers(userManager);
            app.UseSwagger(o => o.SerializeAsV2 = true);
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
            //app.UseMiddleware<CacheImagesMiddleware>(new CacheImageOptions { CacheExpirationInMilliseconds = 50000, CachePath = Path.Combine(env.ContentRootPath, "cache"), MaxImageCount = 2 });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseNodeModules(env.ContentRootPath);
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
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