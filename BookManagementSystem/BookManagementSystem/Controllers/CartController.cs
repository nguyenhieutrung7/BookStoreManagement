using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonBox;
using Model.Models;
using Model.Dao;
using System.Data.Entity;
using System.Globalization;
using System.Threading;
using Model.Infrastructure;

namespace BookManagementSystem.Controllers
{
    public class CartController : BaseController
    {
        BookManagementEntities db = new BookManagementEntities();
        // GET: Cart
        public ActionResult Index()
        {
           
            return View("_CartIndex", GetCart());
        }
        private Cart GetCart()
        {
            Cart cart = (Cart)Session[SessionBox.CART_SESSION];
            if (cart == null)
            {
                cart = new Cart();
                Session[SessionBox.CART_SESSION] = cart;
            }
            return cart;
        }
        public ActionResult AddToCart(int bookID, int quantity)
        {
            var model = db.Books.Where(b => b.BookID == bookID).Select(
                b => new BookViewModel
                {
                    BookID = bookID,
                    Title = b.Title,
                    CategoryName = b.Category.CategoryName,
                    Description = b.Description,
                    CreatedDate = b.CreatedDate,
                    ModifiedDate = b.ModifiedDate,
                    ISBN = b.ISBN,
                    PublisherName = b.Publisher.PublisherName,
                    AuthorName = b.Author.AuthorName,
                    Quantity = b.Quantity,
                    Price = b.Price,
                    Picture = b.Picture,
                }).FirstOrDefault();
            GetCart().AddItem(model, quantity);
            return View("_CartIndex", GetCart());
        }
        public ActionResult RemoveFromCart(int bookID)
        {
            GetCart().RemoveItem(bookID);
            return PartialView("_CartIndex", GetCart());
        }
        public ActionResult UpdateFromCart(int bookID, int newQuantity = 0)
        {
            GetCart().UpdateItem(bookID, newQuantity);
            //int sumQuantity = GetCart().ListCartItems.Sum(m => m.Quantity);
            //double sumTotal = Convert.ToDouble(GetCart().ComputeTotalPrice().ToString());
            ////return Json(GetCart(), JsonRequestBehavior.AllowGet);
            //return Json(new { sumQuantity, sumTotal }, JsonRequestBehavior.AllowGet);
            return PartialView("_CartIndex", GetCart());
        }
        public ActionResult ConfirmOrder()
        {
            //require login
            CurrentCustomer currentCustomer = (CurrentCustomer)Session[SessionBox.CUSTOMER_SESSION];
            if (currentCustomer == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Customer customer = AccountDao.FindCustomer(currentCustomer.GetCustomerID());
                if (customer != null)
                {
                    ViewBag.Customer = customer;
                }
                return PartialView("_ConfirmOrder", GetCart());
            }

        }
        public ActionResult CreateOrder(string phoneNumber, string address)
        {
            CurrentCustomer currentCustomer = (CurrentCustomer)Session[SessionBox.CUSTOMER_SESSION];
            if (currentCustomer != null)
            {
                int customerID = currentCustomer.GetCustomerID();
                Customer customer = db.Customers.FirstOrDefault(m => m.CustomerID == customerID);
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        Order order = new Order();
                        order.AccountCustomerID = customer.AccountID;
                        order.OrderDate = DateTime.Now;
                        order.TotalMoney = GetCart().ComputeTotalPrice();
                        order.ModifiredDate = DateTime.Now;
                        order.OrderStatusID = 1;//ordered
                        order.IsActive = true;
                        order.OrderAddress = address;
                        order.PhoneNumber = phoneNumber;
                        db.Orders.Add(order);
                        foreach (var item in GetCart().ListCartItems)
                        {
                            OrderDetail detail = new OrderDetail();
                            detail.OrderID = order.OrderID;
                            detail.Quantity = item.Quantity;
                            detail.IsActive = true;
                            detail.Money = item.Quantity * item.Book.Price;
                            detail.BookID = item.Book.BookID;
                            db.OrderDetails.Add(detail);
                            var book = db.Books.FirstOrDefault(b => b.BookID == item.Book.BookID);
                            book.Quantity -= item.Quantity;
                            db.Entry(book).State = EntityState.Modified;

                        }
                        db.SaveChanges();
                        transaction.Commit();
                        //clear cart
                        GetCart().Clear();
                        return Json(new { id = order.OrderID, email = customer.CustomerEmail }, JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        transaction.Rollback();
                        return Json(new { id = 0, email = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                return Json(new { id = 0, email = "" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}