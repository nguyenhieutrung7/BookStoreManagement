using Model.Models;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class AuthorDao
    {
        public static List<Author> GetListAuthors()
        {
            using (var db=new BookManagementEntities())
            {
                try
                {
                    return db.Authors.Where(m=>m.IsActive).ToList();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return new List<Author>();
                }
            }
        }
        public static int CreateOrEdit(Author author)
        {
            using (var db=new BookManagementEntities())
            {
                try
                {
                    db.Entry(author).State = author.AuthorID > 0 ? EntityState.Modified : EntityState.Added;
                    int ret = db.SaveChanges();
                    return ret;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }
            }
        }
        public static int Delete(int id)
        {
            using (var db=new BookManagementEntities())
            {
                try
                {
                    var author = db.Authors.FirstOrDefault(m => m.AuthorID == id);
                    author.IsActive = false;
                    db.Entry(author).State = EntityState.Modified;
                    int ret = db.SaveChanges();
                    return ret;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }
            }
        } 
        public static Author GetAuthor(int id)
        {
            using (var db = new BookManagementEntities())
            {
                try
                {
                    var a = db.Authors.Where(m => m.AuthorID == id).Include(b=>b.Books).FirstOrDefault();
                    return a;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }
        
     }
}
