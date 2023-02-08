using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROCERY.Models
{
    public class ProductQuantity
    {
        public PRODUCT item { get; set; }
        public List<STOCK> stocks { get; set; }
        public List<BRANCH> branches { get; set; }
    }
}