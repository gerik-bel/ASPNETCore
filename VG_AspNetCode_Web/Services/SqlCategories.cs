using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VG_AspNetCore_Web.Data;
using VG_AspNetCore_Web.Models;

namespace VG_AspNetCore_Web.Services
{
    public class SqlCategories : ICategoriesService
    {
        private readonly NorthwindDbContext _dbContext;
        public SqlCategories(NorthwindDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Categories>> GetAllAsync()
        {
            return await _dbContext.Categories.OrderBy(p => p.CategoryId).ToListAsync();
        }

        public async Task<Categories> GetAsync(int id)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(p => p.CategoryId == id);
        }
    }
}