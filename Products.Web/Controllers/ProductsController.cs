using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data;
using Products.Data.Entities;
using Products.Business;

namespace Products.Web.Controllers
{
    [Route("api/products")]
    public class ProductsController : Controller
    {
        private ProductManager _productManager;
        public ProductsController()
        {
            _productManager = new ProductManager();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = _productManager.GetProducts();
            return Json(products);
        }

        [HttpGet("/search/{name}")]
        public ActionResult<IEnumerable<Product>> SearchProducts(string name)
        {
            var products = _productManager.SearchProducts(name);
            return Json(products);
        }

        [HttpGet("/{productId}")]
        public ActionResult<Product> GetProduct(int productId)
        {
            var product = _productManager.GetProduct(productId);
            return Json(product);
        }

        [HttpPost]
        public ActionResult<Product> AddProduct(Product product)
        {
            var newProduct = _productManager.AddProduct(product);
            return Json(newProduct);
        }

        [HttpPatch]
        public ActionResult<Product> UpdateProduct(int productId, Product updates)
        {
            var newProduct = _productManager.UpdateProduct(productId, updates);
            return Json(newProduct);
        }

        [HttpDelete]
        public void DeleteProduct(int productId)
        {
            _productManager.DeleteProduct(productId);
        }
    }
}
