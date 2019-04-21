using CommonBox;
using Model.Dao;
using Model.Models;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookManagementSystem.Controllers
{
    public class PartialViewLayoutController : Controller
    {
        public ActionResult DropdownUser()
        {
            //thinh get item in cart and total price
            var cart = (Cart)Session[SessionBox.CART_SESSION];
            ViewBag.NumberBooksInCart = 0;
            ViewBag.TotalPriceInCart = 0;
            if (cart != null)
            {
                ViewBag.NumberBooksInCart = cart.ListCartItems.Sum(c => c.Quantity);
                ViewBag.TotalPriceInCart = cart.ComputeTotalPrice();
            }
            var currentCustomer = (CurrentCustomer)Session[CommonBox.SessionBox.CUSTOMER_SESSION];
            if (currentCustomer != null)
            {
                ViewBag.CustomerName = currentCustomer.GetUserName();
            }
            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
            {
                lang = langCookie.Value;
            }
            ViewBag.Lang = lang;
            return PartialView("_DropdownUser");
        }
        public ActionResult ListCategories()
        {
            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
            {
                lang = langCookie.Value;
            }
            ViewBag.Lang = lang;
            return PartialView("_ListCategories", CategoryDao.GetAllCategory().ToList());
        }

        public ActionResult ListAuthors()
        {
            return PartialView("_ListAuthors", AuthorDao.GetListAuthors().ToList());
        }
    }
}