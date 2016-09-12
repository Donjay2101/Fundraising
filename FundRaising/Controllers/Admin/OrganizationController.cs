using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FundRaising.Filters;
using FundRaising.Models;
using FundRaising.ViewModels;

namespace FundRaising.Controllers.Admin
{
    [Authorize(Users = "Admin")]
    [InitializeSimpleMembership]
    public class OrganizationController : Controller
    {
        private FundRaisingDBContext db = new FundRaisingDBContext();

        //
        // GET: /Organization/

        public ActionResult Index()
        {
            return View();
        }


        //
        // GET: /Organization/Grid

        public ActionResult Grid()
        {
            var orgs = db.Organizations.ToList();

           var data= orgs.Join(db.Distributors, o => o.Distributor, d => d.userID, (o, d) => new { Organization = o, Distributor = d }).Select(x => new OrganizationViewModel
            {
                 //Address1=x.Organization.Address1,
                 //Address2=x.Organization.Address2,
                 //AutoAssignParticipantID=x.Organization.AutoAssignParticipantID,
                 //Catalog=x.Organization.Catalog,
                 //CellPhoneRequired=x.Organization.CellPhoneRequired,
                 //City=x.Organization.City,
                 //CollectCellPhone=x.Organization.CollectCellPhone,
                 //CollectTeacherGrade=x.Organization.CollectTeacherGrade,
                 //ContactName=x.Organization.ContactName,
                 //Country=x.Organization.Country,
                 //DefaultGoal=x.Organization.DefaultGoal,
                 //Distributor=
                 ID=x.Organization.ID,
                 SchoolID=x.Organization.SchoolID,
                 Name=x.Organization.Name,
                 DistributorName=x.Distributor.UserName,
                 City=x.Organization.City,
                 State=x.Organization.State

            }).ToList(); 

            //orgs.ForEach(x => x.DistributorObject = db.Distributors.Find(x.Distributor));
           //FundRaising.Models.Grids.OrganizationsGrid grids = new Models.Grids.OrganizationsGrid(data);

           return PartialView("_OrganizationGrid",data); 

        }


        public ActionResult GetDistributor(int ID)
        {
            var Distributor = db.Distributors.Find(ID);
            return Json(Distributor, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Organization/Details/5

        public ActionResult Details(int id = 0)
        {
          

            Organization organization = db.Organizations.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return View(organization);
        }


        public async Task<ActionResult> CheckShoolID(int ID)
        {
            bool result =await  ShrdMaster.Instance.CheckShoolID(ID);
            if(result)
            {
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }


        //
        // GET: /Organization/Create

        public ActionResult Create()
        {


            //Distributor List
            ViewBag.Distributor = new SelectList(db.Distributors.ToList(), "UserId", "UserName");

            //Country List
            ViewBag.Country = new SelectList(Common.GetCountries(), "ID", "Text");

            //State List
            ViewBag.State = new SelectList(Common.GetStates(), "ID", "Text");          

            //Catalog List
            ViewBag.Catalog = new SelectList(db.Brochures, "ID", "BrochureName");

            //Ship to Catalog List
           // ViewBag.ShipToSchoolCatalog = new SelectList(db.Brochures, "ID", "BrochureName");


           // ViewBag.FreeShippingAmountS = new SelectList(Common.getShipAmountList(), "ID", "Text");   
            //Goal Type
            ViewBag.GoalType = new SelectList(Common.GoalType(), "ID", "Text"); 

            //Pricing Level
            ViewBag.PricingLevel = new SelectList(Common.getPricingLevel(), "ID", "Text");

            ViewBag.ParticipantOption = new SelectList(Common.getParticipantOption(), "ID", "Text");
            return View();
        }



      

        //
        // POST: /Organization/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Organization organization)
        {

            //Distributor List
            ViewBag.Distributor = new SelectList(db.Distributors.ToList(), "UserId", "UserName");

            //Country List
            ViewBag.Country = new SelectList(Common.GetCountries(), "ID", "Text", organization.Country);

            //State List
            ViewBag.State = new SelectList(Common.GetStates(), "ID", "Text", organization.State);

            //Catalog List
            ViewBag.Catalog = new SelectList(db.Brochures, "ID", "BrochureName", organization.Catalog);

           // ViewBag.FreeShippingAmountS = new SelectList(Common.getShipAmountList(), "ID", "Text",organization.FreeShippingAmountS);
            //Ship To catalog List
         //   ViewBag.ShipToSchoolCatalog = new SelectList(db.Brochures, "ID", "BrochureName", organization.ShipToSchoolCatalog);

            //Goal Type List
            ViewBag.GoalType = new SelectList(Common.GoalType(), "ID", "Text", organization.GoalType); ;

            //Pricing Level
            ViewBag.PricingLevel = new SelectList(Common.getPricingLevel(), "ID", "Text");

            ViewBag.ParticipantOption = new SelectList(Common.getParticipantOption(), "ID", "Text");

            //organization.FreeShippingAmountS = string.Format("{0:C}", organization.FreeShippingAmount);
            if (ModelState.IsValid)
            {
                if(ShrdMaster.Instance.IsString(organization.SchoolID))
                {
                    ViewBag.error = "School Id should be numeric";
                }
                bool result=await ShrdMaster.Instance.CheckShoolID(organization.SchoolID);
                if(result)
                {
                    ViewBag.error = "Student already exists.";
                    return View(organization);
                }
                //decimal free;
                //var Distributor = db.Distributors.Find(organization.Distributor);
               // decimal.TryParse(organization.FreeShippingAmountS.Replace("$", ""),out free);
                 organization.Country = "United States";
                db.Organizations.Add(organization);
                int distributor=organization.Distributor;
                var distributordata = db.Distributors.Find(distributor);
                organization.AutoAssignParticipantID = distributordata.AutoAssignParticipantID;
                organization.FreeShippingAmount = distributordata.FreeShipment;                 
                db.SaveChanges();
                return RedirectToAction("Index");
            }

          

            return View(organization);
        }

        //
        // GET: /Organization/Edit/5

        public ActionResult Edit(int id = 0)
        {


          
          
            Organization organization = db.Organizations.Find(id);
            
            organization.SchoolID.ToString();
            var campaigns= db.Campaigns.Where(x => x.OrganizatonID ==organization.SchoolID );
            if(campaigns.Count()>0)
            {
                organization.OrganizationCampaigns =campaigns.ToList();
            }
            Session["OrganizationID"] = organization.SchoolID;            
            if (organization == null)
            {
                return HttpNotFound();
            }
            organization.FreeShippingAmountS = string.Format("{0:C}", organization.FreeShippingAmount);
             //Distributor List
            ViewBag.Distributor = new SelectList(db.Distributors.ToList(), "UserId", "UserName", organization.Distributor);

            //Country List
            ViewBag.Country = new SelectList(Common.GetCountries(), "ID", "Text", organization.Country);

            //State List
            ViewBag.State = new SelectList(Common.GetStates(), "ID", "Text", organization.State);

            //Catalog List
            ViewBag.Catalog = new SelectList(db.Brochures, "ID", "BrochureName", organization.Catalog);


            ViewBag.FreeShippingAmountS = new SelectList(Common.getShipAmountList(), "ID", "Text",organization.FreeShippingAmountS);     

            //Ship To catalog List
            ViewBag.ShipToSchoolCatalog = new SelectList(db.Brochures, "ID", "BrochureName", organization.ShipToSchoolCatalog);

            //Goal Type List
            ViewBag.GoalType = new SelectList(Common.GoalType(), "ID", "Text", organization.GoalType); ;

            //Pricing Level
            ViewBag.PricingLevel = new SelectList(Common.getPricingLevel(), "ID", "Text");


            ViewBag.ParticipantOption = new SelectList(Common.getParticipantOption(), "ID", "Text",organization.ParticipantOption);
            return View(organization);
        }

        //
        // POST: /Organization/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Organization organization)
        {
            if (ModelState.IsValid)
            {

                //decimal free;
                //decimal.TryParse(organization.FreeShippingAmountS.Replace("$", ""), out free);
                //organization.FreeShippingAmount = free;
                int distributor = organization.Distributor;
                organization.Country = "United States";
                var distributordata = db.Distributors.Find(distributor);
                organization.AutoAssignParticipantID = distributordata.AutoAssignParticipantID;
                db.Entry(organization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
         //   organization.FreeShippingAmountS = string.Format("{0:C}", organization.FreeShippingAmount);
            //Distributor List
            ViewBag.Distributor = new SelectList(db.Distributors.ToList(), "UserId", "UserName",organization.Distributor);
            //Country List
            ViewBag.Country = new SelectList(Common.GetCountries(), "ID", "Text", organization.Country);

            //State List
            ViewBag.State = new SelectList(Common.GetStates(), "ID", "Text", organization.State);

            //Catalog List
            ViewBag.Catalog = new SelectList(db.Brochures, "ID", "BrochureName", organization.Catalog);

           // ViewBag.FreeShippingAmountS = new SelectList(Common.getShipAmountList(), "ID", "Text");   

            //Ship To catalog List
        //    ViewBag.ShipToSchoolCatalog = new SelectList(db.Brochures, "ID", "BrochureName", organization.ShipToSchoolCatalog);

            //Goal Type List
            ViewBag.GoalType = new SelectList(Common.GoalType(), "ID", "Text", organization.GoalType); ;

            //Pricing Level
            ViewBag.PricingLevel = new SelectList(Common.getPricingLevel(), "ID", "Text");

            ViewBag.ParticipantOption = new SelectList(Common.getParticipantOption(), "ID", "Text");

            return View(organization);
        }

        //
        // GET: /Organization/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Organization organization = db.Organizations.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return View(organization);
        }

        //
        // POST: /Organization/Delete/5

        [HttpPost, ActionName("Delete")]
        
        public JsonResult DeleteConfirmed(int id)
        {
            Organization organization = db.Organizations.Find(id);
            db.Organizations.Remove(organization);
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