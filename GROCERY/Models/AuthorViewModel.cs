using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROCERY.Models
{
    public class AuthorViewModel
    {
        public AuthorViewModel()
        {
            BookViewModel = new List<BookViewModel>();
        }
        public int Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public bool IsAuthor
        {
            get;
            set;
        }
        public IList<BookViewModel> BookViewModel
        {
            get;
            set;
        }
    }
}