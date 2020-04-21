using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using VG_AspNetCore_Data.Models;

namespace VG_AspNetCore_Web.Services
{
    public interface IProductsService
    {
        Task<IEnumerable<Products>> GetAllAsync();
        Task<Products> GetAsync(int id);
        Task<Products> AddAsync(Products product);
        Task<Products> UpdateAsync(Products product);
        Task<List<SelectListItem>> GetSuppliersAsync();
        Task<List<SelectListItem>> GetCategoriesAsync();
    }
}