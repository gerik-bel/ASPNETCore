using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VG_AspNetCore_Web.ViewModels
{
    public class ProductsEditModel
    {
        public int ProductId { get; set; }
        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        [StringLength(20, ErrorMessage = "Name length can't be more than 20.")]
        public string QuantityPerUnit { get; set; }
        [Range(0, 9999999.99)]
        public decimal? UnitPrice { get; set; }
        [Range(0, 32767)]
        public short? UnitsInStock { get; set; }
        [Range(0, 32767)]
        public short? UnitsOnOrder { get; set; }
        [Range(0, 32767)]
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
    }
}