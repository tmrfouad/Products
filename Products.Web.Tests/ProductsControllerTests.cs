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
            var manager = new Mock<ProductManager>();
            manager.Setup(m => m.GetProducts()).Returns(dbProducts);

            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.GetProducts();
            task.Wait();
            var result = task.Result;
            var products = ((JsonResult)result.Result).Value as List<ProductDTO>;

            // Assert
            CollectionAssert.AreEqual(dbProducts, products);
        }

        [TestMethod]
        public void SearchProducts_Should_Return_All_Products_If_Search_Is_Empty()
        {
            // Arrange
            var dbProducts = Fixtures.Products;
            var manager = new Mock<ProductManager>();
            manager.Setup(m => m.SearchProducts("")).Returns(dbProducts);

            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.SearchProducts("");
            task.Wait();
            var result = task.Result;
            var products = ((JsonResult)result.Result).Value as List<ProductDTO>;

            // Assert
            CollectionAssert.AreEqual(dbProducts, products);
        }

        [TestMethod]
        public void SearchProducts_Should_Return_All_Products_If_Search_Is_Not_Empty()
        {
            // Arrange
            var dbProducts = Fixtures.Products;
            var manager = new Mock<ProductManager>();
            manager.Setup(m => m.SearchProducts("test")).Returns(dbProducts.Where(p => p.Name.ToLower().Contains("test")).ToList());

            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.SearchProducts("test");
            task.Wait();
            var result = task.Result;
            var products = ((JsonResult)result.Result).Value as List<ProductDTO>;

            // Assert
            CollectionAssert.AreEqual(new List<ProductDTO> { dbProducts[5], dbProducts[6] }, products);
        }

        [TestMethod]
        public void SearchProducts_Should_Return_Not_Found_If_No_Matches()
        {
            // Arrange
            var dbProducts = Fixtures.Products;
            var manager = new Mock<ProductManager>();
            manager.Setup(m => m.SearchProducts("test1")).Returns(dbProducts.Where(p => p.Name.ToLower().Contains("test1")).ToList());

            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.SearchProducts("test1");
            task.Wait();
            var result = task.Result;
            var actionResult = (NotFoundObjectResult)result.Result;

            // Assert
            Assert.AreEqual(StatusCodes.Status404NotFound, actionResult.StatusCode);
        }

        [TestMethod]
        public void GetProduct_Should_Return_Product_If_It_Exists()
        {
            // Arrange
            var dbProducts = Fixtures.Products;
            var manager = new Mock<ProductManager>();
            manager.Setup(m => m.GetProduct(2)).Returns(dbProducts.FirstOrDefault(p => p.ID == 2));

            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.GetProduct(2);
            task.Wait();
            var result = task.Result;
            var product = ((JsonResult)result.Result).Value as ProductDTO;

            // Assert
            Assert.AreEqual(dbProducts[1], product);
        }

        [TestMethod]
        public void GetProduct_Should_Return_Not_Found_If_It_Does_Not_Exists()
        {
            // Arrange
            var dbProducts = Fixtures.Products;
            var manager = new Mock<ProductManager>();
            manager.Setup(m => m.GetProduct(10)).Returns(dbProducts.FirstOrDefault(p => p.ID == 10));

            var logger = new Mock<ILogger<ProductsController>>();

            var controller = new ProductsController(manager.Object, logger.Object);

            // Act
            var task = controller.GetProduct(10);
            task.Wait();
            var result = task.Result;
            var actionResult = (NotFoundObjectResult)result.Result;

            // Assert
            Assert.AreEqual(StatusCodes.Status404NotFound, actionResult.StatusCode);
        }
    }
}
