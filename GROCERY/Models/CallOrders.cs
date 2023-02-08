using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROCERY.Models
{
    public class CallOrders
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int amountOrdered { get; set; }
        public decimal? availableQty { get; set; }

        public decimal? price { get; set; }
        public decimal? disc { get; set; }
        public decimal? total { get; set; }
        public string barcode { get; set; }
        
    }
}