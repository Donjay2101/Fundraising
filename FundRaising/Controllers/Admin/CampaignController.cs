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
    public class CampaignController : Controller
    {
        private FundRaisingDBContext db = new FundRaisingDBContext();

        //
        // GET: /Campaign/

        public ActionResult Index()
        {
            return View(db.Campaigns.ToList());
        }

        //
        // GET: /Campaign/Details/5

        public ActionResult Details(int id = 0)
        {
            Campaign campaign = db.Campaigns.Find(id);
            if (campaign == null)
            {
                return HttpNotFound();
            }
            return View(campaign);
        }

        //
        // GET: /Campaign/Create

        public ActionResult Create(string returnUrl)
        {

            ViewBag.returnUrl = returnUrl;
            return View();
            
        }

        //
        // POST: /Campaign/Create

        [HttpPost]        
        public ActionResult Create(FormCollection fc)
        {

            //if(model!=null)
            //{
            //    db.Campaigns.Add(model);
            //    db.SaveChanges();
            //}

            //return Json("1", JsonRequestBehavior.AllowGet);

            string returnUrl = "";

            //collecting data from from
            string campaignName = fc["CampaignName"].ToString();
            DateTime startDate, EndDate;
            DateTime.TryParse(fc["CampaignStartDate"].ToString(), out startDate);
            DateTime.TryParse(fc["CampaignEndDate"].ToString(), out EndDate);

            if (fc["returnUrl"] != null)
            {
                returnUrl = fc["returnUrl"].ToString();
            }

            //if (Request.QueryString["returnUrl"]!=null)
            //{
            //    returnUrl = Request.QueryString["returnUrl"].ToString();
            //}


            //assign data to campaign
            Campaign campaign = new Campaign();
            campaign.CampaignName = campaignName;
            campaign.CampaignStartDate = startDate;
            campaign.CampaignEndDate = EndDate;

            int organizationId = -1;
            if (ModelState.IsValid)
            {
                if (Session["OrganizationID"] != null)
                {
                    int.TryParse(Session["OrganizationID"].ToString(), out organizationId);
                }
                campaign.OrganizatonID = organizationId;
                db.Campaigns.Add(campaign);
                db.SaveChanges();
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return View("Index");
                }
                return Redirect(returnUrl);
                //RedirectToAction(returnUrl);
            }

            return View();
        }

        //
        // GET: /Campaign/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Campaign campaign = db.Campaigns.Find(id);
            if (campaign == null)
            {
                return HttpNotFound();
            }
            if (Request.QueryString["returnUrl"] != null)
            {
                ViewBag.returnUrl = Request.QueryString["returnUrl"].ToString();
            }
            ViewBag.OrganizationID = campaign.OrganizatonID;
            return View(campaign);
        }

        //
        // POST: /Campaign/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Campaign campaign, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(campaign).State = EntityState.Modified;
                db.SaveChanges();

                if (fc!= null)
                {
                    if (fc["returnUrl"] != null)
                    {
                        return Redirect(fc["returnUrl"].ToString());
                    }
                    
                }
               
                return RedirectToAction("Index","Organization");
            } ViewBag.OrganizationID = campaign.OrganizatonID;
            return View(campaign);
        }

        //
        // GET: /Campaign/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Campaign campaign = db.Campaigns.Find(id);
            if (campaign == null)
            {
                return HttpNotFound();
            }
            return View(campaign);
        }

        //
        // POST: /Campaign/Delete/5

        [HttpPost, ActionName("Delete")]       
        public JsonResult DeleteConfirmed(int id)
        {
            Campaign campaign = db.Campaigns.Find(id);
            db.Campaigns.Remove(campaign);
            db.SaveChanges();
            return Json("data",JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}