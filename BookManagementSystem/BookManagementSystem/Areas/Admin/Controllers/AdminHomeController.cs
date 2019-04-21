using Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookManagementSystem.Areas.Admin.Controllers
{
    public class AdminHomeController : BaseLanguageController
    {
        private BookManagementEntities db = new BookManagementEntities();
        // GET: Admin/AdminHome
        public ActionResult Index()
        {
            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
            {
                lang = langCookie.Value;
            }
            ViewBag.Lang = lang;
            if (lang == "vi")
            {
                ViewBag.Language = "Vietnamese";
            }
            else
            {
                ViewBag.Language = "English";
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}