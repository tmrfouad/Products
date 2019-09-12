using Products.Data.Entities;
using Products.Data.Repositories;
using System;
using System.Collections.Generic;

namespace Products.Business.Managers
{
    public class ProductManager
    {
        private IProductRepository _repository;

        public ProductManager()
        {

        }

        public ProductManager(IProductRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _repository.GetAll();
        }

        public IEnumerable<Product> SearchProducts(string Name)
        {
            return _repository.Search(Name);
        }

        public Product GetProduct(int ProductId)
        {
            return _repository.Get(ProductId);
        }

        public Product AddProduct(Product product)
        {
            return _repository.Add(product);
        }

        public Product UpdateProduct(int ProductId, Product updates)
        {
            return _repository.Update(ProductId, updates);
        }

        public void DeleteProduct(int ProductId)
        {
            _repository.Delete(ProductId);
        }
    }
}
