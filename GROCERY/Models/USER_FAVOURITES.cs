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
    
    public partial class USER_FAVOURITES
    {
        public int USER_FAVOURITE_ID { get; set; }
        public Nullable<int> USER_ID { get; set; }
        public Nullable<int> PRODUCT_ID { get; set; }
        public Nullable<System.DateTime> CREATED_ON { get; set; }
        public Nullable<System.DateTime> UPDATED_ON { get; set; }
        public Nullable<int> IS_ACTIVE { get; set; }
    }
}
