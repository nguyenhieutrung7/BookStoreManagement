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
    public class PublisherController : BaseLanguageController
    {
        BookManagementEntities db = new BookManagementEntities();

        // GET: Admin/Publisher
        public ActionResult Index()
        {
            var publishers = db.Publishers.Where(m=>m.IsActive).ToList();
            return PartialView("_IndexPublisherPage", publishers.ToList());
        }

        // CREATE: Admin/Publisher
        public ActionResult Create(Publisher publisher)
        {
            try
            {
                publisher.IsActive = true;
                db.Publishers.Add(publisher);
                db.SaveChanges();

                var publishers = db.Publishers.Where(m=>m.IsActive).ToList();
                return PartialView("_IndexPublisherPage", publishers.ToList());
            }
            catch
            {
                ModelState.AddModelError("", "Abnormal protection mechanism in the system!");
                Session[SessionBox.SITEOWNER_SESSION] = null;
                return RedirectToAction("Login","Account",new { area="Admin"});
            }
        }

        // DETAIL: Detail/Publisher
        public ActionResult ShowDetail(int publisherID)
        {
            return PartialView("_DetailPublisherPage", db.Publishers.Find(publisherID));
        }

        // EDIT: Admin/Publisher
        public ActionResult ShowEdit(int publisherID)
        {
            return PartialView("_EditPublisherPage", db.Publishers.Find(publisherID));
        }

        public ActionResult Edit(Publisher publisher)
        {
            try
            {
                publisher.IsActive = true;
                db.Entry(publisher).State = EntityState.Modified;
                db.SaveChanges();
                var publishers = db.Publishers.Where(m=>m.IsActive).ToList();
                return PartialView("_IndexPublisherPage", publishers.ToList());
            }
            catch
            {
                ModelState.AddModelError("", "Abnormal protection mechanism in the system!");
                Session[SessionBox.SITEOWNER_SESSION] = null;
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

        }

        // DELETE: Admin/Publisher
        public ActionResult Delete(int publisherID)
        {

            try
            {
                db.Publishers.Find(publisherID).IsActive = false;
                db.SaveChanges();

                var publishers = db.Publishers.Where(m => m.IsActive).ToList();
                return PartialView("_IndexPublisherPage", publishers.ToList());
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