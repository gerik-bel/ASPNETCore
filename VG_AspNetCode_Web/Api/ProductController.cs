using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;
using VG_AspNetCore_Data.Models;
using VG_AspNetCore_Web.ActionFilters;
using VG_AspNetCore_Web.Services;

namespace VG_AspNetCore_Web.Api
{
    [Route("api/[controller]")]
    [ValidateModel]
    public class ProductController : Controller
    {
        private readonly IProductsService _productsService;

        public ProductController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get(bool includeCategory = false, bool includeSupplier = false, bool includeOrderDetails = false)
        {
            var products = await _productsService.GetAllAsync(includeCategory, includeSupplier, includeOrderDetails);
            return Ok(products);
        }


        [HttpGet("{id}", Name = "ProductGet")]
        public async Task<IActionResult> Get(int id, bool includeCategory = false, bool includeSupplier = false, bool includeOrderDetails = false)
        {
            try
            {
                Products products = await _productsService.GetAsync(id, includeCategory, includeSupplier, includeOrderDetails);
                if (products == null)
                {
                    return NotFound($"Product witn id-{id} was not found");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
            return BadRequest();
        }

        [HttpPost("", Name = "ProductPost")]
        public async Task<IActionResult> Post([FromBody]Products model)
        {
            try
            {
                var product = await _productsService.AddAsync(model);
                return Created(Url.Link("ProductGet", new { id = product.ProductId }), product);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
            return BadRequest();
        }

        [HttpPut("{id}", Name = "ProductPut")]
        public async Task<IActionResult> Put(int id, [FromBody] Products model)
        {
            try
            {
                var product = await _productsService.GetAsync(id);
                if (product == null)
                {
                    return NotFound($"Could not find a product with id - {id}");
                }
                product.CategoryId = model.CategoryId;
                product.Discontinued = model.Discontinued;
                product.ProductName = model.ProductName;
                product.QuantityPerUnit = model.QuantityPerUnit;
                product.ReorderLevel = model.ReorderLevel;
                product.SupplierId = model.SupplierId;
                product.UnitPrice = model.UnitPrice;
                product.UnitsInStock = model.UnitsInStock;
                product.UnitsOnOrder = model.UnitsOnOrder;
                product = await _productsService.UpdateAsync(product);
                return Ok(product);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
            return BadRequest("Could not update product");
        }

        [HttpPatch("{id}", Name = "ProductPatch")]
        public async Task<IActionResult> Patch(int id, [FromBody] Products model)
        {
            try
            {
                var product = await _productsService.GetAsync(id);
                if (product == null)
                {
                    return NotFound($"Could not find a product with id - {id}");
                }
                product.CategoryId = model.CategoryId ?? product.CategoryId;
                product.Discontinued = model.Discontinued;
                product.ProductName = string.IsNullOrEmpty(model.ProductName) ? product.ProductName : model.ProductName;
                product.QuantityPerUnit = string.IsNullOrEmpty(model.QuantityPerUnit) ? product.QuantityPerUnit : model.QuantityPerUnit;
                product.ReorderLevel = model.ReorderLevel ?? product.ReorderLevel;
                product.SupplierId = model.SupplierId ?? product.SupplierId;
                product.UnitPrice = model.UnitPrice ?? product.UnitPrice;
                product.UnitsInStock = model.UnitsInStock ?? product.UnitsInStock;
                product.UnitsOnOrder = model.UnitsOnOrder ?? product.UnitsOnOrder;
                product = await _productsService.UpdateAsync(product);
                return Ok(product);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
            return BadRequest("Could not update product");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _productsService.GetAsync(id);
                if (product == null)
                {
                    return NotFound($"Could not find a product with id - {id}");
                }
                await _productsService.DeleteAsync(product);
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
            return BadRequest("Could not delete product");
        }
    }
}