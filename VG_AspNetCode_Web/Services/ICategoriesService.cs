using System.Collections.Generic;
using VG_AspNetCore_Web.Models;

namespace VG_AspNetCore_Web.Services
{
    public interface ICategoriesService
    {
        IEnumerable<Categories> GetAll();
        Categories Get(int id);
    }
}