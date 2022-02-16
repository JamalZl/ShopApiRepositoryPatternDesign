using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Api.Apps.Admin.DTOs
{
    public class ListDto<TItem>
    {
        public int TotalCount { get; set; }
        public List<TItem> Items { get; set; }
    }
}
