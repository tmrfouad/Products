using Products.Business.DTOs;
using Products.Data.Repositories;
using System.Collections.Generic;
using AutoMapper;
using Products.Data.Entities;
using System.Threading.Tasks;

namespace Products.Business.Managers
{
    public class ProductManager : IProductManager
    {
        private IProductRepository _repository;
        private IMapper _mapper;

        public ProductManager() {}

        public ProductManager(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            return _mapper.Map<IEnumerable<ProductDTO>>(await _repository.GetAll());
        }

        public async Task<IEnumerable<ProductDTO>> SearchProducts(string Name)
        {
            return _mapper.Map<IEnumerable<ProductDTO>>(await _repository.Search(Name));
        }

        public async Task<ProductDTO> GetProduct(int ProductId)
        {
            return _mapper.Map<ProductDTO>(await _repository.Get(ProductId));
        }

        public async Task<ProductDTO> AddProduct(ProductDTO product)
        {
            return _mapper.Map<ProductDTO>(await _repository.Add(_mapper.Map<Product>(product)));
        }

        public async Task<ProductDTO> UpdateProduct(int ProductId, ProductDTO updates)
        {
            return _mapper.Map<ProductDTO>(await _repository.Update(ProductId, _mapper.Map<Product>(updates)));
        }

        public async Task<bool> DeleteProduct(int ProductId)
        {
            return await _repository.Delete(ProductId);
        }
    }
}
