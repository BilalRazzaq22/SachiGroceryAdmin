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
    
    public partial class BRANCH
    {
        public int BRANCH_ID { get; set; }
        public string BRANCH_NAME { get; set; }
        public Nullable<double> LATITUDE { get; set; }
        public Nullable<double> LONGITUDE { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE { get; set; }
        public Nullable<bool> IS_ACTIVE { get; set; }
    }
}
