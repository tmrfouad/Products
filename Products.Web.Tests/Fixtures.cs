using Products.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Web.Tests
{
    public static class Fixtures
    {
        public static List<ProductDTO> Products
        {
            get
            {
                return new List<ProductDTO>
                {
                    new ProductDTO{ ID = 1, Name = "Product1", Price = 10 },
                    new ProductDTO{ ID = 2, Name = "Product2", Price = 20 },
                    new ProductDTO{ ID = 3, Name = "Product3", Price = 30 },
                    new ProductDTO{ ID = 4, Name = "Product4", Price = 40 },
                    new ProductDTO{ ID = 5, Name = "Product5", Price = 50 },
                    new ProductDTO{ ID = 6, Name = "ProductTestProduct", Price = 60 },
                    new ProductDTO{ ID = 7, Name = "ProductProductTest", Price = 70 }
                };
            }
        }
    }
}
