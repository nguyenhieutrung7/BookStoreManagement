using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
namespace Model.ViewModels
{
    public class OrderAccountModel
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public string AccountCustomerName { get; set; }
        public string OrderStatusName { get; set; }
        public int OrderStatusID { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public decimal TotalMoney { get; set; }

    }
}
