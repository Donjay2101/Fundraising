using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FundRaising.Models;
using Newtonsoft;
using System.Data.Entity;
using System.Data;
using Ionic.Zip;
using System.IO;
namespace FundRaising.Controllers.Admin
{
    public class CommonController : Controller
    {
        //
        // GET: /Common/

        FundRaisingDBContext db = new FundRaisingDBContext();   

        [HttpPost]
        public ActionResult ShippingChargeSave(string model)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var list=ser.Deserialize<ShippingCharge>(model);

                if (list==null)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            //db.Database.SqlQuery<ShippingCharge>("truncate table ShippingChagres");
            //db.Database.ExecuteSqlCommand("truncate table ShippingCharges");
                db.ShippingCharges.Add(list);
            db.SaveChanges();
            //db.ShippingCharges.RemoveRange(db.ShippingCharges);
           // list.ForEach(x => { db.ShippingCharges.Add(x); db.SaveChanges(); });
            return Json(list.ID, JsonRequestBehavior.AllowGet);
          
            



            //return View();
        }

        public ActionResult SalesTaxes()
        {
            var data = db.SalesTaxCharges.ToList();
            List<SalesTaxCharge> updatedList = new List<SalesTaxCharge>();
            foreach(SalesTaxCharge s in data)
            {
                if(s.Active==true)
                {
                    s.Checked = "checked";
                    //break;
                }
                updatedList.Add(s);
            }
            return Json(updatedList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddTax( string model)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var list = ser.Deserialize<SalesTaxCharge>(model);
            if (list==null)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            db.SalesTaxCharges.Add(list);
            db.SaveChanges();
            //list.ForEach(x => {  });
            return Json("1", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DeleteSalesTax(int ID)
        {

            var data = db.SalesTaxCharges.Find(ID);
            db.SalesTaxCharges.Remove(data);
            db.SaveChanges();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetSalesTax(int ID)
        {
            var previous = db.SalesTaxCharges.Where(x => x.Active == true).ToList();
            previous[0].Active = false;

            var newentry=db.SalesTaxCharges.Find(ID);
            newentry.Active = true;
            db.Entry(newentry).State = EntityState.Modified;
            //db.SalesTaxCharges.Remove(d);
            db.SaveChanges();

            return Json("1",JsonRequestBehavior.AllowGet);
        }


        public ActionResult DownloadOrders()
        {
            string FilePath = Server.MapPath("/Orders");
            string Folername = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Day + "-" + DateTime.Now.Month.ToString();
            string folderPath = Path.Combine(FilePath, Folername);
            if (Directory.Exists(folderPath))
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(folderPath);

                    MemoryStream output = new MemoryStream();
                    zip.Save(output);
                    return File(output, "application/zip", "Order(" + Folername + ").zip");
                }
            }


            return RedirectToAction("Index", "Order", new { option=1});             
        }

        public ActionResult GetShippingcharge()
        {
            var ShippingCharge = db.ShippingCharges.ToList();
            if(ShippingCharge.Count>0)
            {
                return Json(ShippingCharge,JsonRequestBehavior.AllowGet);
            }

            return Json("null", JsonRequestBehavior.AllowGet);
        }
    
        [HttpPost]
        public ActionResult DeleteShippingCharge(int ID)
        {
            var data = db.ShippingCharges.Find(ID);
            if(data!=null)
            {
                db.ShippingCharges.Remove(data);
                db.SaveChanges();
                return Json("1",JsonRequestBehavior.AllowGet);
            }
            return Json("0",JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SaveFreeShippingAmount(int Amount)
        {
            try
            {
                db.Database.ExecuteSqlCommand("update ShippingCharges set FreeAmount=" + Amount);
                db.SaveChanges();
                return Json("1",JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(ex.Message,JsonRequestBehavior.AllowGet);
            }
            
            
        }
    }
}
