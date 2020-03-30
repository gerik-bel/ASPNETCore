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

        public async Task<IEnumerable<Categories>> GetAllAsync()
        {
            return await _dbContext.Categories.OrderBy(p => p.CategoryId).ToListAsync();
        }

        public async Task<Categories> GetAsync(int id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(p => p.CategoryId == id);
            category.Picture = CorrectPicture(category.Picture);
            return category;
        }

        public async Task<byte[]> GetImageAsync(int id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(p => p.CategoryId == id);
            var image = CorrectPicture(category.Picture);
            return image;
        }

        public async Task UpdateImageAsync(int id, byte[] image)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(p => p.CategoryId == id);
            category.Picture = image;
            await _dbContext.SaveChangesAsync();
        }

        private static byte[] CorrectPicture(byte[] picture)
        {
            var str = Convert.ToBase64String(picture).Replace(GarbageData, string.Empty);
            return Convert.FromBase64String(str);
        }
    }
}