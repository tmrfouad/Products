using Products.Business.DTOs;
using Products.Data.Repositories;
using System.Collections.Generic;
using AutoMapper;
using Products.Data.Entities;

namespace Products.Business.Managers
{
    public interface IProductManager
    {
        IEnumerable<ProductDTO> GetProducts();

        IEnumerable<ProductDTO> SearchProducts(string Name);

        ProductDTO GetProduct(int ProductId);

        ProductDTO AddProduct(ProductDTO product);

        ProductDTO UpdateProduct(int ProductId, ProductDTO updates);

        void DeleteProduct(int ProductId);
    }
}
