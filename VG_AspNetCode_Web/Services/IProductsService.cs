using System.Collections.Generic;
using VG_AspNetCore_Web.Models;

namespace VG_AspNetCore_Web.Services
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