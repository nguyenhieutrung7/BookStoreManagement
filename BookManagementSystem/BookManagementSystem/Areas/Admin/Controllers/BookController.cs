
using Model.Dao;
using Model.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Script.Serialization;
using Model.ViewModels;
using Microsoft.Ajax.Utilities;

namespace BookManagementSystem.Areas.Admin.Controllers
{
    //book controller to handle request related book management 
    public class BookController : BaseLanguageController
    {
        private static string oldPicture = "";
        // GET: Admin/Book
        public ActionResult Index()
        {
            //get list books which are active and order by descending modified date
            List<Book> list = BookDao.GetListBooks().OrderByDescending(m => m.ModifiedDate).ToList();
            return PartialView("_IndexBook", list);
        }
        //Render page Create new book
        public ActionResult Create()
        {
            //get list categories existing and assign for viewbag
            SelectList categories = new SelectList(CategoryDao.GetAllCategory(), "categoryID", "categoryName");
            ViewBag.Categories = categories;
            //get list authors existing and assign for viewbag
            SelectList authors = new SelectList(AuthorDao.GetListAuthors(), "authorID", "authorName");
            ViewBag.Authors = authors;
            //get list publishers existing and assign for viewbag
            SelectList publishers = new SelectList(PublisherDao.GetListPublisher(), "publisherID", "publisherName");
            ViewBag.Publishers = publishers;

            return PartialView("_CreateBook", new Book());
        }

        //Save new book
        [HttpPost]
        public JsonResult Create(Book model)
        {
            model.ModifiedDate = DateTime.Now;
            model.CreatedDate = DateTime.Now;
            model.Picture = "/Assets/images/" + model.Picture;
            //result equals true if save new book successfully, esle equals false
            var result = BookDao.CreateOrEditBook(model) > 0;
            return Json(result);
        }

        //Render page edit book by book id
        public ActionResult Edit(int id)
        {
            //get book need to edit
            var book = BookDao.GetBook(id);
            //store older picture if user don't choose new picture
            oldPicture = book.Picture;
            //get list categories existing and assign for viewbag
            SelectList categories = new SelectList(CategoryDao.GetAllCategory(), "categoryID", "categoryName", book.Category.CategoryName);
            ViewBag.Categories = categories;
            //get list authors existing and assign for viewbag
            SelectList authors = new SelectList(AuthorDao.GetListAuthors(), "authorID", "authorName", book.Author.AuthorName);
            ViewBag.Authors = authors;
            //get list publishers existing and assign for viewbag
            SelectList publishers = new SelectList(PublisherDao.GetListPublisher(), "publisherID", "publisherName", book.Publisher.PublisherName);
            ViewBag.Publishers = publishers;
            return View("_EditBook", book);
        }

        //save new information about book
        [HttpPost]
        public JsonResult Edit(Book book)
        {
            //update modified date
            book.ModifiedDate = DateTime.Now;
            //check if user doesn't choose new picture and set new picture = old picture
            if (String.IsNullOrEmpty(book.Picture))
            {
                book.Picture = oldPicture;
            }
            //check if user choose new picture and set new picture for this book
            else
            {
                book.Picture = "/Assets/images/" + book.Picture;
            }
            //result equals true if save book successfully, else equals false
            var result = BookDao.CreateOrEditBook(book);
            return Json(result);
        }
        //View detail book by book ID
        public JsonResult Detail(int id)
        {
            using (var db=new BookManagementEntities())
            {
                var model = db.Books.Where(book => book.BookID == id).Join(db.Categories, b => b.CategoryID, c => c.CategoryID, (b, c) => new { b, c }).
                     
                     Join(db.Authors, bo => bo.b.AuthorID, au => au.AuthorID, (bo, au) => new { bo, au }).
                     Join(db.Publishers, bop => bop.bo.b.PublisherID, p => p.PublisherID, (bop, p) => new BookViewModel
                     {
                         BookID = bop.bo.b.BookID,
                         Title = bop.bo.b.Title,
                         CategoryName = bop.bo.c.CategoryName,
                         Description = bop.bo.b.Description,
                         CreatedDate = bop.bo.b.CreatedDate,
                         ModifiedDate = bop.bo.b.ModifiedDate,
                         ISBN = bop.bo.b.ISBN,
                         PublisherName = p.PublisherName,
                         AuthorName = bop.au.AuthorName,
                         Quantity=bop.bo.b.Quantity,
                         Price=bop.bo.b.Price,
                         Picture=bop.bo.b.Picture,
                     }).FirstOrDefault();
                model.CreatedDate = model.CreatedDate.Date;
                model.ModifiedDate = model.ModifiedDate.Date;
                var serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                return Json(model,JsonRequestBehavior.AllowGet);
            }

        }
        //delete book by book ID
        [HttpPost]
        public JsonResult Delete(int id)
        {
            //result equals true if delete book successfully, else equals false
            bool result = BookDao.DeleteBook(id);
            return Json(result);
        }
        //upload image file and save in ~/Assets/images folder
        public JsonResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/Assets/images"), fname);
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
    }
}