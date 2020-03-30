using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VG_AspNetCore_Web.ViewModels
{
    public class CategoriesEditModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}