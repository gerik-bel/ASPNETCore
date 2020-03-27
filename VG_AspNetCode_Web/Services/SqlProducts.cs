using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using VG_AspNetCore_Web.Data;
using VG_AspNetCore_Web.Models;

namespace VG_AspNetCore_Web.Services
{
    public class SqlProductsService : IProductsService
    {
        private readonly NorthwindDbContext _dbContext;
        private readonly int _maxShownDisplayCount;

        public SqlProductsService(NorthwindDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SqlProductsService(NorthwindDbContext dbContext, IOptions<SqlProductsOptions> options) : this(dbContext)
        {
            _maxShownDisplayCount = options.Value.MaxShownDisplayCount;
        }

        public IEnumerable<Products> GetAll()
        {
            IQueryable<Products> products = _dbContext.Products.Include(p => p.Supplier).Include(p => p.Category).OrderBy(p => p.ProductId);
            if (_maxShownDisplayCount != 0)
            {
                products = products.Take(_maxShownDisplayCount);
            }
            return products.AsEnumerable();
        }

        public Products Get(int id)
        {
            return _dbContext.Products.Include(p => p.Supplier).Include(p => p.Category).FirstOrDefault(p => p.ProductId == id);
        }

        public Products Add(Products product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            return product;
        }

        public Products Update(Products product)
        {
            _dbContext.Products.Attach(product).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return product;
        }

        public IEnumerable<Categories> GetAllCategories()
        {
            return _dbContext.Categories.OrderBy(p => p.CategoryId);
        }

        public IEnumerable<Suppliers> GetAllSuppliers()
        {
            return _dbContext.Suppliers.OrderBy(p => p.SupplierId);
        }
    }
}

public class SqlProductsOptions
{
    public int MaxShownDisplayCount { get; set; }
}