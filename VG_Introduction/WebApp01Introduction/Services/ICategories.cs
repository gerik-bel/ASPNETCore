using System.Collections.Generic;
using WebApp01Introduction.Models;

namespace WebApp01Introduction.Services
{
    public interface ICategories
    {
        IEnumerable<Categories> GetAll();
        Categories Get(int id);
    }
}