using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VG_AspNetCore_Data.Data;
using VG_AspNetCore_Data.Models;


namespace VG_AspNetCore_Web.Services
{
    public class SqlProductsService : IProductsService
    {
        private readonly NorthwindDbContext _dbContext;
        private readonly int _maxShownDisplayCount;
        private const int None = 0;
        private readonly SelectListItem _emptyItem = new SelectListItem("None", None.ToString(), true);

        public SqlProductsService(NorthwindDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SqlProductsService(NorthwindDbContext dbContext, IOptions<SqlProductsOptions> options) : this(dbContext)
        {
            _maxShownDisplayCount = options.Value.MaxShownDisplayCount;
        }

        public async Task<IEnumerable<Products>> GetAllWithIncludesAsync()
        {
            IQueryable<Products> products = _dbContext.Products.Include(p => p.Supplier).Include(p => p.Category).OrderBy(p => p.ProductId);
            if (_maxShownDisplayCount != 0)
            {
                products = products.Take(_maxShownDisplayCount);
            }
            return await products.ToListAsync();
        }

        public async Task<IEnumerable<Products>> GetAllAsync()
        {
            IQueryable<Products> products = _dbContext.Products.OrderBy(p => p.ProductId);
            if (_maxShownDisplayCount != 0)
            {
                products = products.Take(_maxShownDisplayCount);
            }
            return await products.ToListAsync();
        }

        public async Task<List<SelectListItem>> GetSuppliersAsync()
        {
            var suppliers = await _dbContext.Suppliers.OrderBy(p => p.SupplierId)
                .Select(p => new SelectListItem(p.CompanyName, p.SupplierId.ToString())).ToListAsync();
            suppliers.Add(_emptyItem);
            return suppliers;
        }

        public async Task<List<SelectListItem>> GetCategoriesAsync()
        {
            var categories = await _dbContext.Categories.OrderBy(p => p.CategoryId)
                .Select(p => new SelectListItem(p.CategoryName, p.CategoryId.ToString())).ToListAsync();
            categories.Add(_emptyItem);
            return categories;
        }

        public async Task<Products> GetAsync(int id)
        {
            return await _dbContext.Products.Include(p => p.Supplier).Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<Products> AddAsync(Products product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Products> UpdateAsync(Products product)
        {
            _dbContext.Products.Attach(product).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return product;
        }
    }
}

public class SqlProductsOptions
{
    public int MaxShownDisplayCount { get; set; }
}