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

namespace FundRaising.Controllers
{
    [Authorize(Users="Admin")]
    [InitializeSimpleMembership]
    public class BrochureController : Controller
    {
        private FundRaisingDBContext db = new FundRaisingDBContext();

        //
        // GET: /Brochure/
        public ActionResult Index()
        {                                                                                         
            var data = db.Brochures.ToList();
            

            foreach (Brochure b in data)
            {
                
                //Categories for Brochure
                
                var Categories=(ICollection<Category>)(db.Categories.Join
                                        (db.CategoryToBrochureMap,c=>c.ID,
                                                    MC=>MC.CategoryId,
                                                            (c,MC)=>new{Category=c,CategoryMapping=MC}).
                                                            Where(both => both.CategoryMapping.BrochureID == b.ID)).Select(both => both.Category).ToList();
               
                //product count
                b.productsCount=db.productToCategoryMap.ToList().Join
                    (Categories,map=>map.CategoryID,cat=>cat.ID,(map,cat)=>new{Category=cat,ProductMapping=map}).Select(x=>x.ProductMapping).Count();               
            }


            return View(data);
        }



        public JsonResult getBrochures()
        {
            BrochureViewModel BVM = new BrochureViewModel();
            BVM.BrochureList = db.Brochures.ToList();
            BVM.CategoryList = db.Categories.ToList();
            BVM.ProductList = db.Products.ToList();
            return Json(BVM,JsonRequestBehavior.AllowGet);
        }

        public ActionResult getProductsByBrochureID(string BrochureID,string CategoryID)
        {
            
            BrochureViewModel BVM = new BrochureViewModel();

            BVM.ProductList =ShrdMaster.Instance.getProductsByBrochureID_and_CategoryID(BrochureID,CategoryID);


            //return PartialView("_product",BVM.ProductList);
          return Json(BVM, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCategoriesByBrochureID(string BrochureID,int option=0)
        {

            BrochureViewModel BVM = new BrochureViewModel();

            
            if(option==1)
            {
                BVM.CategoryList = ShrdMaster.Instance.GetByBrochureID(BrochureID).ToList();
            }
            else
            {
                BVM.CategoryList = ShrdMaster.Instance.GetByBrochureID(BrochureID).Where(x => x.check == "checked").ToList();
            }
            


            return Json(BVM, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> MapBrochure(string BrochureID, string CategoryID,string products)
        {

            await ShrdMaster.Instance.Mapbrochure(BrochureID, CategoryID, products);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        


        //
        // GET: /Brochure/Details/5
        public ActionResult Grid()
        {
            //FundRaising.Models.Grids.OrganizationsGrid grids = new Models.Grids.OrganizationsGrid(db.Organizations.ToList().AsQueryable());

            return View("",db.Brochures.ToList());
        }
    

        //
        // GET: /Brochure/Details/5

        public ActionResult Details(int id = 0)
        {
            Brochure brochure = db.Brochures.Find(id);
            Session["BrochureID"] = id;
            Session["BrochureName"] = brochure.BrochureName;
            brochure.CategoryList = db.Categories.ToList().Join(
                    db.mappings.ToList(),
                        cat=>cat.ID,
                            map=>map.CategoryID,
                                (cat,map)=>new{Category=cat,MapCategory=map})
                                    .Where(both=>both.MapCategory.BrochureID==id)
                                        .Select(both=>both.Category).Distinct().ToList();
            
            //brochure.CategoryList=from m in db.Categories 
            if (brochure == null)
            {
                return HttpNotFound();
            }

            ViewBag.BrochureID = id;
            ViewBag.BrochureName = brochure.BrochureName;

            ViewBag.returnUrl = Request.Url.AbsoluteUri;
            return View(brochure);
        }

        //
        // GET: /Brochure/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Brochure/Create

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Brochure brochure)
        {
            if (ModelState.IsValid)
            {
                if(!ShrdMaster.Instance.checkBrochureID(brochure.BrochureID))
                {
                    db.Brochures.Add(brochure);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "BrochureID exists.enter another Brochure ID.";
                }
                
            }

            return View(brochure);
        }

        //
        // GET: /Brochure/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Brochure brochure = db.Brochures.Find(id);
          //  Brochure brochure = db.Brochures.Find(id);
            Session["BrochureID"] = id;
            Session["BrochureName"] = brochure.BrochureName;
            brochure.CategoryList = db.Categories.ToList().Join(
                    db.CategoryToBrochureMap.ToList(),
                        cat => cat.ID,
                            map => map.CategoryId,
                                (cat, map) => new { Category = cat, CategoryMapping=map})
                                    .Where(both => both.CategoryMapping.BrochureID == id)
                                        .Select(both => both.Category).Distinct().ToList();
            if (brochure == null)
            {
                return HttpNotFound();
            }
            return View(brochure);
        }

        //
        // POST: /Brochure/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Brochure brochure)
        {
            if (ModelState.IsValid)
            {
                db.Entry(brochure).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //Brochure brochure = db.Brochures.Find(id);
           // Session["BrochureID"] = id;
            //Session["BrochureName"] = brochure.BrochureName;
            brochure.CategoryList = db.Categories.ToList().Join(
                      db.CategoryToBrochureMap.ToList(),
                          cat => cat.ID,
                              map => map.CategoryId,
                                  (cat, map) => new { Category = cat, CategoryMapping = map })
                                      .Where(both => both.CategoryMapping.BrochureID == brochure.ID)
                                          .Select(both => both.Category).Distinct().ToList();
            return View(brochure);
        }

        //
        // GET: /Brochure/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Brochure brochure = db.Brochures.Find(id);
            if (brochure == null)
            {
                return HttpNotFound();
            }
            return View(brochure);
        }


        [HttpPost]
        public ActionResult DeleteStudent(int id = 0)
        {
            Student student= db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            db.Students.Remove(student);
            db.SaveChanges();
            return Json("done", JsonRequestBehavior.AllowGet);
        }


        //
        // POST: /Brochure/Delete/5

        [HttpPost, ActionName("Delete")]
       
        public JsonResult DeleteConfirmed(int id)
        {
            Brochure brochure = db.Brochures.Find(id);
            db.Brochures.Remove(brochure);
            db.SaveChanges();
            return Json("done", JsonRequestBehavior.AllowGet);
        }



        public async Task<JsonResult> copyBrochure( string copyFrom,string copyTo,string returnUrl)
        {
           
            //string copyFrom = fc["copyFrom"].ToString();
            //string copyTo = fc["copyTo"].ToString();
            //string returnUrl = fc["returnUrl"].ToString();
            int cpyFrom,cpyTo;
            int.TryParse(copyFrom,out cpyFrom);
            int.TryParse(copyTo, out cpyTo);
            if(!ShrdMaster.Instance.checkBrochureID(copyTo))
            {
                List<CategoryMapping> map = db.CategoryToBrochureMap.Where(x => x.BrochureID == cpyFrom).ToList();
                await ShrdMaster.Instance.CopyBrochure(map, cpyTo);
                //string datatoreturn = copyFrom + "-" + copyTo + "-" + returnUrl;
                return Json(returnUrl, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Brochure ID already exists.enter another ID.", JsonRequestBehavior.AllowGet);
            }
           
        }

        public async Task<JsonResult> CreateBrochureFromBrochure(string BrochureID, string BrochureName,string copyFrom)
        {

            //string copyFrom = fc["copyFrom"].ToString();
            //string copyTo = fc["copyTo"].ToString();
            //string returnUrl = fc["returnUrl"].ToString();
           // string message = "";  
            if(ShrdMaster.Instance.checkBrochureID(BrochureID))
              {
             //     message = "";
                  return Json("0", JsonRequestBehavior.AllowGet);
              }

            Brochure b = new Brochure();
            b.BrochureID = BrochureID;
            b.BrochureName = BrochureName;
            
            db.Brochures.Add(b);
            db.SaveChanges();

            
            //Brochure createdBrochure =(Brochure)db.Brochures.Where(x => x.BrochureID == BrochureID && x.BrochureName == BrochureName);
            int cpyFrom,cpyto;
            cpyto = b.ID;
            int.TryParse(copyFrom, out cpyFrom);
            //int.TryParse(copyTo, out cpyTo);
            List<CategoryMapping> map = db.CategoryToBrochureMap.Where(x => x.BrochureID == cpyFrom).ToList();
            await ShrdMaster.Instance.CopyBrochure(map, cpyto);
            //string datatoreturn = copyFrom + "-" + copyTo + "-" + returnUrl;
            return Json("1", JsonRequestBehavior.AllowGet);
        }


        //public ActionResult MapCatoery(int ID)
        //{




        //}



        //Add / Delete Categories from Brochure
        public async Task<ActionResult> MapCategory(int BrochureID,string Categories)
        {
            //Add Categories to Brochure
            await ShrdMaster.Instance.AddCategoriesToBrochure(BrochureID, Categories);

            //get Categories for Brochure
            var BrochureCategories = db.Categories.ToList()
                .Join(db.CategoryToBrochureMap, cat => cat.ID, brochure => brochure.CategoryId, (cat, brochure) => new { Category = cat, CategoryMapping = brochure })
                .Where(b =>b.CategoryMapping.BrochureID == BrochureID).Select(both => both.Category).ToList();

            //return View
            return PartialView("_Category", BrochureCategories);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}