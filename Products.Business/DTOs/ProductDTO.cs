using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Business.DTOs
{
    public class ProductDTO
    {
        public int? ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1, 9999999)]
        public float? Price { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
