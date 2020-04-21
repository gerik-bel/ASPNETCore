using System.Collections.Generic;
using System.Threading.Tasks;
using VG_AspNetCore_Data.Models;

namespace VG_AspNetCore_Web.Services
{
    public interface ICategoriesService
    {
        Task<IEnumerable<Categories>> GetAllAsync();
        Task<Categories> GetAsync(int id);
        Task<byte[]> GetImageAsync(int id);
        Task UpdateImageAsync(int id, byte[] image);
    }
}