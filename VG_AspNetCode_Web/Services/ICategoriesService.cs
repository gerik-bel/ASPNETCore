using System.Collections.Generic;
using System.Threading.Tasks;
using VG_AspNetCore_Data.Models;

namespace VG_AspNetCore_Web.Services
{
    public interface ICategoriesService
    {
        Task<IEnumerable<Categories>> GetAllAsync(bool includeProducts = false);
        Task<Categories> GetAsync(int id, bool includeProducts = false);
        Task<byte[]> GetImageAsync(int id);
        Task<bool> UpdateImageAsync(int id, byte[] image);
    }
}