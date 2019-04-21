using Model.Infrastructure;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookManagementSystem.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
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
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (CurrentSiteOwner)Session[CommonBox.SessionBox.SITEOWNER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" ,area=""}));
            }
            base.OnActionExecuting(filterContext);
        }      
            

    }
}