using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROCERY.Models
{
    public class CustomerOrderBO
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public List<CallOrders> cOrders { get; set; }
        public bool isTestOrder { get; set; }
        public string DeliveryDescription { get; set; }
        public int PaymentMode { get; set; }
        public int extraDisc { get; set; }
        public string coupon { get; set; }
        public string couponDisc { get; set; }
        public string DeliveryTime { get; set; }
        public int Package { get; set; }
        public decimal totalAmount { get; set; }
        public decimal DeliveryFee { get; set; }
    }
}