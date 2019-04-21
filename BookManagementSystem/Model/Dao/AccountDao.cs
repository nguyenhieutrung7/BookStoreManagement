using CommonBox;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Model.Dao
{
    public class AccountDao
    {
        public int CheckAccount(string accountName,string password)
        {
            using(var db=new BookManagementEntities())
            {
                var account = db.Accounts.Where(m => m.AccountName == accountName && m.Password == password).FirstOrDefault();
                if (null!=account)
                {
                    if (account.IsActive)
                    {
                        return account.AccountID; //account actived - get account id
                    }
                    else
                    {
                        return -1; // account locked
                    }
                }
                else if(null!= db.Accounts.Where(m => m.AccountName == accountName || m.Password == password).SingleOrDefault())
                {
                    return -2; //username or password not true
                }
                else
                {
                    return 0; //account not exist
                }
            }
        }
        //find customer by customer id
        public static Customer FindCustomer(int id)
        {
            using (var db = new BookManagementEntities())
            {
                try
                {
                    var customer = db.Customers.Include(m=>m.Account).FirstOrDefault(m => m.CustomerID == id);
                    return customer;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }

        }
    }
}
