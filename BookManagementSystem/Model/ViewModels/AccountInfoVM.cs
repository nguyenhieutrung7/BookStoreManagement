using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels
{
    public class AccountInfoVM
    {
        public int SiteOwnerID { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }
        public string SiteOwnerName { get; set; }
        public string SiteOwnerAddress { get; set; }
        public string SiteOwnerEmail { get; set; }
        public string SiteOwnerPhoneNumber { get; set; }
        public int GenderID { get; set; }
    }
}
