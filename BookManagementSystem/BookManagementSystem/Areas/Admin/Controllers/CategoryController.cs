using CommonBox;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookManagementSystem.Areas.Admin.Controllers
{
    public class CategoryController : BaseLanguageController
    {

        BookManagementEntities db = new BookManagementEntities();

        // GET: Admin/Category
        public ActionResult Index()
        {
            var categories = db.Categories.ToList();
            return PartialView("_IndexCategoryPage", categories.ToList());
        }

        //Check unique category name
        public JsonResult CheckUniqueCategoryName(string categoryName)
        {
            if (db.Categories.Where(m => m.CategoryName == categoryName).Count() > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        // CREATE: Admin/Category
        public ActionResult Create(Category category)
        {
            try
            {
                category.IsActive = true;
                db.Categories.Add(category);
                db.SaveChanges();

                var categories = db.Categories.ToList();
                return PartialView("_IndexCategoryPage", categories.ToList());
            }
            catch
            {
                ModelState.AddModelError("", "Abnormal protection mechanism in the system!");
                Session[SessionBox.SITEOWNER_SESSION] = null;
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
        }

        // DETAIL: Detail/Publisher
        public ActionResult ShowDetail(int categoryID)
        {
            return PartialView("_DetailCategoryPage", db.Categories.Find(categoryID));
        }

        // EDIT: Admin/Category
        public ActionResult ShowEdit(int categoryID)
        {
            return PartialView("_EditCategoryPage", db.Categories.Find(categoryID));
        }
        public ActionResult Edit(Category category)
        {
            try
            {
                category.IsActive = true;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                var categories = db.Categories.ToList();
                return PartialView("_IndexCategoryPage", categories.ToList());
            }
            catch
            {
                ModelState.AddModelError("", "Abnormal protection mechanism in the system!");
                Session[SessionBox.SITEOWNER_SESSION] = null;
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

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