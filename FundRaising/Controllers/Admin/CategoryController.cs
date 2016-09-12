    using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FundRaising.Models;
using System.IO;
using FundRaising.Filters;
using System.Threading.Tasks;

namespace FundRaising.Controllers.Admin
{
    [Authorize(Users = "Admin")]
    [InitializeSimpleMembership]
    public class CategoryController : Controller
    {
        private FundRaisingDBContext db = new FundRaisingDBContext();

        //
        // GET: /Category/

        public ActionResult Index()
        {
            var categories=db.Categories.ToList();
            foreach (Category cat in categories)
            {
                cat.productsCount=db.Products.ToList().Join(
                    db.mappings.ToList(),
                            p => p.ID,
                                map => map.ProductID,
                                        (p, map) => new { Product = p, productToCategoryMap = map })
                                                .Where(both => both.productToCategoryMap.CategoryID == cat.ID).Select(both => both.Product).Distinct().Count();
            }
            //var cat=category.products = 
            return View(categories);
            
        }

        //
        // GET: /Category/Details/5

        public ActionResult Details(int id = 0)
        {
            int BrochureID=-1;
            string BrochureName=null;

            if(Session["BrochureID"]!=null)
            {
                int.TryParse(Session["BrochureID"].ToString(),out BrochureID);
            }
            if(Session["BrochureName"]!=null)
            {
               BrochureName=Session["BrochureName"].ToString();
            }

            

            Category category = db.Categories.Find(id);

            category.products = (ICollection<Product>)db.Products.ToList().Join(
                    db.mappings.ToList(), 
                            p => p.ID, 
                                map => map.ProductID,
                                        (p, map) => new { Product = p, productToCategoryMap = map })
                                                .Where(both => both.productToCategoryMap.CategoryID == id).Select(both => both.Product).Distinct().ToList();
            if (category == null)
            {
                return HttpNotFound();
            }
            Session["CategoryID"] = category.ID;
            Session["CategoryName"] = category.CategoryName;
            ViewBag.BrochureID = BrochureID;
            ViewBag.BrochureName = BrochureName;
            return View(category);
        }

        //
        // GET: /Category/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Category/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HttpPostedFileBase FileUpload,Category category)
            //public ActionResult Create(HttpPostedFileBase FileUpload,Category category)
        {
            string returnURl="";
            if (ModelState.IsValid)
            {
                if(!ShrdMaster.Instance.CheckCategoryID(category.CategoryID))
                {
                    string fileName = "";
                    string path = Server.MapPath("~/SiteImages");

                    if (FileUpload != null)
                    {
                        
                        fileName = FileUpload.FileName;                     
                        //code to generate different name if the filename exists
                        string ImageName = await ShrdMaster.Instance.SaveImage(path, fileName);
                        //string ImageName = ShrdMaster.Instance.SaveImage(path, fileName);
                        //fileName = FileUpload.FileName;
                        path = Path.Combine(path, ImageName);
                        FileUpload.SaveAs(path);
                        path = "/SiteImages/" + ImageName;

                        path = "/SiteImages/" + ImageName;
                        category.Image = path;                                                                
                    }

                  
                    db.Categories.Add(category);
                    db.SaveChanges();
                    if (Request.QueryString["returnUrl"] != null)
                    {
                        returnURl = Request.QueryString["returnUrl"].ToString();
                        return Redirect(returnURl);
                    }
                    else
                    {
                        return View();
                    }       
                }
                else
                {
                    ViewBag.error = "Category ID exists. enter another category ID";
                }
                                                                     
            }

            return View(category);
        }

        //
        // GET: /Category/Edit/5

        public ActionResult Edit(int id = 0)
        {
            int BrochureID = -1;
            string BrochureName = null;

            if (Session["BrochureID"] != null)
            {
                int.TryParse(Session["BrochureID"].ToString(), out BrochureID);
            }
            if (Session["BrochureName"] != null)
            {
                BrochureName = Session["BrochureName"].ToString();
            }

            
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            category.products = (ICollection<Product>)db.Products.ToList().Join(
                   db.productToCategoryMap.ToList(),
                           p => p.ID,
                               map => map.ProductID,
                                       (p, map) => new { Product = p, productToCategoryMap = map })
                                               .Where(both => both.productToCategoryMap.CategoryID == id).Select(both => both.Product).Distinct().ToList();
            
            Session["CategoryID"] = category.ID;
            Session["CategoryName"] = category.CategoryName;
            ViewBag.BrochureID = BrochureID;
            ViewBag.BrochureName = BrochureName;
            if(Request.QueryString["returnUrl"]!=null)
            {
                ViewBag.returnUrl = Request.QueryString["returnUrl"];
            }
            
            return View(category);
        }

        

        public async Task <ActionResult> MapProduct(int CategoryID,string products)
        {
            string returnValue="";
            int result=await ShrdMaster.Instance.AddProductsToCategory(CategoryID, products);
            //ProductMapping pm = new ProductMapping();
            //string[] productlist = products.Split(',');
            //if(!string.IsNullOrEmpty(products))
            //{
            //    foreach (string str in productlist)
            //    {
            //        pm.CategoryID = CategoryID;
            //        pm.ProductID = int.Parse(str);
            //        db.productToCategoryMap.Add(pm);
            //        db.SaveChanges(); 
            //    }
            //    returnValue="done-"+CategoryID;
            //}
            if(result!=-1)
            {
                returnValue = "done-" + CategoryID;
            }
            else
            {
                returnValue = "notdone-" + CategoryID;
            }
            
            return Json(returnValue, JsonRequestBehavior.AllowGet);
        }
        
        
        public ActionResult loadProducts(int CategoryID)
        {
            var Cateogry_Products = (ICollection<Product>)db.Products.ToList().Join(
               db.productToCategoryMap.ToList(),
                       p => p.ID,
                           map => map.ProductID,
                                   (p, map) => new { Product = p, MapCategory = map })
                                           .Where(both => both.MapCategory.CategoryID == CategoryID).Select(both => both.Product).Distinct().ToList();
            return PartialView("_products", Cateogry_Products);
        }
        //
        // POST: /Category/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(HttpPostedFileBase FileUpload,Category category,FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                string fileName = "";
                string path = Server.MapPath("~/SiteImages");

                if (FileUpload != null)
                {
                    fileName = FileUpload.FileName;
                    //code to generate different name if the filename exists
                    string ImageName = await ShrdMaster.Instance.SaveImage(path, fileName);
                    //string ImageName = ShrdMaster.Instance.SaveImage(path, fileName);
                    //fileName = FileUpload.FileName;
                    path = Path.Combine(path, ImageName);
                    FileUpload.SaveAs(path);
                    path = "/SiteImages/" + ImageName;                  
                    category.Image = path; 
                }

               
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                if (fc != null)
                {
                    if (fc["returnUrl"] != null)
                    {
                        return Redirect(fc["returnUrl"].ToString());
                    }
                }            
                return RedirectToAction("Index");
            }
            category.products = (ICollection<Product>)db.Products.ToList().Join(
                  db.mappings.ToList(),
                          p => p.ID,
                              map => map.ProductID,
                                      (p, map) => new { Product = p, MapCategory = map })
                                              .Where(both => both.MapCategory.CategoryID == category.ID).Select(both => both.Product).Distinct().ToList();
            return View(category);
        }

        //
        // GET: /Category/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //
        // POST: /Category/Delete/5

        [HttpPost, ActionName("Delete")]
      
        public JsonResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();

            return Json("done", JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> GetProductsByCategoryID(int ID)
        {
            List<Product> List=await ShrdMaster.Instance.GetProductsByCategoryID(ID);
            BrochureViewModel bvm = new BrochureViewModel();
            bvm.ProductList = List;
            return Json(bvm, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> CopyCategory(int CopyFrom, int CopyTo)
        {
          await ShrdMaster.Instance.CopyCategory(CopyFrom, CopyTo);

            return Json("1", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> CopyCategoryToNew(int CopyFrom, string CategoryID,string Name)
        {
            if(!ShrdMaster.Instance.CheckCategoryID(CategoryID))
            {
                await ShrdMaster.Instance.CopyToNewCategory(CopyFrom, CategoryID, Name);
            }
            
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}