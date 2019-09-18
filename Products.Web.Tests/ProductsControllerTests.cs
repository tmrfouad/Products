using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Products.Business.DTOs;
using Products.Business.Managers;
using Products.Data.Repositories;
using Products.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Web.Tests
{
    [TestClass]
    public class ProductsControllerTests
    {
        [TestMethod]
        public void GetProducts_Should_Return_All_Products()
        {
            // Arrange
            var dbProducts = Fixtures.Products;
            var manager = new Mock<IProductManager>();
            manager.Setup(m => m.GetProducts()).Returns(Task.FromResult(dbProducts.AsEnumerable()));

            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.GetProducts();
            task.Wait();
            var result = task.Result;
            var actionResult = (ObjectResult)result.Result;
            var products = actionResult.Value as List<ProductDTO>;

            // Assert
            Assert.AreEqual(StatusCodes.Status200OK, actionResult.StatusCode);
            CollectionAssert.AreEqual(dbProducts, products);
        }

        [TestMethod]
        public void SearchProducts_Should_Return_All_Products_If_Search_Is_Empty()
        {
            // Arrange
            var dbProducts = Fixtures.Products;
            var search = "";
            var manager = new Mock<IProductManager>();
            manager.Setup(m => m.SearchProducts(search)).Returns(Task.FromResult(dbProducts.AsEnumerable()));

            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.SearchProducts(search);
            task.Wait();
            var result = task.Result;
            var actionResult = (ObjectResult)result.Result;
            var products = actionResult.Value as List<ProductDTO>;

            // Assert
            Assert.AreEqual(StatusCodes.Status200OK, actionResult.StatusCode);
            CollectionAssert.AreEqual(dbProducts, products);
        }

        [TestMethod]
        public void SearchProducts_Should_Return_Filtered_Products_If_Search_Is_Not_Empty()
        {
            // Arrange
            var dbProducts = Fixtures.Products;
            var search = "test";
            var manager = new Mock<IProductManager>();
            manager.Setup(m => m.SearchProducts(search)).Returns(Task.FromResult(dbProducts.Where(p => p.Name.ToLower().Contains(search))));

            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.SearchProducts(search);
            task.Wait();
            var result = task.Result;
            var actionResult = (ObjectResult)result.Result;
            var products = actionResult.Value as IEnumerable<ProductDTO>;

            // Assert
            Assert.AreEqual(StatusCodes.Status200OK, actionResult.StatusCode);
            CollectionAssert.AreEqual(new List<ProductDTO> { dbProducts[5], dbProducts[6] }, products.ToList());
        }

        [TestMethod]
        public void GetProduct_Should_Return_Product_If_It_Exists()
        {
            // Arrange
            var dbProducts = Fixtures.Products;
            var productId = 2;
            var manager = new Mock<IProductManager>();
            manager.Setup(m => m.GetProduct(productId)).Returns(Task.FromResult(dbProducts.FirstOrDefault(p => p.ID == productId)));

            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.GetProduct(productId);
            task.Wait();
            var result = task.Result;
            var actionResult = (ObjectResult)result.Result;
            var product = actionResult.Value as ProductDTO;

            // Assert
            Assert.AreEqual(StatusCodes.Status200OK, actionResult.StatusCode);
            Assert.AreEqual(dbProducts[1], product);
        }

        [TestMethod]
        public void GetProduct_Should_Return_Bad_Request_If_ProductId_Is_Zero()
        {
            // Arrange
            var dbProducts = Fixtures.Products;
            var productId = 0;
            var manager = new Mock<IProductManager>();
            manager.Setup(m => m.GetProduct(productId)).Returns(Task.FromResult(dbProducts.FirstOrDefault(p => p.ID == productId)));

            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.GetProduct(productId);
            task.Wait();
            var result = task.Result;
            var actionResult = (ObjectResult)result.Result;

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, actionResult.StatusCode);
        }

        [TestMethod]
        public void GetProduct_Should_Return_Not_Found_If_It_Does_Not_Exists()
        {
            // Arrange
            var dbProducts = Fixtures.Products;
            var productId = 10;
            var manager = new Mock<IProductManager>();
            manager.Setup(m => m.GetProduct(productId)).Returns(Task.FromResult(dbProducts.FirstOrDefault(p => p.ID == productId)));

            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.GetProduct(productId);
            task.Wait();
            var result = task.Result;
            var actionResult = (ObjectResult)result.Result;

            // Assert
            Assert.AreEqual(StatusCodes.Status404NotFound, actionResult.StatusCode);
        }

        [TestMethod]
        public void AddProduct_Should_Return_Added_Product_If_Product_Is_Not_Null()
        {
            // Arrange
            var dbProducts = Fixtures.Products;
            var newProduct = new ProductDTO { ID = 8, Name = "New Product", Price = 10 };
            var manager = new Mock<IProductManager>();
            manager.Setup(m => m.AddProduct(newProduct)).Returns(() => 
            {
                dbProducts.Add(newProduct);
                return Task.FromResult(dbProducts.FirstOrDefault(p => p.ID == dbProducts.Count));
            });

            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.AddProduct(newProduct);
            task.Wait();
            var result = task.Result;
            var actionResult = (ObjectResult)result.Result;

            // Assert
            Assert.AreEqual(StatusCodes.Status200OK, actionResult.StatusCode);
            Assert.AreEqual(newProduct, actionResult.Value);
        }

        [TestMethod]
        public void AddProduct_Should_Return_Bad_Request_If_Product_Is_Null()
        {
            // Arrange
            var dbProducts = Fixtures.Products;
            var manager = new Mock<IProductManager>();
            manager.Setup(m => m.AddProduct(null)).Returns(Task.FromResult(dbProducts.FirstOrDefault(p => p.ID == dbProducts.Count)));

            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.AddProduct(null);
            task.Wait();
            var result = task.Result;
            var actionResult = (ObjectResult)result.Result;

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, actionResult.StatusCode);
        }

        [TestMethod]
        public void UpdateProduct_Should_Return_Updated_Product_If_Updates_Is_Not_Null_And_ProductId_Is_Not_Zero()
        {
            // Arrange
            var dbProducts = Fixtures.Products;
            var productId = 3;
            var updates = new ProductDTO { Name = "Updated Product" };
            var manager = new Mock<IProductManager>();
            manager.Setup(m => m.UpdateProduct(productId, updates)).Returns(() =>
            {
                dbProducts.FirstOrDefault(p => p.ID == productId).Name = updates.Name;
                return Task.FromResult(dbProducts.FirstOrDefault(p => p.ID == productId));
            });

            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.UpdateProduct(productId, updates);
            task.Wait();
            var result = task.Result;
            var actionResult = (ObjectResult)result.Result;
            var updatedProduct = (ProductDTO)actionResult.Value;

            // Assert
            Assert.AreEqual(StatusCodes.Status200OK, actionResult.StatusCode);
            Assert.AreEqual(updates.Name, updatedProduct.Name);
        }

        [TestMethod]
        public void UpdateProduct_Should_Return_Bad_Request_If_Updates_Is_Null()
        {
            // Arrange
            var dbProducts = Fixtures.Products;
            var productId = 3;
            var manager = new Mock<IProductManager>();
            manager.Setup(m => m.UpdateProduct(productId, null)).Returns(Task.FromResult(dbProducts.FirstOrDefault(p => p.ID == productId)));

            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.UpdateProduct(productId, null);
            task.Wait();
            var result = task.Result;
            var actionResult = (ObjectResult)result.Result;

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, actionResult.StatusCode);
        }

        [TestMethod]
        public void UpdateProduct_Should_Return_Bad_Request_If_ProductId_Is_Zero()
        {
            // Arrange
            var dbProducts = Fixtures.Products;
            var productId = 0;
            var updates = new ProductDTO { Name = "Updated Product" };
            var manager = new Mock<IProductManager>();
            manager.Setup(m => m.UpdateProduct(productId, updates)).Returns(() =>
            {
                dbProducts.FirstOrDefault(p => p.ID == productId).Name = updates.Name;
                return Task.FromResult(dbProducts.FirstOrDefault(p => p.ID == productId));
            });

            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.UpdateProduct(productId, updates);
            task.Wait();
            var result = task.Result;
            var actionResult = (ObjectResult)result.Result;

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, actionResult.StatusCode);
        }

        [TestMethod]
        public void DeleteProduct_Should_Return_Bad_Request_If_ProductId_Is_Zero()
        {
            // Arrange
            var dbProducts = Fixtures.Products;
            var productId = 0;
            var manager = new Mock<IProductManager>();
            manager.Setup(m => m.DeleteProduct(productId)).Returns(() => Task.FromResult(dbProducts.RemoveAll(p => p.ID == productId) > 0));
            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.DeleteProduct(productId);
            task.Wait();
            var result = task.Result;
            var actionResult = (ObjectResult)result.Result;

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, actionResult.StatusCode);
        }

        [TestMethod]
        public void DeleteProduct_Should_Return_Ok_If_ProductId_Is_Not_Zero()
        {
            // Arrange
            var dbProducts = Fixtures.Products;
            var productId = 4;
            var manager = new Mock<IProductManager>();
            manager.Setup(m => m.DeleteProduct(productId)).Returns(() => Task.FromResult(dbProducts.RemoveAll(p => p.ID == productId) > 0));
            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.DeleteProduct(productId);
            task.Wait();
            var result = task.Result;
            var actionResult = (ObjectResult)result.Result;

            // Assert
            Assert.AreEqual(StatusCodes.Status200OK, actionResult.StatusCode);
            CollectionAssert.DoesNotContain(dbProducts, dbProducts.FirstOrDefault(p => p.ID == productId));
        }
    }
}
