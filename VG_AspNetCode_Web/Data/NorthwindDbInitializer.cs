using Microsoft.AspNetCore.Identity;

namespace VG_AspNetCore_Web.Data
{
    public static class NorthwindDbInitializer
    {

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (roleManager.FindByNameAsync("Administrator").Result == null)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                };

                IdentityResult result = roleManager.CreateAsync(role).Result;
                if (result.Succeeded)
                {
                }
            }
        }

        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByEmailAsync("admin@VGAspNetCoreWeb.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "admin@VGAspNetCoreWeb.com",
                    Email = "admin@VGAspNetCoreWeb.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    NormalizedUserName = "ADMIN@VGASPNETCOREWEB.COM",
                    NormalizedEmail = "ADMIN@VGASPNETCOREWEB.COM",
                    TwoFactorEnabled = false,
                };

                IdentityResult result = userManager.CreateAsync(user, "QWE123qwe").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }
    }
}
