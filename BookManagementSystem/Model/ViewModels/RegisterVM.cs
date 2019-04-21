using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels
{
    public class RegisterVM
    {
        public string username { get; set; }
        public string phonenumber { get; set; }
        public string address { get; set; }
        public DateTime birthday { get; set; }
        public int genderid { get; set; }
        public string email { get; set; }
        public string accountname { get; set; }
        public string password { get; set; }
    }
}
