using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Infrastructure;
namespace BookManagementSystem.Areas.Admin.Controllers
{
    public class BaseLanguageController : Controller
    {
        // GET: Admin/BaseLanguage
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
            {
                lang = langCookie.Value;
            }
            else
            {
                var userLanguage = Request.UserLanguages;
                var userLang = userLanguage != null ? userLanguage[0] : "";
                if (userLang != "")
                {
                    lang = userLang;
                }
                else
                {
                    lang = LanguageManagement.GetDefaultLanguage();
                }
            }
            LanguageManagement.SetCurrentLanguage(lang);          
            return base.BeginExecuteCore(callback, state);
        }

    }
}