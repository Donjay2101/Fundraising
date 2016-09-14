using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FundRaising.Models;
using FundRaising.ViewModels;
using System.Data.SqlClient;

namespace FundRaising.Views.Customer
{
    public class StoreController : Controller
    {

        FundRaisingDBContext db = new FundRaisingDBContext();
        public const int RecordsPerPage = 20;
        //
        // GET: /Store/

        public ActionResult Index()
        {
            
            
            return View();
        }

        public ActionResult Browse(int? pageNum,int CategoryID=-1)
        {

            pageNum = pageNum ?? 1;
            int BrochureID = -1;
            ViewBag.IsEndOfRecords = false;
            Category category = null;
            string SchoolID="0";
            Organization org= null;

            if (Session["SchoolID"] != null)
            {
                SchoolID =Session["SchoolID"].ToString() ;
                org = db.Organizations.Where(x => x.SchoolID == SchoolID && x.IsActive == true).SingleOrDefault();
            }
            else
            {
                return RedirectToAction("Select", "Customer");
            }
            ICollection<ProductShopViewModel> list = null;
          if(Request.IsAjaxRequest())
          {
             
              if(Request.Cookies["CategoryID"]!=null)
              {
                  int.TryParse(Request.Cookies["CategoryID"].Value,out CategoryID);
                  ViewBag.CategoryID = CategoryID;
              }
              else
              {
                  
                  if(Request.Cookies["StudentID"]!=null)
                  {                      
                      int studentid=int.Parse(Request.Cookies["StudentID"].Value);
                      return RedirectToAction("Index", "Customer", new { ID = studentid });
                  }
                  
              }
             
             list = (ICollection<ProductShopViewModel>)db.Products.ToList().Join(
              db.productToCategoryMap.ToList(),
                      p => p.ID,
                          map => map.ProductID,
                                  (p, map) => new { Product = p, ProductMapping = map })
                                          .Where(both => both.ProductMapping.CategoryID == CategoryID).Select(both => new ProductShopViewModel
                                          {
                                              CustomerRetailPrice = both.Product.CustomerRetailPrice,
                                              ID = both.Product.ID,
                                              Description = both.Product.Description,
                                              ImageUrl = both.Product.ImageUrl
                                          }).Skip(pageNum.Value * 10).Take(9).Distinct().ToList();
                if (!org.ShipToSchool)
              {                  
                  list = (ICollection<ProductShopViewModel>)list.Where(x => x.ShipToSchoolOnly == false);
              }
              
              int custIndex = 1;
            //  var ProductsDictionary = ;
              var products = list.ToDictionary(x => custIndex++, x => x);
              ViewBag.IsEndOfRecords = (products.Any()) && ((pageNum.Value * RecordsPerPage) >= products.Last().Key);
              return PartialView("_productView",products);
          }
          else
          {
              category = db.Categories.Find(CategoryID);

              HttpCookie CookieCategoryID = new HttpCookie("CategoryID");
              CookieCategoryID.Value = CategoryID.ToString();
              Response.Cookies.Add(CookieCategoryID);
              //getting products from Database

              ViewBag.CategoryID = CategoryID;
              
              if (Session["BrochureID"] != null)
              {
                  int.TryParse(Session["BrochureID"].ToString(), out BrochureID);
              }
             
              if(!org.ShipToSchool)
              {

              }

                if(CategoryID==17)
                {
                    list = db.Database.SqlQuery<ProductShopViewModel>("exec sp_GetMagazinesforShopPage").ToList();
                }
                else
                {
                    list = (ICollection<ProductShopViewModel>)db.Products.ToList().Join(
              db.productToCategoryMap.ToList(),
                      p => p.ID,
                          map => map.ProductID,
                                  (p, map) => new { Product = p, ProductMapping = map })
                                          .Where(both => both.ProductMapping.CategoryID == CategoryID).Select(both =>new ProductShopViewModel{
                                              CustomerRetailPrice=both.Product.CustomerRetailPrice,                                              
                                              ID=both.Product.ID,
                                              Description = both.Product.Description,
                                              ImageUrl = both.Product.ImageUrl                                                                                    
                                          }).Skip(0 * 10).Take(9).Distinct().ToList();
                }

              
               if (!org.ShipToSchool)
               {
                   list = (ICollection<ProductShopViewModel>)list.Where(x => x.ShipToSchoolOnly == false).ToList();
               }
              int custIndex = 1;
              var ProductsDictionary = list.ToDictionary(x => custIndex++, x => x);           
              ViewBag.BrochureID = BrochureID;
              ViewBag.TotalNumberProducts = list.Count();
              ViewBag.products = ProductsDictionary;

              //ViewBag.products = GetRecordsForPage(pageNum.Value, ProductsDictionary);
              //Session["CategoryID"] = category.ID;
              //Session["CategoryName"] = category.CategoryName;
          }

        
          Session["CategoryID"] = category.ID;
          Session["CategoryName"] = category.CategoryName;
           // category.products = list;
            //ViewBag.BrochureID = BrochureID;
            if(Session["studentID"]!=null)
            {
                ViewBag.studentID = Session["studentID"].ToString();
            }
            //ViewBag.BRowseURl = "/Store/Browse?CategoryID="+category.ID+"?";            
            return View();
        }

        public Dictionary<int, Product> GetRecordsForPage(int pageNum, Dictionary<int, Product> products)
        {
            //Dictionary<int, Product> products = (ProductList as Dictionary<int, Product>);

            int from = (pageNum * RecordsPerPage);
            int to = from + RecordsPerPage;

            return products
                .Where(x => x.Key > from && x.Key <= to)
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public void LoadProductsToSession(int CategoryID)
        {
            
        }

        public ActionResult Details(int ID)
        {
            if(Session["BrochureID"]!=null)
            {
                ViewBag.BrochureID = Session["BrochureID"].ToString();
            }

            if(Session["studentID"]!=null)
            {
                ViewBag.studentID = Session["studentID"].ToString();
            }
            if (Session["categoryID"] != null && Session["CategoryName"]!=null)
            {
                ViewBag.caterogyID = Session["CategoryID"]+"-"+Session["CategoryName"];
            }

            ViewBag.caterogy = Session["CategoryID"];
            Product product = db.Products.Find(ID);
            if (ViewBag.caterogy==17)
            {
                product.MagazinePriceList = db.Database.SqlQuery<MagazinePriceMappingModel>("exec sp_getMagazinePriceList @description", new SqlParameter("@description", product.Description)).ToList();
            }

                HttpCookie catcookie = new HttpCookie("categoryCookie");
            catcookie.Value = Session["CategoryID"].ToString();
            Response.Cookies.Add(catcookie);
            
            if(product.ShipToSchoolOnly)
            {
                ViewBag.ShipToSchooolOnly = 1;
            }
            else
            {
                ViewBag.ShipToSchooolOnly = 0;
            }
            
            return View(product);
        }


        [ChildActionOnly]
        public ActionResult CategoryMenu(int ID)
        {
            ICollection<Category> list = null;
            if(ID==0007)
            {
                list=(ICollection<Category>)db.Categories.ToList();
            }
            else
            {
                list = (ICollection<Category>)(db.Categories.Join
                                            (db.CategoryToBrochureMap, c => c.ID,
                                                        MC => MC.CategoryId,
                                                                (c, MC) => new { Category = c, MapCategory = MC }).

                                                               Where(both => both.MapCategory.BrochureID == ID)).Select(both => both.Category).Distinct().ToList();
            }

           return PartialView(list);
        }
    }
}
