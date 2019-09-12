using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Products.Business.Managers;
using Microsoft.Extensions.Logging;
using System.Linq;
using Products.Business.DTOs;

namespace Products.Web.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private ProductManager _productManager;
        private ILogger<ProductsController> _logger;

        public ProductsController(ProductManager productManager, ILogger<ProductsController> logger)
        {
            _productManager = productManager;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDTO>> GetProducts()
        {
            _logger.LogInformation("Fetching all products from database");
            var products = _productManager.GetProducts();
            _logger.LogInformation("Returning '{0}' product(s)", products.Count());
            return Json(products);
        }

        [HttpGet("search/{name?}")]
        public ActionResult<IEnumerable<ProductDTO>> SearchProducts(string name)
        {
            _logger.LogInformation("Searching products by the value '{0}' from database", name);
            var products = _productManager.SearchProducts(name);
            _logger.LogInformation("Returning '{0}' product(s)", products.Count());
            return Json(products);
        }

        [HttpGet("{productId}")]
        public ActionResult<ProductDTO> GetProduct(int productId)
        {
            _logger.LogInformation("Fetching products with id '{0}' from database", productId);
            var product = _productManager.GetProduct(productId);
            _logger.LogInformation("Returning '{0}' product(s)", product == null ? 0 : 1);
            return Json(product);
        }

        [HttpPost]
        public ActionResult<ProductDTO> AddProduct([FromBody] ProductDTO product)
        {
            _logger.LogInformation("Added new product");
            var newProduct = _productManager.AddProduct(product);
            _logger.LogInformation("Product was added with id: '{0}'", newProduct.ID);
            return Json(newProduct);
        }

        [HttpPatch("{productId}")]
        public ActionResult<ProductDTO> UpdateProduct(int productId, [FromBody] ProductDTO updates)
        {
            _logger.LogInformation("Updating product with id: '{0}'", productId);
            var newProduct = _productManager.UpdateProduct(productId, updates);
            _logger.LogInformation("Product with id: '{0}' was modified", productId);
            return Json(newProduct);
        }

        [HttpDelete("{productId}")]
        public void DeleteProduct(int productId)
        {
            _logger.LogInformation("Deleteing product with id: '{0}'", productId);
            _productManager.DeleteProduct(productId);
            _logger.LogInformation("Product with id: {0} was deleted", productId);
        }
    }
}
