using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels
{
    public class OrderDetailBookModel
    {
        public string BookName { get; set; }
        public int Quantity { get; set; }
        public decimal Money { get; set; }
    }
}
