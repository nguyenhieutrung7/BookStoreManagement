using Model.Dao;
using Model.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Model.ViewModels;
using CommonBox;
using Model.Infrastructure;
using System.Web;
using System.Web.Mvc.Ajax;
using System.Net;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using System.Web.Http.Cors;
using System.Web.Http;

namespace BookManagementSystem.Controllers
{

    public class HomeController : BaseController
    {
        BookManagementEntities db = new BookManagementEntities();
        public ActionResult Index()
        {
            if (HttpContext.Response.Cookies["culture"]!=null && HttpContext.Response.Cookies["culture"].Value == "vi")
            {
                HttpContext.Application["CurrentRateUSD_VND"] = GetCurrencyRate();
                if ((Double)HttpContext.Application["CurrentRateUSD_VND"] == -1)
                {
                    ViewBag.ErrorMessage = "No internet access!";
                    return View("Error");
                }
            }
            else
            {
                HttpContext.Application["CurrentRateUSD_VND"] = Convert.ToDouble(1);
            }
            return View();
        }
        public ActionResult PartialIndex(int? page, int authorID = 0, int categoryID = 0)
        {
            ViewBag.AuthorID = authorID;
            ViewBag.CategoryID = categoryID;
            int pageNumber = page ?? 1;
            var listBooks = BookDao.GetListBooks().ToList();
            if (authorID > 0)
            {
                listBooks = listBooks.Where(m => m.AuthorID == authorID && m.IsActive).ToList();
                categoryID = 0;
            }

            if (categoryID > 0)
            {
                listBooks = listBooks.Where(m => m.CategoryID == categoryID && m.IsActive).ToList();
                authorID = 0;
            }
            ViewBag.CurrentAuthorID = authorID;
            ViewBag.CurrentCategoryID = categoryID;
            Cart cart = (Cart)Session[SessionBox.CART_SESSION];
            if (cart == null)
            {
                cart = new Cart();
            }
            ViewBag.Cart = cart;
            return PartialView("_Index", listBooks.ToPagedList(pageNumber, 8));
        }
        //view detail Book by book ID
        public ActionResult Detail(int id)
        {
            Book book = BookDao.GetBook(id);
            ViewBag.Quantity = book.Quantity;
            Cart cart = (Cart)Session[SessionBox.CART_SESSION];
            if (cart != null)
            {
                foreach (var item in cart.ListCartItems)
                {
                    if (item.Book.BookID == book.BookID)
                    {
                        ViewBag.Quantity -= item.Quantity;
                        break;
                    }
                }
            }
            ViewBag.ID = id;
            return PartialView("_Detail", book);
        }
        public ActionResult DetailBookPage(int id)
        {
            Book book = BookDao.GetBook(id);
            ViewBag.Quantity = book.Quantity;
            Cart cart = (Cart)Session[SessionBox.CART_SESSION];
            if (cart != null)
            {
                foreach (var item in cart.ListCartItems)
                {
                    if (item.Book.BookID == book.BookID)
                    {
                        ViewBag.Quantity -= item.Quantity;
                        break;
                    }
                }
            }
            ViewBag.ID = id;
            return View("Detail", book);
        }
        public ActionResult CommentUser(int id)
        {
            ViewBag.ID = id;
            BookDao dao = new BookDao();
            var cmt = dao.LoadComment(id);
            ViewBag.Reply = dao.LoadReply();
            return PartialView("CommentUser", cmt);
        }
        public JsonResult SendComment(int bookID, string commentContent)
        {
            BookDao dao = new BookDao();
            dao.SendComment(bookID, commentContent);
            return Json(new
            {
                status = true
            });
        }
        public ActionResult ViewShoppingCart()
        {
            return View("_ShoppingCart");
        }
        public JsonResult UpdateStatusOrder_Customer(int orderID)
        {
            CurrentCustomer currentCustomer = (CurrentCustomer)Session[SessionBox.CUSTOMER_SESSION];
            int currentCustomerId = currentCustomer.GetCustomerID();
            int accountId = db.Customers.Where(m => m.CustomerID == currentCustomerId).FirstOrDefault().AccountID;
            var isContain = db.Orders.Where(m => m.AccountCustomerID == accountId).Select(m => m.OrderID).Contains(orderID);
            if (!isContain)
            {
                Session[SessionBox.CUSTOMER_SESSION] = null;
            }
            else
            {
                BookDao dao = new BookDao();
                dao.UpdateStatusOrder_Customer(orderID);
            }
            
            return Json(new
            {
                status = isContain
            });
        }
        public ActionResult ChangeLanguage(string language)
        {
            LanguageManagement.SetCurrentLanguage(language);
            return RedirectToAction("Index","Home");
        }
        //get current currency rate
        private double GetCurrencyRate()
        {
            string apiUrl = "https://free.currencyconverterapi.com/api/v6/convert?q=USD_VND&compact=ultra&apiKey=5ed7e9b2743eefb282d6";
            WebClient web = new WebClient();
            string response = String.Empty;
            try
            {
                response = web.DownloadString(apiUrl);
            }
            catch(Exception ex)
            {
                return -1;
            }
            Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.Linq.JObject.Parse(response);
            string money = obj.Property("USD_VND").Value.ToString();
            try
            {
                double rate = Convert.ToDouble(money);
                return rate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Convert.ToDouble(1);
            }
        }
        
        public ActionResult TestApi()
        {
            return View();
        }
    }
}