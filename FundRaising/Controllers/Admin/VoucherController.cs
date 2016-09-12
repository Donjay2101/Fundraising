using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FundRaising.Filters;
using FundRaising.Models;

namespace FundRaising.Controllers.Admin
{
    [Authorize(Users = "Admin")]
    [InitializeSimpleMembership]
    public class VoucherController : Controller
    {
        private FundRaisingDBContext db = new FundRaisingDBContext();

        //
        // GET: /Voucher/

        public ActionResult Index()
        {
            return View(db.Vouchers.ToList());
        }

        //
        // GET: /Voucher/Details/5

        public ActionResult Details(int id = 0)
        {
            Voucher voucher = db.Vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        //
        // GET: /Voucher/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Voucher/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                db.Vouchers.Add(voucher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(voucher);
        }

        //
        // GET: /Voucher/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Voucher voucher = db.Vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        //
        // POST: /Voucher/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(voucher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(voucher);
        }

        //
        // GET: /Voucher/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Voucher voucher = db.Vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        //
        // POST: /Voucher/Delete/5

        [HttpPost, ActionName("Delete")]
        
        public JsonResult DeleteConfirmed(int id)
        {
            Voucher voucher = db.Vouchers.Find(id);
            db.Vouchers.Remove(voucher);
            db.SaveChanges();
            return Json("done",JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}