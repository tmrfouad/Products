using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Business.DTOs
{
    public class ProductDTO
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public float? Price { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
