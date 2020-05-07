using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VG_AspNetCore_Web.Models;

namespace VG_AspNetCore_ConsoleClient
{
    internal class Program
    {
        private static HttpClient client = new HttpClient();

        private static void ShowCategory(Categories category)
        {
            Console.WriteLine($"{category.CategoryId}\t{category.CategoryName}\t{category.Description}");
        }

        private static void ShowProduct(Products product)
        {
            Console.WriteLine($"{product.ProductId}\t{product.ProductName}\t{product.CategoryId}\t{product.Discontinued}\t{product.QuantityPerUnit}\t{product.ReorderLevel}\t{product.SupplierId}\t{product.UnitPrice}\t{product.UnitsInStock}\t{product.UnitsOnOrder}");
        }

        private static async Task<IEnumerable<Categories>> GetCategories()
        {
            HttpResponseMessage response = await client.GetAsync($"api/category");
            response.EnsureSuccessStatusCode();

            var categories = await response.Content.ReadAsAsync<IEnumerable<Categories>>();
            return categories;
        }

        private static async Task<IEnumerable<Products>> GetProducts()
        {
            HttpResponseMessage response = await client.GetAsync($"api/product");
            response.EnsureSuccessStatusCode();

            var products = await response.Content.ReadAsAsync<IEnumerable<Products>>();
            return products;
        }

        private static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:2904/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var categories = await GetCategories();
                Console.WriteLine("=============Categories:=============");
                Console.WriteLine($"CategoryId: \tCategoryName: \tDescription:");
                foreach (var category in categories)
                {
                    ShowCategory(category);
                }
                var products = await GetProducts();
                Console.WriteLine("=============Products:=============");
                Console.WriteLine($"ProductId: \tProductName: \tCategoryId: \tDiscontinued: \tQuantityPerUnit: \tReorderLevel: \tSupplierId: \tUnitPrice: \tUnitsInStock: \tUnitsOnOrder:");
                foreach (var product in products)
                {
                    ShowProduct(product);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }
}