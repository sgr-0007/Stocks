using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stocks.API.Helpers
{
    public class QueryObject
    {
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Sortby { get; set; } = string.Empty;
        public bool IsDescending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}