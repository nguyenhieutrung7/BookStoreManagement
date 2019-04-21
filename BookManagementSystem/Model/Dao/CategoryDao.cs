using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class CategoryDao
    {
        public static List<Category>GetAllCategory()
        {
            using (var db=new BookManagementEntities())
            {
                try
                {
                    var list = db.Categories.ToList();
                    return list;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return new List<Category>();
                }
            }
        }
    }
}
