using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROCERY.Models
{
    public class PackageModel
    {
        public int PackageId { get; set; }
        public string Name {get;set;}
        public string Desc { get; set; }
        public List<CallOrders> cOrders { get; set; }
    }
}