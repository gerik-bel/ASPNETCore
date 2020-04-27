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

        private IQueryable<Products> BuildRequest(bool includeCategory = false, bool includeSupplier = false, bool includeOrderDetails = false)
        {
            IQueryable<Products> request = _dbContext.Products;
            if (includeCategory)
            {
                request = request.Include(p => p.Category);
            }
            if (includeSupplier)
            {
                request = request.Include(p => p.Supplier);
            }
            if (includeOrderDetails)
            {
                request = request.Include(p => p.OrderDetails);
            }
            return request;
        }

        public async Task<IEnumerable<Products>> GetAllAsync(bool includeCategory = false, bool includeSupplier = false, bool includeOrderDetails = false)
        {
            IQueryable<Products> products = BuildRequest(includeCategory, includeSupplier, includeOrderDetails).OrderBy(p => p.ProductId);
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

        public async Task<Products> GetAsync(int id, bool includeCategory = false, bool includeSupplier = false, bool includeOrderDetails = false)
        {
            IQueryable<Products> products = BuildRequest(includeCategory, includeSupplier, includeOrderDetails);
            return await products.FirstOrDefaultAsync(p => p.ProductId == id);
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

        public async Task DeleteAsync(Products product)
        {
            _dbContext.Remove(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}

public class SqlProductsOptions
{
    public int MaxShownDisplayCount { get; set; }
}