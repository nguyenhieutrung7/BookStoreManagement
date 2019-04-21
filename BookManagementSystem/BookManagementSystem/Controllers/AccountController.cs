using CommonBox;
using Model.Dao;
using Model.Models;
using Model.ViewModels;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Model.Resources;
using System.Web;

namespace BookManagementSystem.Controllers
{
    public class AccountController : BaseController
    {
        BookManagementEntities db = new BookManagementEntities();

        // LOGIN: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string accountname, string password)
        {
            var model = new LoginVM() { AccountName = accountname, Password = password };
            if (ModelState.IsValid)
            {
                AccountDao accountDao = new AccountDao();
                int result = accountDao.CheckAccount(model.AccountName, CommonBox.StringSecurity.SHA256Encrypt(password));

                switch (result)
                {
                    case -2:
                        ModelState.AddModelError("", Model.Resources.Login.Account_NotTrue);
                        break;
                    case 0:
                        ModelState.AddModelError("", Model.Resources.Login.Account_NotExist);
                        break;
                    case -1:
                        ModelState.AddModelError("", Model.Resources.Login.Account_Locked);
                        break;
                    default:
                        try
                        {
                            var account = db.Accounts.Find(result);
                            if (account.RoleID == 1)
                            {
                                var siteOwner = db.SiteOwners.Where(m => m.AccountID == result).SingleOrDefault();

                                //Add Session and return Home Page
                                CurrentSiteOwner currentSiteOwner = new CurrentSiteOwner(model.AccountName, siteOwner.SiteOwnerName, siteOwner.AccountID);
                                Session[SessionBox.SITEOWNER_SESSION] = null;
                                Session[SessionBox.SITEOWNER_SESSION] = currentSiteOwner;
                                return RedirectToAction("Index", "AdminHome", new { area = "Admin" });

                            }
                            else if (account.RoleID == 2)
                            {
                                var customer = db.Customers.Where(m => m.AccountID == result).SingleOrDefault();

                                //Add Session and return Home Page
                                CurrentCustomer currentCustomer = new CurrentCustomer(model.AccountName, customer.CustomerName, customer.CustomerID);
                                Session[SessionBox.CUSTOMER_SESSION] = null;
                                Session[SessionBox.CUSTOMER_SESSION] = currentCustomer;
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        catch
                        {
                            //Action when account name not single or not found
                            ModelState.AddModelError("", Model.Resources.Login.Account_SomethingWrong);
                        }
                        break;
                        
                }
            }
            return View(model);
        }

        // REGISTER: Account
        public ActionResult Register()
        {
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName");
            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
            {
                lang = langCookie.Value;
            }
            ViewBag.Lang = lang;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterVM registerVM)
        {
            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];
            if (ModelState.IsValid)
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Create Customer data first and create account data
                        string password = CommonBox.StringSecurity.SHA256Encrypt(registerVM.password);
                        var account = new Account() { AccountName = registerVM.accountname, Password = password, RoleID = 2, CreatedDate = System.DateTime.Now, ModifiedDate = System.DateTime.Now, IsActive = false };
                        db.Accounts.Add(account);
                        db.SaveChanges();
                        var customer = new Customer() { CustomerName = registerVM.username, CustomerAddress = registerVM.address, CustomerEmail = registerVM.email, CustomerPhoneNumber = registerVM.phonenumber, GenderID = registerVM.genderid, AccountID = account.AccountID,CustomerBirthday=registerVM.birthday };
                        db.Customers.Add(customer);
                        db.SaveChanges();
                        transaction.Commit();

                        // Send mail authentication
                        string parameterUrl = "/Account/Authentication?security1=" + account.AccountName + "&security2=" + password;
                        string linkAuthentication = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content(parameterUrl));
                        //Get cookie language choosen
                        if (langCookie != null)
                        {
                            lang = langCookie.Value;
                        }
                        MailBox.SendMailAuthentication(customer.CustomerEmail, "Authentication Account Book Management", linkAuthentication,lang);
                        ModelState.AddModelError("", Model.Resources.Register.Active_Account_Alert);
                        return View("Login");
                     }
                    catch
                     {
                         transaction.Rollback();
                         ModelState.AddModelError("", Model.Resources.Register.Something_Wrong);
                         ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName");
                            return View("Register");
                     }
                }
             }
            if (langCookie != null)
            {
                lang = langCookie.Value;
            }
            ViewBag.Lang = lang;
            return View();
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

        public ActionResult Authentication(string security1, string security2)
        {
            try
            {
                var account = db.Accounts.Where(m => m.AccountName == security1 && m.Password == security2).SingleOrDefault();
                account.IsActive = true;
                db.SaveChanges();
                ModelState.AddModelError("", Model.Resources.Register.Account_Activitied);
                return View("Login");
            }
            catch
            {
                ModelState.AddModelError("", Model.Resources.Register.Account_Activitied_Wrong);
                return View("Login");
            }
        }

        public ActionResult Logout()
        {
            Session[SessionBox.CUSTOMER_SESSION] = null;
            return RedirectToAction("Index","Home");
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