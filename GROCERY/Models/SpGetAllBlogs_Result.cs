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
    
    public partial class SpGetAllBlogs_Result
    {
        public int BlogId { get; set; }
        public string BlogTitle { get; set; }
        public string BlogHtml { get; set; }
        public string BlogImageUrl { get; set; }
        public string BlogTitleUrl { get; set; }
        public string RecordStatus { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string CreatedDateString { get; set; }
        public Nullable<long> RowIndex { get; set; }
        public Nullable<int> TotalRecords { get; set; }
        public Nullable<decimal> TotalPages { get; set; }
        public Nullable<int> Start { get; set; }
        public Nullable<int> End { get; set; }
    }
}
