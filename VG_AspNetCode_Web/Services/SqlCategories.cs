using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VG_AspNetCore_Web.Data;
using VG_AspNetCore_Web.Models;

namespace VG_AspNetCore_Web.Services
{
    public class SqlCategories : ICategoriesService
    {
        private const string GarbageData = "FRwvAAIAAAANAA4AFAAhAP////9CaXRtYXAgSW1hZ2UAUGFpbnQuUGljdHVyZQABBQAAAgAAAAcAAABQQnJ1c2gAAAAAAAAAAACgKQAA";
        private readonly NorthwindDbContext _dbContext;
        public SqlCategories(NorthwindDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IQueryable<Categories> BuildRequest(bool includeProducts = false)
        {
            IQueryable<Categories> request = _dbContext.Categories;
            if (includeProducts)
            {
                request = request.Include(p => p.Products);
            }
            return request;
        }

        public async Task<IEnumerable<Categories>> GetAllAsync(bool includeProducts = false)
        {
            var request = BuildRequest(includeProducts);
            return await request.OrderBy(p => p.CategoryId).ToListAsync();
        }

        public async Task<Categories> GetAsync(int id, bool includeProducts = false)
        {
            var request = BuildRequest(includeProducts);
            var category = await request.FirstOrDefaultAsync(p => p.CategoryId == id);
            if (category != null)
            {
                category.Picture = CorrectPicture(category.Picture);
            }
            return category;
        }

        public async Task<byte[]> GetImageAsync(int id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(p => p.CategoryId == id);
            if (category != null)
            {
                var image = CorrectPicture(category.Picture);
                return image;
            }
            return null;
        }

        public async Task<bool> UpdateImageAsync(int id, byte[] image)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(p => p.CategoryId == id);
            if (category == null)
            {
                return false;
            }
            category.Picture = image;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private static byte[] CorrectPicture(byte[] picture)
        {
            var str = Convert.ToBase64String(picture).Replace(GarbageData, string.Empty);
            return Convert.FromBase64String(str);
        }
    }
}