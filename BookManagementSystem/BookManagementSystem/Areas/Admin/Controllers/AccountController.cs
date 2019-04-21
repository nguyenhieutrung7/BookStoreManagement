using CommonBox;
using Model.Dao;
using Model.Models;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookManagementSystem.Areas.Admin.Controllers
{
    public class AccountController : BaseLanguageController
    {
        BookManagementEntities db = new BookManagementEntities();

        //CRUD Account
        public ActionResult Index()
        {
            var siteowners = db.SiteOwners.Include(m => m.Account).Include(m=>m.Gender).Where(m=>m.Account.IsActive).OrderByDescending(m=>m.Account.ModifiedDate);
            ViewBag.GenderID = new SelectList(db.Genders, "genderID", "genderName");
            return PartialView("_IndexAccountPage",siteowners.ToList());
        }


        // GET: Admin/Account
        public ActionResult ShowDetail(int siteownerID)
        {
            var siteowner = db.SiteOwners.Include(m => m.Account).Where(m => m.SiteOwnerID == siteownerID).SingleOrDefault();
            var accountInfoVM = new AccountInfoVM()
            {
                AccountName = siteowner.Account.AccountName,
                GenderID = siteowner.GenderID,
                SiteOwnerAddress = siteowner.SiteOwnerAddress,
                Password = siteowner.Account.Password,
                SiteOwnerEmail = siteowner.SiteOwnerEmail,
                SiteOwnerID = siteowner.SiteOwnerID,
                SiteOwnerName = siteowner.SiteOwnerName,
                SiteOwnerPhoneNumber = siteowner.SiteOwnerPhoneNumber
            };
            ViewBag.Gender = siteowner.Gender.GenderName;
            ViewBag.CreatedDate = siteowner.Account.CreatedDate.ToString("dd/MM/yyyy");
            ViewBag.ModifiredDate = siteowner.Account.ModifiedDate.ToString("dd/MM/yyyy");
            ViewBag.Role = siteowner.Account.Role.RoleName;

            return PartialView("_DetailAccountPage", accountInfoVM);
        }

        // EDIT: Admin/Account
        public ActionResult ShowEdit(int siteownerID)
        {
            var siteowner = db.SiteOwners.Include(m => m.Account).Where(m => m.SiteOwnerID == siteownerID).SingleOrDefault();
            var accountInfoVM = new AccountInfoVM()
            {
                AccountName = siteowner.Account.AccountName,
                GenderID = siteowner.GenderID,
                SiteOwnerAddress = siteowner.SiteOwnerAddress,
                Password = siteowner.Account.Password,
                SiteOwnerEmail = siteowner.SiteOwnerEmail,
                SiteOwnerID = siteowner.SiteOwnerID,
                SiteOwnerName = siteowner.SiteOwnerName,
                SiteOwnerPhoneNumber = siteowner.SiteOwnerPhoneNumber
            };
            ViewBag.GenderID = new SelectList(db.Genders, "genderID", "genderName", siteowner.GenderID);
            return PartialView("_EditAccountPage", accountInfoVM);
        }
        public ActionResult Edit(AccountInfoVM accountInfoVM)
        {
            using(DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var siteownerID = accountInfoVM.SiteOwnerID;
                    var accountID = db.SiteOwners.Where(m => m.SiteOwnerID == siteownerID).Select(m => m.AccountID).SingleOrDefault();
                    var createdDate = db.SiteOwners.Where(m => m.SiteOwnerID == siteownerID).Select(m => m.Account.CreatedDate).SingleOrDefault();

                    if(accountInfoVM.Password != db.Accounts.Where(m=>m.AccountID==accountID).Select(m=>m.Password).FirstOrDefault())
                    {
                        accountInfoVM.Password = CommonBox.StringSecurity.SHA256Encrypt(accountInfoVM.Password);
                    }

                    var siteowner = new SiteOwner()
                    {
                        SiteOwnerID = accountInfoVM.SiteOwnerID,
                        GenderID = accountInfoVM.GenderID,
                        SiteOwnerAddress = accountInfoVM.SiteOwnerAddress,
                        SiteOwnerEmail = accountInfoVM.SiteOwnerEmail,
                        SiteOwnerName = accountInfoVM.SiteOwnerName,
                        SiteOwnerPhoneNumber = accountInfoVM.SiteOwnerPhoneNumber,
                        AccountID = accountID
                    };
                    db.Entry(siteowner).State = EntityState.Modified;
                    var account = new Account()
                    {
                        AccountID = accountID,
                        AccountName = accountInfoVM.AccountName,
                        CreatedDate = createdDate,
                        IsActive = true,
                        Password = accountInfoVM.Password,
                        RoleID = 1,
                        ModifiedDate = System.DateTime.Now
                    };
                    db.Entry(account).State = EntityState.Modified;
                    db.SaveChanges();
                    transaction.Commit();

                var siteowners = db.SiteOwners.Include(m => m.Account.Role).Include(m => m.Gender).Where(m => m.Account.IsActive).OrderByDescending(m => m.Account.ModifiedDate);
                    ViewBag.GenderID = new SelectList(db.Genders, "genderID", "genderName");
                    return PartialView("_IndexAccountPage", siteowners.ToList());
                }
                catch
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", "Abnormal protection mechanism in the system!");
                    Session[SessionBox.SITEOWNER_SESSION] = null;
                    return View("Login");
                }
            }

        }

        //Check unique account name when CREATE
        public JsonResult CheckUniqueAccountName(string accountName)
        {
            if (db.Accounts.Where(m => m.AccountName == accountName).Count() > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
         
        // CREATE: Admin/Account
        public ActionResult Create(AccountInfoVM accountInfoVM)
        {
            try
            {
                string password = CommonBox.StringSecurity.SHA256Encrypt(accountInfoVM.Password);
                var account = new Account() { AccountName = accountInfoVM.AccountName, Password = password, RoleID = 1, CreatedDate = System.DateTime.Now, ModifiedDate = System.DateTime.Now, IsActive = true };
                db.Accounts.Add(account);
                db.SaveChanges();
                var siteOwner = new SiteOwner() { SiteOwnerName = accountInfoVM.SiteOwnerName, SiteOwnerAddress = accountInfoVM.SiteOwnerAddress, SiteOwnerEmail = accountInfoVM.SiteOwnerEmail, SiteOwnerPhoneNumber = accountInfoVM.SiteOwnerPhoneNumber, GenderID = accountInfoVM.GenderID, AccountID = account.AccountID };
                db.SiteOwners.Add(siteOwner);
                db.SaveChanges();

                var siteowners = db.SiteOwners.Include(m => m.Account.Role).Include(m => m.Gender).Where(m => m.Account.IsActive).OrderByDescending(m => m.Account.ModifiedDate);
                ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName");
                return PartialView("_IndexAccountPage", siteowners.ToList());
            }
            catch
            {
                ModelState.AddModelError("", "Abnormal protection mechanism in the system! ");
                Session[SessionBox.SITEOWNER_SESSION] = null;
                return View("Login");
            }
        }

        public ActionResult Logout()
        {
            Session[SessionBox.SITEOWNER_SESSION] = null;
            return View("Login");
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