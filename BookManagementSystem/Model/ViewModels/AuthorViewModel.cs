using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels
{
    public class AuthorViewModel
    {
        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
        public string AuthorPhone { get; set; }
        public bool IsActive { get; set; }
        public List<string> ListBooksTitle { get; set; }
    }
}
