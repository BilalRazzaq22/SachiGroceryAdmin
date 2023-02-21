using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROCERY.Models
{
    public class BookViewModel
    {
        public long Id
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public bool IsWritten
        {
            get;
            set;
        }
    }
}