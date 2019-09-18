using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Products.Business.Managers;
using Microsoft.Extensions.Logging;
using System.Linq;
using Products.Business.DTOs;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Products.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private IProductManager _productManager;
        private ILogger<ProductsController> _logger;

        public ProductsController(IProductManager productManager, ILogger<ProductsController> logger)
        {
            _productManager = productManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            try
            {
                _logger.LogInformation("Fetching all products from database");
                var products = await _productManager.GetProducts();
                _logger.LogInformation("Returning '{0}' product(s)", products.Count());

                return Ok(products);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting products!");
            }
        }

        [HttpGet("search/{name?}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> SearchProducts(string name)
        {
            try
            {
                _logger.LogInformation("Searching products by the name '{0}' from database", name);
                var products = await _productManager.SearchProducts(name);

                _logger.LogInformation("Returning '{0}' product(s)", products.Count());

                return Ok(products);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting products!");
            }
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int productId)
        {
            try
            {
                if (productId == 0)
                    return BadRequest("No product id provided!");

                _logger.LogInformation("Fetching products with id '{0}' from database", productId);
                var product = await _productManager.GetProduct(productId);
                _logger.LogInformation("Returning '{0}' product(s)", product == null ? 0 : 1);

                if (product == null)
                    return NotFound("No product found with this id!");

                return Ok(product);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting product!");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> AddProduct(ProductDTO product)
        {
            try
            {
                if (product == null)
                    return BadRequest("No product provided!");

                _logger.LogInformation("Adding new product");
                var newProduct = await _productManager.AddProduct(product);
                _logger.LogInformation("Product was added with id: '{0}'", newProduct.ID);

                return Ok(newProduct);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error adding product!");
            }
        }

        [HttpPatch("{productId}")]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(int productId, ProductDTO updates)
        {
            try
            {
                if (productId == 0)
                    return BadRequest("No product id provided!");

                if (updates == null)
                    return BadRequest("No updates provided!");

                _logger.LogInformation("Updating product with id: '{0}'", productId);
                var newProduct = await _productManager.UpdateProduct(productId, updates);
                _logger.LogInformation("Product with id: '{0}' was modified", productId);

                return Ok(newProduct);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating product!");
            }
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult<bool>> DeleteProduct(int productId)
        {
            try
            {
                if (productId == 0)
                    return BadRequest("No product id provided!");

                _logger.LogInformation("Deleteing product with id: '{0}'", productId);
                var result = await _productManager.DeleteProduct(productId);
                _logger.LogInformation("Product with id: {0} was deleted", productId);

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting product!");
            }
        }
    }
}
