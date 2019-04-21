using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class BookStock
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string AuthorName { get; set; }
        public string ISBN { get; set; }
        public decimal Price { get; set; }
        public string PublisherName { get; set; }
        public int Quantity { get; set; }
    }
}
