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
    
    public partial class PS000
    {
        public short LOCNO { get; set; }
        public int ITEM_CODE { get; set; }
        public Nullable<decimal> QTY { get; set; }
        public Nullable<decimal> WH_QTY { get; set; }
        public Nullable<decimal> T_QTY { get; set; }
        public Nullable<byte> FLOOR_NO { get; set; }
        public Nullable<short> COUNTER { get; set; }
        public Nullable<byte> SHELF { get; set; }
        public Nullable<decimal> SHELF_QTY { get; set; }
        public Nullable<decimal> SHELF_FACING { get; set; }
        public Nullable<System.DateTime> LSDATE { get; set; }
        public Nullable<System.DateTime> LPDATE { get; set; }
        public Nullable<bool> ALLOW_NEGATIVE { get; set; }
        public Nullable<decimal> AreaCovered { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<decimal> MAX_QTY { get; set; }
        public Nullable<decimal> MIN_QTY { get; set; }
        public Nullable<decimal> OP_STOCK { get; set; }
        public Nullable<decimal> OP_STOCK_VAL { get; set; }
        public Nullable<bool> NeedsReplication { get; set; }
        public Nullable<bool> NeedsUpdation { get; set; }
    }
}
