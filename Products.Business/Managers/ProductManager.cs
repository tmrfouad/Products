using Products.Business.DTOs;
using Products.Data.Repositories;
using System.Collections.Generic;
using AutoMapper;
using Products.Data.Entities;

namespace Products.Business.Managers
{
    public class ProductManager
    {
        private IProductRepository _repository;
        private IMapper _mapper;

        public ProductManager(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<ProductDTO> GetProducts()
        {
            return _mapper.Map<IEnumerable<ProductDTO>>(_repository.GetAll());
        }

        public IEnumerable<ProductDTO> SearchProducts(string Name)
        {
            return _mapper.Map<IEnumerable<ProductDTO>>(_repository.Search(Name));
        }

        public ProductDTO GetProduct(int ProductId)
        {
            return _mapper.Map<ProductDTO>(_repository.Get(ProductId));
        }

        public ProductDTO AddProduct(ProductDTO product)
        {
            return _mapper.Map<ProductDTO>(_repository.Add(_mapper.Map<Product>(product)));
        }

        public ProductDTO UpdateProduct(int ProductId, ProductDTO updates)
        {
            return _mapper.Map<ProductDTO>(_repository.Update(ProductId, _mapper.Map<Product>(updates)));
        }

        public void DeleteProduct(int ProductId)
        {
            _repository.Delete(ProductId);
        }
    }
}
