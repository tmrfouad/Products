using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Data.Entities
{
    public class Product
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public float? Price { get; set; }
    }
}
