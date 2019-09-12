using Products.Data.Entities;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;
using System.Data.SqlClient;

namespace Products.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public SqlConnection _connection { get; set; }

        public ProductRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Product> GetAll()
        {
            var products = new List<Product>();
            using (_connection)
            {
                products = _connection.Query<Product>("GetAllProducts", commandType: CommandType.StoredProcedure).ToList();
            }
            return products;
        }
        public IEnumerable<Product> Search(string search)
        {
            var products = new List<Product>();
            using (_connection)
            {
                products = _connection.Query<Product>("SearchProducts", new { Search = search }, commandType: CommandType.StoredProcedure).ToList();
            }
            return products;
        }
        public Product Get(int id)
        {
            Product product = null;
            using (_connection)
            {
                product = _connection.Query<Product>("GetProduct", new { id }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return product;
        }

        public Product Add(Product entity)
        {
            Product newProduct = null;
            using (_connection)
            {
                newProduct = _connection.Query<Product>("AddProduct", new { entity.Name, entity.Price }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return newProduct;
        }
        public Product Update(int id, Product updates)
        {
            Product updatedProduct = null;
            using (_connection)
            {
                updatedProduct = _connection.Query<Product>("UpdateProduct", new { id, updates.Name, updates.Price }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return updatedProduct;
        }

        public void Delete(int id)
        {
            using (_connection)
            {
                _connection.Execute("DeleteProduct", new { id }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
