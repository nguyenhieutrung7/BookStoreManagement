using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels
{
    public class CurrentSiteOwner
    {
        private string accountName;
        private string userName;
        private int accountID;
        public CurrentSiteOwner()
        {

        }
        public CurrentSiteOwner(string accountName,string userName,int accountID)
        {
            this.accountName = accountName;
            this.userName = userName;
            this.accountID = accountID;
        }
        public string GetUserName()
        {
            return this.userName;
        }
        public int GetAccountID()
        {
            return this.accountID;
        }
    }
}
