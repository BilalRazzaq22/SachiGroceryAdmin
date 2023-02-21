using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROCERY.Models
{
    public class MainMenuModel
    {
        public MainMenuModel()
        {
            SubMenus = new List<SubMenuViewModel>();
        }
        public int ID { get; set; }
        public string MainMenu { get; set; }
        public int UserTypeID { get; set; }
        public int UserID { get; set; }
        public bool IsActive { get; set; }
        public IList<SubMenuViewModel> SubMenus { get; set; }
    }
}