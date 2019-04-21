using Model.Dao;
using Model.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Model.ViewModels;
using System.Web.Script.Serialization;

namespace BookManagementSystem.Areas.Admin.Controllers
{
    public class AuthorController : BaseLanguageController
    {
        // GET: Admin/Author
        public ActionResult Index()
        {
            var listAuthors = AuthorDao.GetListAuthors().OrderByDescending(m => m.AuthorID).Where(m => m.IsActive).ToList();
            return PartialView("_IndexAuthor", listAuthors);
        }
        
        //create new author
        [HttpPost]
        public JsonResult Create(Author author)
        {
            author.IsActive = true;
            var result = AuthorDao.CreateOrEdit(author) > 0;
            return Json(result);
        }

        //Delete athor by ID
        [HttpPost]
        public JsonResult Delete(int id)
        {
            // result return true  if delete successfully, else return false
            var result = AuthorDao.Delete(id) > 0;
            return Json(result);
        }

        //Delete author by ID
        [HttpPost]
        public JsonResult Edit(Author author)
        {
            // result return true  if update successfully, else return false
            var result = AuthorDao.CreateOrEdit(author) > 0;
            return Json(result);
        }

        //View detail author by ID
        public JsonResult Detail(int id)
        {
            var author = AuthorDao.GetAuthor(id);
            var authorView = new AuthorViewModel();

            authorView.ListBooksTitle = author.Books.Select(m => m.Title).ToList();
            authorView.AuthorPhone = author.AuthorPhone;
            authorView.AuthorName= author.AuthorName;
            authorView.AuthorID = author.AuthorID;
            authorView.IsActive = author.IsActive;
            return Json(authorView, JsonRequestBehavior.AllowGet);
        }
    }
}