using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FundRaising.Models;
using FundRaising.ViewModels;

namespace FundRaising.Controllers
{
    public class CouponController : Controller
    {
        private FundRaisingDBContext db = new FundRaisingDBContext();

        //
        // GET: /Coupon/

        public ActionResult Index()
        {
        
            
            return View();
        }

        //
        // GET: /Coupon/Details/5

        public ActionResult Details(int id = 0)
        {
            Coupon coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        //
        // GET: /Coupon/Create

        public ActionResult Create()
        {
           // ViewBag.CouponType = new SelectList(Common.CouponTypeList(), "Code", "Description");
            ViewBag.CouponUsage = new SelectList(Common.CouponUsageList(), "ID", "Description");
            ViewBag.CouponCode = ShrdMaster.Instance.GenerateCouponCode();  
            return View();
        }

        //
        // POST: /Coupon/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                coupon.Active = true;
                db.Coupons.Add(coupon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
         //   ViewBag.CouponType = new SelectList(Common.CouponTypeList(), "Code", "Description");
            ViewBag.CouponUsage = new SelectList(Common.CouponUsageList(), "ID", "Description");
            return View(coupon);
        }

        //
        // GET: /Coupon/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Coupon coupon = db.Coupons.Find(id);

            if (coupon == null)
            {
                return HttpNotFound();
            }
            ViewBag.CouponType = new SelectList(Common.CouponTypeList(), "Code", "Description",coupon.CouponType);
            ViewBag.CouponUsage= new SelectList(Common.CouponUsageList(), "ID", "Description",coupon.CouponUsage);
            return View(coupon);
        }

        //
        // POST: /Coupon/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coupon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CouponType = new SelectList(Common.CouponTypeList(), "Code", "Description", coupon.CouponType);
            ViewBag.CouponUsage = new SelectList(Common.CouponUsageList(), "ID", "Description", coupon.CouponUsage);
            return View(coupon);
        }

        //
        // GET: /Coupon/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Coupon coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        public ActionResult CouponData(bool option=true)
        {
            var list = db.Coupons.ToList().Where(x => x.Active == option).Join(Common.CouponUsageList(),
            coupon => coupon.CouponUsage,
            couponUsage => couponUsage.ID,
            (coupon, couponUsage) => new { Coupon = coupon, CouponUsage = couponUsage })
            .Select(x =>
                new CouponViewModel
                {
                    Code = x.Coupon.Code,                   
                    CouponUsage = x.CouponUsage.Description,
                    Description = x.Coupon.Description,
                    EndDate = x.Coupon.EndDate,
                    StartDate = x.Coupon.StartDate,
                    ID = x.Coupon.ID,
                    value = x.Coupon.value
                }).ToList();

            return PartialView("_CouponView",list);
        }

        //
        // POST: /Coupon/Delete/5

        [HttpPost, ActionName("Delete")]      
        public ActionResult DeleteConfirmed(int id)
        {
            Coupon coupon = db.Coupons.Find(id);
            db.Coupons.Remove(coupon);
            db.SaveChanges();
            return Json("done", JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}