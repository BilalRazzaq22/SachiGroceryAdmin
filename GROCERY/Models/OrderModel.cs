using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROCERY.Models
{
    public class OrderModel
    {
        public List<Order> Orders { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int oStID { get; set; }
        public string oDateFrom { get; set; }
        public string oDateTo { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string SearchText { get; set; }
    }

    public class Order
    {
        public int ORDER_ID { get; set; }
        public int CUSTOMER_ID { get; set; }
        public string NAME { get; set; }
        public string MOBILE { get; set; }
        public string ADDRESS { get; set; }
        public string DELIVERY_DESCRIPTION { get; set; }
        public string DELIVERY_TIME { get; set; }
        public int MANUAL_DISCOUNT { get; set; }
        public int COUPON_DISCOUNT { get; set; }
        public int ADDED_BY { get; set; }
        public string STATUS_DESCRIPTION { get; set; }
        public string IS_PACKAGE { get; set; }
        public int TOTAL_ITEMS { get; set; }
        public decimal TOTAL_AMOUNT { get; set; }
    }
}