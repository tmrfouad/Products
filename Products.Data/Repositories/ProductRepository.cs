using Products.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;

namespace Products.Data.Repositories
{
    public class ProductRepository : RepositoryBase
    {
        public IEnumerable<Product> GetProducts()
        {
            var products = new List<Product>();
            using (connection)
            {
                products = connection.Query<Product>("GetAllProducts", commandType: CommandType.StoredProcedure).ToList();
            }
            return products;
        }
        public IEnumerable<Product> SearchProducts(string Name)
        {
            var products = new List<Product>();
            using (connection)
            {
                products = connection.Query<Product>("SearchProducts", new { Name }, commandType: CommandType.StoredProcedure).ToList();
            }
            return products;
        }
        public Product GetProduct(int ProductId)
        {
            Product product = null;
            using (connection)
            {
                product = connection.Query<Product>("GetProduct", new { ProductId }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return product;
        }

        public Product AddProduct(Product product)
        {
            Product newProduct = null;
            using (connection)
            {
                newProduct = connection.Query<Product>("AddProduct", new { product.Name, product.Price }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return newProduct;
        }
        public Product UpdateProduct(int ProductId, Product updates)
        {
            Product updatedProduct = null;
            using (connection)
            {
                updatedProduct = connection.Query<Product>("UpdateProduct", new { ProductId, updates.Name, updates.Price }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return updatedProduct;
        }

        public void DeleteProduct(int ProductId)
        {
            using (connection)
            {
                connection.Execute("DeleteProduct", new { ProductId }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
