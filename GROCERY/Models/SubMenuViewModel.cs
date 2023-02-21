using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROCERY.Models
{
    public class SubMenuViewModel
    {
        public int ID { get; set; }
        public string SubMenu { get; set; }
        public int MainMenuID { get; set; }
        public int UserTypeID { get; set; }
        public int UserID { get; set; }
        public bool IsActive { get; set; }
    }
}