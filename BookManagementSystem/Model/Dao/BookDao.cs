using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Model.ViewModels;
namespace Model.Dao
{
    public class BookDao
    {
        //Get book by book id
        public static Book GetBook(int id)
        {
            using (var db=new BookManagementEntities())
            {
                Book book = null;
                try
                {
                    book = db.Books.Include(m=>m.Category).Include(m=>m.Publisher).Include(m=>m.Author).FirstOrDefault(b => b.BookID == id);
                    return book;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
                
            }
        }
        //get list books which are active
        public static List<Book> GetListBooks()
        {
            using (var db = new BookManagementEntities())
            {
                List<Book> list = new List<Book>();
                try
                {
                    list = db.Books.Include(m=>m.Author).Include(m=>m.Category).Include(m=>m.Publisher).Where(m=>m.IsActive).ToList();
                    return list;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new List<Book>();
                }
            }
        }
        //delete book by book ID
        public static bool DeleteBook(int id)
        {
            using (var db = new BookManagementEntities())
            {
                try
                {
                    //check this book exist in order
                    if (db.OrderDetails.FirstOrDefault(d => d.BookID == id)==null)
                    {
                        return false;
                    }
                    Book book = db.Books.FirstOrDefault(m => m.BookID == id);
                    book.IsActive = false;
                    db.Entry(book).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                
            }
        }
        //create or edit book
        public static int CreateOrEditBook(Book book)
        {
            using (var db=new BookManagementEntities())
            {
                try
                {

                    db.Entry(book).State = book.BookID>0? EntityState.Modified:EntityState.Added;
                    int ret= db.SaveChanges();
                    return ret;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }
            }
        }
        public List<Comment> LoadComment(int id)
        {
            using (var db = new BookManagementEntities())
            {
                var list = db.Comments.Where(s => s.IsActive == true && s.BookID==id).OrderByDescending(s=>s.CreatedDate).ToList();
                return list;
            }
        }
        public List<ReplyAccount> LoadReply()
        {
            using (var db = new BookManagementEntities())
            {
                var list = (from a in db.Replies
                            join b in db.Accounts
                            on a.AccountID equals b.AccountID
                            orderby a.ReplyDate descending
                            select new ReplyAccount
                            {
                                AccountName = b.AccountName,
                                CommentID = a.CommentID,
                                ReplyDate = a.ReplyDate,
                                ReplyContent = a.ReplyContent
                            }).ToList();
                return list;
            }
        }
        public void SendComment(int bookID,string commentContent)
        {
            using (var db = new BookManagementEntities())
            {
                Comment cmt = new Comment();
                cmt.CommentContent = commentContent;
                cmt.CreatedDate = DateTime.Now;
                cmt.BookID = bookID;
                cmt.IsActive = true;
                db.Comments.Add(cmt);
                db.SaveChanges();
            }
        }
        public void UpdateStatusOrder_Customer(int orderID)
        {
            using (var db = new BookManagementEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var order = db.Orders.Where(s => s.OrderID == orderID).FirstOrDefault();
                        order.OrderStatusID = 4;
                        db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();
                        var orderDetail = db.OrderDetails.Where(s => s.OrderID == orderID).ToList();
                        foreach(var item in orderDetail)
                        {
                            var book = db.Books.Where(s => s.BookID == item.BookID).FirstOrDefault();
                            book.Quantity += item.Quantity;
                            
                        }
                        db.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }
    }
}
