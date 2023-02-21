using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROCERY.Models
{
    public class RiderOrderModel
    {
        public int USER_ID { get; set; }
        public string USERNAME { get; set; }
        public string USER_TYPE_DESCRIPTION { get; set; }
        public bool USER_STATUS { get; set; }
        public string RIDER_TIME_IN { get; set; }
        public string RIDER_TIME_OUT { get; set; }
        public bool IS_RIDER_BACK { get; set; }
    }
}