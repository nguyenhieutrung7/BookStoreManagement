using CommonBox;
using Model.Models;
using Model.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookManagementSystem.Areas.Admin.Controllers
{
    public class MailController : BaseLanguageController
    {
        BookManagementEntities db = new BookManagementEntities();
        // GET: Admin/Mail
        
        [ValidateInput(false)]
        public JsonResult SendOrderedMail(string content,string email)
        {
            MailBox.SendMail(email, Mail.OrderedMail_Subject,content);
            return Json(new
            {
                status = true
            });
        }
        [ValidateInput(false)]
        public JsonResult SendDeliveringMail(string content, string email)
        {
            string datetime = System.DateTime.Now.ToString();
            MailBox.SendMail(email, Mail.DeliveringMail_Subject, content);
            return Json(new
            {
                status = true
            });
        }
        [ValidateInput(false)]
        public JsonResult SendCancelledMail(string content, string email)
        {
            MailBox.SendMail(email, Mail.CancelledMail_Subject, content);
            return Json(new
            {
                status = true
            });
        }
        [ValidateInput(false)]
        public JsonResult SendCompletedMail(string content, string email)
        {
            MailBox.SendMail(email, Mail.CompletedMail_Subject, content);
            return Json(new
            {
                status = true
            });
        }

        //Create Form Ordered to send mail
        public ActionResult OrderedMail(int orderId)
        {
            string datetime = System.DateTime.Now.ToString();
            var order = db.Orders.Find(orderId);
            var orderDetailList = db.OrderDetails.Where(m => m.OrderID == orderId).ToList();
            decimal totalMoney = 0;
            foreach(var item in orderDetailList)
            {
                totalMoney += item.Money ;
            }
            ViewBag.TotalMoney = totalMoney;
            ViewBag.Header = Mail.Mail_Hello + order.Account.Customers.Select(m => m.CustomerName).First();
            ViewBag.DateNow = System.DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.Content = Mail.Ordered_Alert_Time+datetime+".";
            ViewBag.ImageSource = "https://d1oco4z2z1fhwp.cloudfront.net/templates/default/18/okok.gif";
            return PartialView("_DetailMail",orderDetailList);
        }
        //Create Form Delivering to send mail
        public ActionResult DeliveringMail(int orderId)
        {
            string datetime = System.DateTime.Now.ToString();
            var orderDetailList = db.OrderDetails.Where(m => m.OrderID == orderId).ToList();
            var order = db.Orders.Find(orderId);
            decimal totalMoney = 0;
            foreach (var item in orderDetailList)
            {
                totalMoney += item.Money ;
            }
            ViewBag.TotalMoney = totalMoney;
            ViewBag.Header = Mail.Mail_Hello + order.Account.Customers.Select(m => m.CustomerName).First();
            ViewBag.DateNow = System.DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.Content = Mail.Delivering_Alert_Time+datetime+".";
            ViewBag.Address = Mail.Address_Shipping+order.OrderAddress;
            ViewBag.Phone = Mail.Phone_Contact+order.PhoneNumber;
            ViewBag.ImageSource = "https://i.imgur.com/L3uu5J8.png";
            return PartialView("_DetailMail", orderDetailList);
        }
        //Create Form Cancelled to send mail
        public ActionResult CancelledMail(int orderId)
        {
            string datetime = System.DateTime.Now.ToString();
            var order = db.Orders.Find(orderId);
            var orderDetailList = db.OrderDetails.Where(m => m.OrderID == orderId).ToList();
            decimal totalMoney = 0;
            foreach (var item in orderDetailList)
            {
                totalMoney += item.Money ;
            }
            ViewBag.TotalMoney = totalMoney;
            ViewBag.Header = Mail.Sorry_Header + order.Account.Customers.Select(m => m.CustomerName).First();
            ViewBag.DateNow = System.DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.Content = Mail.Cancelled_Alert_Time + datetime + ".";
            ViewBag.ImageSource = "https://i.imgur.com/2O9zcOa.png";
            return PartialView("_DetailMail", orderDetailList);
        }
        //Create Form Completed to send mail
        public ActionResult CompletedMail(int orderId)
        {
            string datetime = System.DateTime.Now.ToString();
            var order = db.Orders.Find(orderId);
            var orderDetailList = db.OrderDetails.Where(m => m.OrderID == orderId).ToList();
            decimal totalMoney = 0;
            foreach (var item in orderDetailList)
            {
                totalMoney += item.Money ;
            }
            ViewBag.TotalMoney = totalMoney;
            ViewBag.Header = Mail.Mail_Hello + order.Account.Customers.Select(m => m.CustomerName).First();
            ViewBag.DateNow = System.DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.Content = Mail.Completed_Alert;
            ViewBag.ImageSource = "https://i.imgur.com/xMiMkbX.png";
            return PartialView("_DetailMail", orderDetailList);
        }
    }
}