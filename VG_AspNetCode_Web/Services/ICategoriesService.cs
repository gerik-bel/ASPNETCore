using System.Collections.Generic;
using System.Threading.Tasks;
using VG_AspNetCore_Web.Models;

namespace VG_AspNetCore_Web.Services
{
    public interface ICategoriesService
    {
        Task<IEnumerable<Categories>> GetAllAsync();
        Task<Categories> GetAsync(int id);
    }
}