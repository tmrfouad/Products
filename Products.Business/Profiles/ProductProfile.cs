using AutoMapper;
using Products.Business.DTOs;
using Products.Data.Entities;

namespace Products.Business.Profiles
{
    class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
        }
    }
}
