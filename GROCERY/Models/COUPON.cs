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
    using System.Collections.Generic;
    
    public partial class COUPON
    {
        public int COUPON_ID { get; set; }
        public string PROMO_TEXT { get; set; }
        public Nullable<System.DateTime> START_DATE { get; set; }
        public Nullable<System.DateTime> EXPIRY_DATE { get; set; }
        public Nullable<int> VALUE { get; set; }
        public Nullable<System.DateTime> CREATED_ON { get; set; }
        public Nullable<int> CREATED_BY { get; set; }
        public Nullable<bool> IS_ACTIVE { get; set; }
        public Nullable<bool> IS_USED { get; set; }
        public Nullable<int> UNLOCK_AMOUNT { get; set; }
        public Nullable<int> CODE { get; set; }
        public string PROMO { get; set; }
        public Nullable<int> updated_by { get; set; }
        public Nullable<System.DateTime> updated_on { get; set; }
    }
}