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
    
    public partial class PRODUCT_REVIEWS
    {
        public long PRODUCT_REVIEW_ID { get; set; }
        public long USER_ID { get; set; }
        public long PRODUCT_ID { get; set; }
        public Nullable<int> REVIEW { get; set; }
        public string COMMENT { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public Nullable<System.DateTime> MODIFIED_DATE { get; set; }
        public string RECORD_STATUS { get; set; }
    }
}
