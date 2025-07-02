using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNET8.Helpers
{
    public class QueryObject
    {
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Sortby { get; set; } = string.Empty;
        public bool IsDescending { get; set; } = false;
    }
}