using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GROCERY.Models
{
    public class COUPONSBO
    {
        public int COUPON_ID { get; set; }
        public string PROMO_TEXT { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public Nullable<System.DateTime> START_DATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public Nullable<System.DateTime> EXPIRY_DATE { get; set; }
        public Nullable<int> VALUE { get; set; }
        public Nullable<System.DateTime> CREATED_ON { get; set; }
        public Nullable<int> CREATED_BY { get; set; }
        public Nullable<bool> IS_ACTIVE { get; set; }
        public Nullable<bool> IS_USED { get; set; }
        public Nullable<int> UNLOCK_AMOUNT { get; set; }
        public Nullable<int> CODE { get; set; }
        public int count_coupons { get; set; }
        public string promo { get; set; }
    }
}