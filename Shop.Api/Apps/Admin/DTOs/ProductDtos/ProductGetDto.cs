using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Api.Apps.Admin.DTOs.ProductDtos
{
    public class ProductGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Profit { get; set; }
        public bool DisplayStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public CategoryInProductGetDto Category { get; set; }
    }

    public class CategoryInProductGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductCounts { get; set; }
    }
}
