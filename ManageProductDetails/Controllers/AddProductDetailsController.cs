using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageProductDetails.Models;
using System.IO;
using System.Data.Entity;
using System.Net;
//using PagingAndSorting.Entities;
//using PagingAndSorting.Models;
using PagedList;
namespace ManageProductDetails.Controllers
{
    [Authorize]
    public class AddProductDetailsController : Controller
    {
        private ProductsDBContext db = new ProductsDBContext();
        // GET: AddProducts
        public ActionResult SortProducts(string sortOrder, string CurrentSort, int? page) {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CurrentSort = sortOrder;
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "Name" : sortOrder;
            IPagedList<Products> products = null;
            switch (sortOrder)
            {
                case "Name":
                    if (sortOrder.Equals(CurrentSort))
                        products = db.Products.OrderByDescending
                                (m => m.Name).ToPagedList(pageIndex,pageSize);
                    else
                        products = db.Products.OrderBy
                                (m => m.Name).ToPagedList(pageIndex,pageSize);
                    break;
                case "Category":
                    if (sortOrder.Equals(CurrentSort))
                        products = db.Products.OrderByDescending
                                (m => m.Category).ToPagedList(pageIndex, pageSize);
                    else
                        products = db.Products.OrderBy
                                (m => m.Category).ToPagedList(pageIndex,pageSize);
                    break;
            }

            return View(products);
            //return RedirectToAction("Index");
        }
        [OutputCache(Duration = 60)]
        public ActionResult Index()
        {
           
                    var products = from e in db.Products
                           orderby e.ID
                           select e;
            return View(products);
        }

        // GET: AddProducts/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AddProducts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AddProducts/Create
        [HttpPost]
        // public ActionResult Create(Products pro)
        public ActionResult Create(Products pro, HttpPostedFileBase smallImage, HttpPostedFileBase largeImage)
        {
            try
            {

                string path = Server.MapPath("~/SmallImage/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (smallImage != null)
                {
                    string fileName = Path.GetFileName(smallImage.FileName);
                    smallImage.SaveAs(path + fileName);

                    //string converted = fileName.Replace('-', '+');
                    //converted = converted.Replace('_', '/');
                    pro.SmallImagePath = "~/SmallImage/" + fileName;
                    ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                }
                string path1 = Server.MapPath("~/LargeImage/");
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                }

                if (largeImage != null)
                {
                    string fileName1 = Path.GetFileName(largeImage.FileName);
                    largeImage.SaveAs(path1 + fileName1);
                    //string converted = fileName.Replace('-', '+');
                    //converted = converted.Replace('_', '/');
                    pro.LargeImagePath = "~/LargeImage/" + fileName1;
                    ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName1);
                }
                
                db.Products.Add(pro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AddProducts/Edit/5
        public ActionResult Edit(int id)
        {
            var employee = db.Products.Single(m => m.ID == id);
            return View(employee);
        }

        // POST: AddProducts/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                var products = db.Products.Single(m => m.ID == id);
                if (TryUpdateModel(products))
                {
                    //To Do:- database code
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(products);
            }
            catch
            {
                return View();
            }
        }

        // GET: AddProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Products product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        // POST: AddProducts/Delete/5
        [HttpPost]
        
        public ActionResult Delete(int id, FormCollection collection)
        {
            Products product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DeleteMultiple(FormCollection formCollection)
        {
            try
            {
                string[] ids = formCollection["ID"].Split(new char[] { ',' });
                foreach (string id in ids)
                {
                    var product = this.db.Products.Find(int.Parse(id));
                    this.db.Products.Remove(product);
                    this.db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e) {
                return RedirectToAction("Index");
            }
            
        }
        public ActionResult Searching(string Searchby, string search)
        {
            if (Searchby == "Name")
            {
                var model = db.Products.Where(emp => emp.Name == search || search == null).ToList();
                return View(model);

            }
            else if (Searchby == "Category")
            {
                var model = db.Products.Where(emp => emp.Category.ToString() == search || search == null).ToList();
                return View(model);

            }
            else
            {
                var model = db.Products.Where(emp => emp.Name.StartsWith(search) || search == null).ToList();
                return View(model);
            }
        }


    }
}
