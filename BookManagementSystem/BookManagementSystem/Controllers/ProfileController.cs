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
    public class ProfileController : BaseController
    {
        // GET: Admin/Profile
        public ActionResult Index()
        {
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
                    List<Order> orders = OrderDAO.GetOrder(customer.AccountID);
                    return PartialView("_IndexProfile", orders);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }

        }
    }
}