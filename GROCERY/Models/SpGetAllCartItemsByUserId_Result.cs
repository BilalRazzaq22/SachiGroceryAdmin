//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GROCERY.Models
{
    using System;
    
    public partial class SpGetAllCartItemsByUserId_Result
    {
        public int CART_ID { get; set; }
        public Nullable<int> USER_ID { get; set; }
        public Nullable<int> PRODUCT_ID { get; set; }
        public string NAME { get; set; }
        public Nullable<decimal> PRICE { get; set; }
        public Nullable<int> PRICE2 { get; set; }
        public string PACKING { get; set; }
        public string IMAGE_PATH { get; set; }
        public string IMAGE_THUMBNAIL_PATH { get; set; }
        public string PRODUCT_NAME_URL { get; set; }
        public Nullable<int> QUANTITY { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public Nullable<long> RowIndex { get; set; }
        public Nullable<int> TotalRecords { get; set; }
        public Nullable<decimal> TotalPages { get; set; }
        public Nullable<int> Start { get; set; }
        public Nullable<int> End { get; set; }
    }
}