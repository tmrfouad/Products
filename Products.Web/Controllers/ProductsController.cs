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
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            try
            {
                _logger.LogInformation("Fetching all products from database");
                var products = await Task.Run(() => _productManager.GetProducts());
                _logger.LogInformation("Returning '{0}' product(s)", products.Count());

                return await Task.Run(() => Json(products));
            }
            catch (Exception)
            {
                return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, "Error getting products!"));
            }
        }

        [HttpGet("search/{name?}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> SearchProducts(string name)
        {
            try
            {
                _logger.LogInformation("Searching products by the value '{0}' from database", name);
                var products = await Task.Run(() => _productManager.SearchProducts(name));

                _logger.LogInformation("Returning '{0}' product(s)", products.Count());

                if (!products.Any())
                    return await Task.Run(() => NotFound("No products found for this search criteria!"));

                return await Task.Run(() => Json(products));
            }
            catch (Exception)
            {
                return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, "Error getting products!"));
            }
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int productId)
        {
            try
            {
                if (productId == 0)
                    return await Task.Run(() => BadRequest("No product id provided!"));

                _logger.LogInformation("Fetching products with id '{0}' from database", productId);
                var product = await Task.Run(() => _productManager.GetProduct(productId));
                _logger.LogInformation("Returning '{0}' product(s)", product == null ? 0 : 1);

                if (product == null)
                    return await Task.Run(() => NotFound("No product found with this id!"));

                return await Task.Run(() => Json(product));
            }
            catch (Exception)
            {
                return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, "Error getting product!"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> AddProduct([FromBody] ProductDTO product)
        {
            try
            {
                if (product == null)
                    return await Task.Run(() => BadRequest("No product provided!"));

                _logger.LogInformation("Adding new product");
                var newProduct = await Task.Run(() => _productManager.AddProduct(product));
                _logger.LogInformation("Product was added with id: '{0}'", newProduct.ID);

                return await Task.Run(() => Json(newProduct));
            }
            catch (Exception)
            {
                return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, "Error adding product!"));
            }
        }

        [HttpPatch("{productId}")]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(int productId, [FromBody] ProductDTO updates)
        {
            try
            {
                if (productId == 0)
                    return await Task.Run(() => BadRequest("No product id provided!"));

                if (updates == null)
                    return await Task.Run(() => BadRequest("No updates provided!"));

                _logger.LogInformation("Updating product with id: '{0}'", productId);
                var newProduct = await Task.Run(() => _productManager.UpdateProduct(productId, updates));
                _logger.LogInformation("Product with id: '{0}' was modified", productId);

                return await Task.Run(() => Json(newProduct));
            }
            catch (Exception)
            {
                return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, "Error updating product!"));
            }
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            try
            {
                if (productId == 0)
                    return await Task.Run(() => BadRequest("No product id provided!"));

                _logger.LogInformation("Deleteing product with id: '{0}'", productId);
                await Task.Run(() => _productManager.DeleteProduct(productId));
                _logger.LogInformation("Product with id: {0} was deleted", productId);

                return await Task.Run(() => Ok());
            }
            catch (Exception)
            {
                return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, "Error deleting product!"));
            }
        }
    }
}
