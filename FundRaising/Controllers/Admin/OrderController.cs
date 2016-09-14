using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FundRaising.Models;
using FundRaising.ViewModels;

namespace FundRaising.Controllers.Admin
{
    public class OrderController : Controller
    {
        private FundRaisingDBContext db = new FundRaisingDBContext();

        //
        // GET: /Order/

        public ActionResult Index(int option=0)
        {
            if(option==1)
            {
                ViewBag.error = "File not found";
            }
            var list = db.Orders.ToList().Join(db.Organizations.Where(x=>x.IsActive==true).AsEnumerable(), o => o.SchoolID, org => org.ID, 
                (o, org) => new { Order = o, Organization = org })
                .Join(db.Students.ToList(),outer => outer.Order.StudentID, 
                stu => stu.StudentID, (outer, stu) => new { Student = stu, Organization = outer.Organization, Order = outer.Order })
                .Select(x => new OrderViewModel { CompanyName = x.Order.CompanyName, FirstName = x.Order.FirstName,LastName=x.Order.LastName,
                                                  ID = x.Order.ID,
                                                  Status = x.Order.Status,
                                                  SchoolID = x.Organization.SchoolID,
                                                  SchoolName = x.Organization.Name,
                                                  StudentFirstName = x.Student.FirstName,
                                                  StudentLastName = x.Student.LastName,
                                                  CreatedDate = x.Order.CreatedDate
                }).ToList();
            return View(list);
        }

        //
        // GET: /Order/Details/5

        public ActionResult Details(int id = 0)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // GET: /Order/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Order/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        //
        // GET: /Order/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // POST: /Order/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        //
        // GET: /Order/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // POST: /Order/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        //[HttpPost]
        public ActionResult DownloadOrder()
        {

            DateTime DT = DateTime.Now;

           if (DT.Hour == 2)
            {
                ShrdMaster.Instance.DownloadFile();
            }
            //ShrdMaster.Instance.DownloadFile();
           
                return Json("done", JsonRequestBehavior.AllowGet);                                              
        }
        public ActionResult DownloadAllOrders()
        {


                ShrdMaster.Instance.DownloadAllFile();

            return Json("done", JsonRequestBehavior.AllowGet);
        }

        public ActionResult downloadData()
        {
            string FolderPath = Server.MapPath("~/Orders");
            DirectoryInfo dir=new DirectoryInfo(FolderPath);
            FileInfo[] files=dir.GetFiles();

            List<ExportFile> exportFiles = new List<ExportFile>();
            ExportFile exportfile = null;

            foreach(FileInfo file in files)
            {
                exportfile = new ExportFile();
                exportfile.CreatedDate = file.CreationTime.ToShortDateString();
                exportfile.FileName = file.Name;
                if(file.Length>0)
                {
                    exportfile.Files = "1";
                }
                else
                {
                    exportfile.Files = "0";
                }
                
                exportFiles.Add(exportfile);
            }

            return Json(exportFiles, JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadFile(string name)
        {
            string FolderPath = Server.MapPath("~/Orders");            
             string Filepath = Path.Combine(FolderPath,name);
            return File(Filepath, "text/plain", name);
   
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}