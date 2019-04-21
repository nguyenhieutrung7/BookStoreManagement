using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.Models;
using Model.ViewModels;

namespace BookManagementSystem.Areas.Admin.Controllers
{
    public class CommentController : BaseLanguageController
    {
        
        // GET: Admin/Comment
        CommentDao dao = new CommentDao();
          
        public ActionResult CommentManagement()
        {
           
            var res = dao.Load();
            if(res==null)
                return PartialView("CommentPartialView", new CommentBookModels());
            else
                return PartialView("CommentPartialView",res);
        }      
        public JsonResult LoadReply(int idComment)
        {
            var res = dao.LoadReply(idComment);           
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddReply(string replyContent, int idComment)
        {
            var currentSiteOwner = (CurrentSiteOwner)Session[CommonBox.SessionBox.SITEOWNER_SESSION];
            var accountID= currentSiteOwner.GetAccountID();
            dao.AddReply(replyContent,accountID, idComment);
            return Json(new
            {
                status = true
            });
        }
        public JsonResult DeleteComment(int idComment)
        {
            dao.DeleteComment(idComment);
            return Json(new
            {
                status = true
            });
        }
        
        
    }
}