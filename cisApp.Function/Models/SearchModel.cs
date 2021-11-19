using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Function
{
    public class SearchModel
    {
        public string text { get; set; }
        public int? currentPage { get; set; } = 1;
        public int? pageSize { get; set; } = 10;
        public int status { get; set; } = 0;

    }
}
