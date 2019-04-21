using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
using Model.ViewModels;
using System.Data.Entity;

namespace Model.Dao
{
    public class OrderDAO
    {
        //public void CreateOrder()
        //{

        //    //var currentCustomer = (CurrentCustomer)Session[CommonBox.SessionBox.CUSTOMER_SESSION];
        //    //customerID = currentCustomer.GetCustomerID();
        //    using (var db = new BookManagementEntities())
        //    {
        //        using (var dbContextTransaction = db.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                int customerID = 0;
        //                Order order = new Order();
        //                order.AccountCustomerID = customerID;
        //                order.OrderDate = DateTime.Now;
        //                order.TotalMoney = 0;
        //                order.ModifiredDate = DateTime.Now;
        //                order.OrderStatusID = 1;
        //                order.IsActive = true;
        //                db.Orders.Add(order);
        //                db.SaveChanges();
        //            }
        //            catch (Exception)
        //            {
        //                dbContextTransaction.Rollback();
        //            }
        //        }
        //    }
        //}
        //public void CreateOrder_Detail(int bookID, int quantity)
        //{
        //    using (var db = new BookManagementEntities())
        //    using (var dbContextTransaction = db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            OrderDetail orderDetail = new OrderDetail();
        //            var order = db.Orders.Where(s => s.IsActive == true).OrderByDescending(s => s.OrderID).FirstOrDefault(); // info of order
        //            var detailBook = db.Books.Where(s => s.BookID == bookID && s.IsActive == true).FirstOrDefault(); // info of book
        //            var money = detailBook.Price * quantity; // money
        //            orderDetail.OrderID = Convert.ToInt32(order.OrderID);
        //            orderDetail.BookID = bookID;
        //            orderDetail.Quantity = quantity;
        //            orderDetail.Money = money;
        //            orderDetail.IsActive = true;
        //            db.OrderDetails.Add(orderDetail);
        //            db.SaveChanges();
        //            detailBook.Quantity -= quantity; // update quantity of book after add orderDetail
        //            order.TotalMoney += money;  //upadte total money of Order after add orderDetail
        //            db.SaveChanges();
        //        }
        //        catch (Exception)
        //        {
        //            dbContextTransaction.Rollback();
        //        }
        //    }
        //}
        public IEnumerable<OrderAccountModel> LoadOrder()
        {
            using (var db = new BookManagementEntities())
            {
                var list = (from a in db.Orders
                            join b in db.Accounts
                            on a.AccountCustomerID equals b.AccountID
                            join c in db.OrderStatus on a.OrderStatusID equals c.OrderStatusID
                            where a.IsActive == true
                            orderby a.OrderDate descending
                            select new OrderAccountModel
                            {
                                OrderID = a.OrderID,
                                OrderStatusID = c.OrderStatusID,
                                AccountCustomerName = b.AccountName,
                                OrderStatusName = c.OrderStatusName,
                                OrderDate = a.OrderDate,
                                ModifiedDate = a.ModifiredDate,
                                TotalMoney = a.TotalMoney
                            });

                return list.ToList();
            }
        }
        public void UpdateOrderStatus(int orderID, int idStatus)
        {
            using (var db = new BookManagementEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        
                        var order = db.Orders.Where(s => s.OrderID == orderID).FirstOrDefault();
                        if (idStatus == 1)
                        {
                            order.OrderStatusID = 2;
                            order.ModifiredDate = DateTime.Now;
                        }
                        else
                        {
                            order.OrderStatusID = 3;
                            order.ModifiredDate = DateTime.Now;
                        }
                        db.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }

        }
        public IEnumerable<OrderDetailBookModel> LoadOrderDetail(int orderID)
        {
            using (var db = new BookManagementEntities())
            {
                var list = (from a in db.OrderDetails
                            join b in db.Books
                            on a.BookID equals b.BookID
                            where a.IsActive == true && a.OrderID == orderID
                            select new OrderDetailBookModel
                            {
                                BookName = b.Title,
                                Quantity = a.Quantity,
                                Money = a.Money
                            }).ToList();
                return list;
            }
        }
        public OrderDetailCustomer GetInfoCustomer(int orderID)
        {
            using (var db = new BookManagementEntities())
            {
                var customer = (from a in db.Orders
                                join b in db.Customers
                                on a.AccountCustomerID equals b.AccountID
                                where a.OrderID == orderID
                                select new OrderDetailCustomer
                                {
                                    CustomerName = b.CustomerName,
                                    Email = b.CustomerEmail,
                                    Phone = a.PhoneNumber,
                                    Address = a.OrderAddress
                                }).SingleOrDefault();
                return customer;
            }
        }
        public void CancelOrder(int orderID)
        {
            using (var db = new BookManagementEntities())
            {
                var order = db.Orders.Where(s => s.OrderID == orderID).FirstOrDefault();
                order.OrderStatusID = 4;
                order.ModifiredDate = DateTime.Now;
                var orderDetail = db.OrderDetails.Where(s => s.OrderID == orderID).ToList();
                foreach (var item in orderDetail)
                {
                    var book = db.Books.Where(s => s.BookID == item.BookID).FirstOrDefault();
                    book.Quantity += item.Quantity;

                }
                db.SaveChanges();
            }
        }
        public int GetQuantityConfirm()
        {
            using (var db = new BookManagementEntities())
            {
                var res = db.Orders.Where(s => s.IsActive == true && (s.OrderStatusID == 1 || s.OrderStatusID == 2)).Count();
                return res;
            }
        }
        //get order by account id
        public static List<Order> GetOrder(int id)
        {
            using (var db=new BookManagementEntities())
            {
                try
                {
                    return db.Orders.Where(m => m.AccountCustomerID == id).Include(m => m.OrderDetails.Select(c=>c.Book)).Include(m=>m.OrderStatu).OrderByDescending(m=>m.ModifiredDate).ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new List<Order>();
                }
            }
        }
    }
}
