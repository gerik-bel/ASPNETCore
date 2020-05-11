using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VG_AspNetCore_Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Authorize(Policy = "RequireAdministratorRole")]
    public class UsersController : Controller
    {
        private readonly IdentityDbContext _context;

        public UsersController(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await GetUsersAsync();
            return View(users);
        }

        private async Task<List<IdentityUser>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}