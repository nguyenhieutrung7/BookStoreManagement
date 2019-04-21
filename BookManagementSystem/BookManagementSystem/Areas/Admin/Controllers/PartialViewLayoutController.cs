using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookManagementSystem.Areas.Admin.Controllers
{
    public class PartialViewLayoutController : Controller
    {
        // GET: Admin/PartialViewLayout
        public ActionResult DropdownSiteOwner()
        {
            var currentSiteOwner = (CurrentSiteOwner)Session[CommonBox.SessionBox.SITEOWNER_SESSION];
            if (currentSiteOwner != null)
            {
                ViewBag.SiteOwnerName = currentSiteOwner.GetUserName();
            }
            return PartialView("_DropdownSiteOwner");
        }
    }
}