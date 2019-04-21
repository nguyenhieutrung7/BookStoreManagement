using Model.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Model.Resources;

namespace Model.ViewModels
{
    public class BookViewModel
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string AuthorName { get; set; }
        public string Picture { get; set; }
        public string ISBN { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string PublisherName { get; set; }
        public bool IsActive { get; set; }

        public virtual Author Author { get; set; }
        public virtual Category Category { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
