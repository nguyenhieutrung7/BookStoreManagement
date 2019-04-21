using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels
{
    public class CurrentCustomer
    {
        private string accountName;
        private string userName;
        private int customerID;
        public CurrentCustomer()
        {

        }
        public CurrentCustomer(string accountName, string userName, int customerID)
        {
            this.accountName = accountName;
            this.userName = userName;
            this.customerID = customerID;
        }
        public string GetUserName()
        {
            return this.userName;
        }
        public int GetCustomerID()
        {
            return this.customerID;
        }
    }
}
