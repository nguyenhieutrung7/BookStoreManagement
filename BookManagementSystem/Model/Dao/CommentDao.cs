using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
using Model.ViewModels;
namespace Model.Dao
{
    public class CommentDao
    {
        // Load data comment
        public IEnumerable<CommentBookModels> Load()
        {
            using (var db = new BookManagementEntities())
            {
                var list = (from a in db.Comments
                            join b in db.Books
                             on a.BookID equals b.BookID
                            where a.IsActive == true
                            orderby a.CreatedDate descending
                            select new CommentBookModels
                            {
                                Title = b.Title,
                                Content = a.CommentContent,
                                CreatedDate = a.CreatedDate,
                                CommentID=a.CommentID,
                                BookID=b.BookID
                            }).ToList();
                return list;
            }

            

        }
        // Load data Reply
        public IEnumerable<ReplyAccount> LoadReply(int idcomment)
        {
            using (var db = new BookManagementEntities())
            {
                var list = (from a in db.Replies
                            join b in db.Accounts
                            on a.AccountID equals b.AccountID                         
                            where a.CommentID==idcomment
                            orderby a.ReplyDate descending
                            select new ReplyAccount
                            {
                                AccountName = b.AccountName,
                                ReplyContent = a.ReplyContent,
                                ReplyDate = a.ReplyDate
                            }).ToList();
                return list; 
            }
        }
        // Add Reply
        public void AddReply(string replyContent,int accountID,int idComment)
        {
            using (var db = new BookManagementEntities())
            {
                Reply res = new Reply();
                res.CommentID = idComment;
                res.AccountID = accountID;
                res.ReplyContent = replyContent;
                res.ReplyDate = DateTime.Now;
                db.Replies.Add(res);
                db.SaveChanges();
            }         
        }
        //Delete comment
        public void DeleteComment(int idComment)
        {
            using (var db = new BookManagementEntities())
            {
                var res = db.Comments.Where(s => s.CommentID == idComment).FirstOrDefault();
                res.IsActive = false;
                db.SaveChanges();
            }
        }
    }
}
