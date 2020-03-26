using System.Collections.Generic;
using System.Linq;
using WebApp01Introduction.Data;
using WebApp01Introduction.Models;

namespace WebApp01Introduction.Services
{
    public class SqlCategories : ICategories
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