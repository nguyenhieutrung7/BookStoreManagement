using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.Infrastructure;
using Model.Models;
using Model.ViewModels;
namespace BookManagementSystem.Areas.Admin.Controllers
{
    public class OrderController : BaseLanguageController
    {
        // GET: Admin/Order
        OrderDAO dao = new OrderDAO();
        public ActionResult OrderManagement()
        {
            var res = dao.LoadOrder();
            ViewBag.QuantityConfirm = dao.GetQuantityConfirm();
            if (res==null)
            {
                return PartialView("OrderPartialView",new OrderAccountModel());
            }
            else
                return PartialView("OrderPartialView", res);
        }
        
        public ActionResult ChangeLanguage(string lang)
        {
            LanguageManagement.SetCurrentLanguage(lang);
            
            System.Web.HttpContext.Current.Application["CurrentLanguage"] = lang.Equals("vi") ? "Vietnamese" : "English";
            return RedirectToAction("Index","AdminHome");
        }

        // Send mail nge dai ca
        public JsonResult UpdateOrderStatus(int orderID,int idStatus)
        {
            dao.UpdateOrderStatus(orderID,idStatus);
            string emailCustomer;
            using(BookManagementEntities db = new BookManagementEntities())
            {
                var accountId = db.Orders.Where(m => m.OrderID == orderID).Select(m => m.Account.AccountID).SingleOrDefault();
                emailCustomer = db.Customers.Where(m => m.AccountID == accountId).Select(m => m.CustomerEmail).FirstOrDefault();
            }
            return Json(new
            {
                status = true,
                email = emailCustomer
            });
        }
        public JsonResult LoadOrderDetail(int orderID)
        {
            var res=dao.LoadOrderDetail(orderID);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadInfoCustomer(int orderID)
        {
            var res = dao.GetInfoCustomer(orderID);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        // Send mail nge dai ca
        public JsonResult CancelOrder(int orderID) // 
        {
            dao.CancelOrder(orderID);
            string emailCustomer;
            using (BookManagementEntities db = new BookManagementEntities())
            {
                var accountId = db.Orders.Where(m => m.OrderID == orderID).Select(m => m.Account.AccountID).SingleOrDefault();
                emailCustomer = db.Customers.Where(m => m.AccountID == accountId).Select(m => m.CustomerEmail).FirstOrDefault();
            }
            return Json(new
            {
                status = true,
                email = emailCustomer
            });
        }
    }
}