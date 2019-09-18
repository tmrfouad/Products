using Products.Data.Entities;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace Products.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public SqlConnection _connection { get; set; }

        public ProductRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            var products = new List<Product>();
            using (_connection)
            {
                products = await Task.Run(() => _connection.Query<Product>("GetAllProducts", commandType: CommandType.StoredProcedure).ToList());
            }
            return products;
        }
        public async Task<IEnumerable<Product>> Search(string search)
        {
            var products = new List<Product>();
            using (_connection)
            {
                products = await Task.Run(() => _connection.Query<Product>("SearchProducts", new { Search = search }, commandType: CommandType.StoredProcedure).ToList());
            }
            return products;
        }
        public async Task<Product> Get(int id)
        {
            Product product = null;
            using (_connection)
            {
                product = await Task.Run(() => _connection.Query<Product>("GetProduct", new { ProductID = id }, commandType: CommandType.StoredProcedure).FirstOrDefault());
            }
            return product;
        }

        public async Task<Product> Add(Product entity)
        {
            Product newProduct = null;
            using (_connection)
            {
                newProduct = await Task.Run(() => _connection.Query<Product>("AddProduct", new
                {
                    entity.Name,
                    entity.Price
                }, commandType: CommandType.StoredProcedure).FirstOrDefault());

            }
            return newProduct;
        }
        public async Task<Product> Update(int id, Product updates)
        {
            Product updatedProduct = null;
            using (_connection)
            {
                updatedProduct = await Task.Run(() => _connection.Query<Product>("UpdateProduct", new
                {
                    ProductID = id,
                    updates.Name,
                    updates.Price
                }, commandType: CommandType.StoredProcedure).FirstOrDefault());
            }
            return updatedProduct;
        }

        public async Task<bool> Delete(int id)
        {
            using (_connection)
            {
                return await Task.Run(() => _connection.Execute("DeleteProduct", new { ProductID = id }, commandType: CommandType.StoredProcedure) > 0);
            }
        }
    }
}
