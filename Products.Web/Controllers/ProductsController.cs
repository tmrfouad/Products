using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Products.Business.Managers;
using Microsoft.Extensions.Logging;
using System.Linq;
using Products.Business.DTOs;
using System;
using Microsoft.AspNetCore.Http;

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
            try
            {
                _logger.LogInformation("Fetching all products from database");
                var products = _productManager.GetProducts();
                _logger.LogInformation("Returning '{0}' product(s)", products.Count());

                return Json(products);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting products!");
            }
        }

        [HttpGet("search/{name?}")]
        public ActionResult<IEnumerable<ProductDTO>> SearchProducts(string name)
        {
            try
            {
                _logger.LogInformation("Searching products by the value '{0}' from database", name);
                var products = _productManager.SearchProducts(name);

                _logger.LogInformation("Returning '{0}' product(s)", products.Count());

                if (!products.Any())
                    return NotFound("No products found for this search criteria!");

                return Json(products);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting products!");
            }
        }

        [HttpGet("{productId}")]
        public ActionResult<ProductDTO> GetProduct(int productId)
        {
            try
            {
                if (productId == 0)
                    return BadRequest("No product id provided!");

                _logger.LogInformation("Fetching products with id '{0}' from database", productId);
                var product = _productManager.GetProduct(productId);
                _logger.LogInformation("Returning '{0}' product(s)", product == null ? 0 : 1);

                if (product == null)
                    return NotFound("No product found with this id!");

                return Json(product);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting product!");
            }
        }

        [HttpPost]
        public ActionResult<ProductDTO> AddProduct([FromBody] ProductDTO product)
        {
            try
            {
                if (product == null)
                    return BadRequest("No product provided!");

                _logger.LogInformation("Adding new product");
                var newProduct = _productManager.AddProduct(product);
                _logger.LogInformation("Product was added with id: '{0}'", newProduct.ID);

                return Json(newProduct);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error adding product!");
            }
        }

        [HttpPatch("{productId}")]
        public ActionResult<ProductDTO> UpdateProduct(int productId, [FromBody] ProductDTO updates)
        {
            try
            {
                if (productId == 0)
                    return BadRequest("No product id provided!");

                if (updates == null)
                    return BadRequest("No updates provided!");

                _logger.LogInformation("Updating product with id: '{0}'", productId);
                var newProduct = _productManager.UpdateProduct(productId, updates);
                _logger.LogInformation("Product with id: '{0}' was modified", productId);

                return Json(newProduct);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating product!");
            }
        }

        [HttpDelete("{productId}")]
        public ActionResult DeleteProduct(int productId)
        {
            productId = 0;
            try
            {
                if (productId == 0)
                    return BadRequest("No product id provided!");

                _logger.LogInformation("Deleteing product with id: '{0}'", productId);
                _productManager.DeleteProduct(productId);
                _logger.LogInformation("Product with id: {0} was deleted", productId);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting product!");
            }
        }
    }
}
