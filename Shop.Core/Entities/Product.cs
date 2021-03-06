using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Core.Entities
{
    public class Product:BaseEntity
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public bool DisplayStatus { get; set; }
        public bool IsDeleted { get; set; }
        public Category Category { get; set; }
    }
}
