using System.Collections.Generic;
using WebApp01Introduction.Models;

namespace WebApp01Introduction.Services
{
    public interface IProductsService
    {
        IEnumerable<Products> GetAll();
        Products Get(int id);
        Products Add(Products product);
        Products Update(Products product);
        IEnumerable<Suppliers> GetAllSuppliers();
        IEnumerable<Categories> GetAllCategories();
    }
}