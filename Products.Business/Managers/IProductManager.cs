using Products.Business.DTOs;
using Products.Data.Repositories;
using System.Collections.Generic;
using AutoMapper;
using Products.Data.Entities;
using System.Threading.Tasks;

namespace Products.Business.Managers
{
    public interface IProductManager
    {
        Task<IEnumerable<ProductDTO>> GetProducts();

        Task<IEnumerable<ProductDTO>> SearchProducts(string Name);

        Task<ProductDTO> GetProduct(int ProductId);

        Task<ProductDTO> AddProduct(ProductDTO product);

        Task<ProductDTO> UpdateProduct(int ProductId, ProductDTO updates);

        Task<bool> DeleteProduct(int ProductId);
    }
}
