using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels
{
    public class ReplyAccount
    {
        public int CommentID { get; set; }
        public string AccountName { get; set; }
        public string ReplyContent { get; set; }
        public DateTime ReplyDate { get; set; }
    }
}
