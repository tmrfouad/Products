using Products.Data.Entities;
using Products.Data.Repositories;
using System;
using System.Collections.Generic;

namespace Products.Business
{
    public class ProductManager
    {
        public IEnumerable<Product> GetProducts()
        {
            return new ProductRepository().GetProducts();
        }

        public IEnumerable<Product> SearchProducts(string Name)
        {
            return new ProductRepository().SearchProducts(Name);
        }

        public Product GetProduct(int ProductId)
        {
            return new ProductRepository().GetProduct(ProductId);
        }

        public Product AddProduct(Product product)
        {
            return new ProductRepository().AddProduct(product);
        }

        public Product UpdateProduct(int ProductId, Product updates)
        {
            return new ProductRepository().UpdateProduct(ProductId, updates);
        }

        public void DeleteProduct(int ProductId)
        {
            new ProductRepository().DeleteProduct(ProductId);
        }
    }
}
