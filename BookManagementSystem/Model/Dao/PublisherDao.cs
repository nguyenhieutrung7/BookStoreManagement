using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class PublisherDao
    {
        //get list publishers which are active
        public static List<Publisher> GetListPublisher()
        {
            using (var db=new BookManagementEntities())
            {
                try
                {
                    return db.Publishers.Where(m=>m.IsActive).ToList();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return new List<Publisher>();
                }
            }
        }
    }
}
