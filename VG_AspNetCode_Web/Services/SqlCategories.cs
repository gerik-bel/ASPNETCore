using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<Categories> GetAll()
        {
            return _dbContext.Categories.OrderBy(p => p.CategoryId);
        }

        public Categories Get(int id)
        {
            return _dbContext.Categories.FirstOrDefault(p => p.CategoryId == id);
        }
    }
}