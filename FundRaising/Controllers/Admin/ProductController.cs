using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FundRaising.Filters;
using FundRaising.Models;
using FundRaising.ViewModels;
using Ionic.Zip;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;

namespace FundRaising.Controllers.Admin
{
    [Authorize(Users = "Admin")]
    [InitializeSimpleMembership]
    public class ProductController : Controller
    {
        private FundRaisingDBContext db = new FundRaisingDBContext();

        //
        // GET: /Product/

        public ActionResult Index()
        {
           

            return View();
        }

        public ActionResult Products(int option=0)
        {
            var productlist = db.Products.Where(x => x.ItemActive == true).ToList();
            List<ProductViewModel> plist = new List<ProductViewModel>();
            if (option==0)
            {                
                ProductViewModel product = null;
                productlist.ForEach(x =>
                {
                    product = new ProductViewModel();
                    product.CustomerRetailPrice = x.CustomerRetailPrice;
                    product.FundTrackerRetailPrice = x.FundTrackerPrice;
                    product.ID = x.ID;
                    product.Name = x.Description;
                    product.ItemNumber = x.ItemNumber;
                    product.InventoryAmount = (int)x.InventoryAmount;
                    if (x.InventoryAmount < 0)
                    {
                        product.sInventoryAmount = "Unlimited";
                    }
                    else
                    {
                        product.sInventoryAmount = string.Format("{0:0.00}", product.InventoryAmount);
                    }
                    if (x.productType == 1)
                    {
                        product.sProductType = "Product";
                    }
                    else if (x.productType == 2)
                    {
                        product.sProductType = "Subscription";
                    }
                    else
                    {
                        product.sProductType = "Donation";
                    }
                    //product.sProductType = ;
                    plist.Add(product);

                });
                return PartialView("_ProductView", plist);
            }
            else
            {

                plist=db.Database.SqlQuery<ProductViewModel>("exec sp_GetMagazines").ToList();
                return PartialView("_MagazineView", plist);
            }
            

           // return PartialView("_ProductView", plist);
        }


        //
        // GET: /Product/Details/5

        public ActionResult Details(int id = 0)
        {

            Product product = db.Products.Find(id);


            if (product == null)
            {
                return HttpNotFound();
            }
            if (Session["CategoryID"] != null && Session["CategoryName"] != null)
            {
                ViewBag.CategoryID = Session["CategoryID"];
                ViewBag.CategoryName = Session["CategoryName"];
            }
            if (Session["BrochureID"] != null && Session["BrochureName"] != null)
            {
                ViewBag.BrochureID = Session["BrochureID"].ToString();
                ViewBag.BrochureName = Session["BrochureName"].ToString(); ;
            }
          
            return View(product);
        }

        //
        // GET: /Product/Create

        public ActionResult Create(int option=0)
        {
            ViewBag.ProductType =new SelectList(Common.ProductTypes(),"ID","Description");
            ViewBag.option = option;
            SetOption(option);
            return View();
        }

        //
        // POST: /Product/Create

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]        
        public async Task<ActionResult> Create(Product product,HttpPostedFileBase FileUpload)        
        {
            ViewBag.ProductType = new SelectList(Common.ProductTypes(), "ID", "Description");
            int option = GetOption();
            ViewBag.option = option;
            if (ModelState.IsValid)
            {                                
                if(!ShrdMaster.Instance.checkItemNumber(product.ItemNumber))
                {
                    string returnURl = "";
                    string fileName = "";
                    string path = Server.MapPath("~/SiteImages");
                    string filePath = path;
                    if (FileUpload != null)
                    {
                        if(FileUpload.ContentLength<150000)
                        {
                            fileName = FileUpload.FileName;
                            //code to generate different name if the filename exists
                            //string ImageName = ShrdMaster.Instance.SaveImage(path, fileName);
                            string ImageName = await ShrdMaster.Instance.SaveImage(path, fileName);
                            //fileName = FileUpload.FileName;
                            path = Path.Combine(path, ImageName);
                            FileUpload.SaveAs(path);
                           // Image image;
                            string imagename1;
                            //image = ShrdMaster.Instance.ResizeImage(oldIamge, path);
                            //oldIamge.Dispose();
                            
                            using (var srcImage = Image.FromFile(path))
                            {
                                var newWidth = (int)(150);
                                var newHeight = (int)(150);
                                using (var newImage = new Bitmap(newWidth, newHeight))
                                using (var graphics = Graphics.FromImage(newImage))
                                {


                                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                    graphics.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight));
                                    imagename1 = await ShrdMaster.Instance.SaveImage(filePath, fileName);
                                    filePath = Path.Combine(filePath, imagename1);
                                    newImage.Save(filePath);
                                }
                            }
                            System.IO.File.Delete(path);
                            //image.Save(path);
                            path = "/SiteImages/" + imagename1;
                            product.ImageUrl = path;
                        }
                        else
                        {
                            
                            ViewBag.error = "Image length exceeds. try to insert less then 150bytes.";
                            return View(product); 
                        }
                        
                    }
                    if(product.InventoryAmount>=0)
                    {
                        product.Inventory = true;
                    }

                    
                    

                    if(option==1)
                    {
                        var priceData = JsonConvert.DeserializeObject<List<MagazinePrice>>(product.MagazinePrice);
                        if(priceData.Count>0)
                        {
                            priceData.ForEach(x => {
                                product.ItemNumber = x.ItemNumber;
                                db.Products.Add(product);
                                db.SaveChanges();
                                MagazinePriceMapping mp = new MagazinePriceMapping();
                                mp.Issue = x.Issue;
                                mp.Price = x.Price;
                                mp.description = product.Description;
                                mp.ItemNumber = x.ItemNumber;
                                mp.ProductID = product.ID;

                                db.MagazinePriceMappings.Add(mp);
                                db.SaveChanges();
                            });

                            //product.ItemNumber = priceData[0].ItemNumber;
                            //db.Products.Add(product);
                            //db.SaveChanges();
                        }

                    }
                    else
                    {
                        db.Products.Add(product);
                        db.SaveChanges();
                    }

                    if (Request.QueryString["returnUrl"] != null)
                    {
                        returnURl = Request.QueryString["returnUrl"].ToString();
                        return Redirect(returnURl);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }                                       
                }
                else
                {
                    ViewBag.error="Product Active with item number";
                }
               
                
            }
           
            return View(product);
        }

        //
        // GET: /Product/Edit/5

        public ActionResult Edit(int id = 0,int option=0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            SetOption(option);
            ViewBag.option = option;
            if (Session["CategoryID"] != null && Session["CategoryName"] != null)
            {
                ViewBag.CategoryID = Session["CategoryID"];
                ViewBag.CategoryName = Session["CategoryName"];
            }
            if (Session["BrochureID"] != null && Session["BrochureName"] != null)
            {
                ViewBag.BrochureID = Session["BrochureID"].ToString();
                ViewBag.BrochureName = Session["BrochureName"].ToString(); ;
            }
            ViewBag.ProductType = new SelectList(Common.ProductTypes(), "ID", "Description",product.productType);
            // end code for BreadCrumb(SiteMap)




            if (product == null)
            {
                return HttpNotFound();
            }

            // code to go to previous page
            if(Request.QueryString["returnUrl"]!=null)
            {
                ViewBag.returnUrl = Request.QueryString["returnUrl"];
            }
            if(option==1)
            {
                Session["description"] = product.Description;
                product.MagazinePriceList = db.Database.SqlQuery<MagazinePriceMappingModel>("exec sp_getMagazinePriceList @description",new SqlParameter("@description",product.Description)).ToList();
            }
            //var data =db.MagazinePriceMappings.Where(x => x.description == product.Description).ToList();
            
            ViewBag.Shipping = product.ChargeShipping;

            return View(product);
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(Product product,FormCollection fc,HttpPostedFileBase FileUpload)
        {

            int option;
            ViewBag.ProductType = new SelectList(Common.ProductTypes(), "ID", "Description", product.productType);
            
            option =GetOption();
            ViewBag.option = option;
            if (ModelState.IsValid)
            {
                //string returnURl = "";
                string fileName = "";
                string path = Server.MapPath("~/SiteImages");
                string filePath = path;
                if (FileUpload != null)
                {
                    if (FileUpload.ContentLength < 150000)
                    {
                        fileName = FileUpload.FileName;
                        //code to generate different name if the filename exists
                        string ImageName = await ShrdMaster.Instance.SaveImage(path, fileName);
                        //fileName = FileUpload.FileName;
                       
                        path = Path.Combine(path, ImageName);
                        FileUpload.SaveAs(path);
                        //Image oldIamge = Image.FromFile(path);
                        Image image;
                        string imagename1;
                        //image = ShrdMaster.Instance.ResizeImage(oldIamge, path);
                        //oldIamge.Dispose();
                        using (var srcImage = Image.FromFile(path))
                        {
                            var newWidth = (int)(150);
                            var newHeight = (int)(150);
                            using (var newImage = new Bitmap(newWidth, newHeight))
                            using (var graphics = Graphics.FromImage(newImage))
                            {


                                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                graphics.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight));
                                 imagename1 = await ShrdMaster.Instance.SaveImage(filePath, fileName);
                                filePath = Path.Combine(filePath, imagename1);
                                newImage.Save(filePath);                               
                            }
                        }
                        System.IO.File.Delete(path);
                       //image.Save(path);
                       path = "/SiteImages/" + imagename1;
                       product.ImageUrl = path;
                        //path = "/SiteImages/" + fileName;
                        //product.ImageUrl = path;
                    }
                    else
                    {
                        
                        ViewBag.error = "Image size exceeds insert another image";
                        
                        return View(product);
                    }
                  
                }

                if (product.InventoryAmount >= 0)
                {
                    product.Inventory = true;
                }
                if(option==1)
                {
                    db.Database.ExecuteSqlCommand("exec sp_UpdateProduct @description,@CustomerRetailPrice,@FundtrackerPrice,@itemWeight,@ChargeSalesTax,@ChargeShipping,@ItemOverSize,@ItemActive,@InventoryAmount,@inventory,@ItemExtraTitle,@ItemExtraFileName,@detaildescription,@imageUrl,@shiptoSchoolOnly,@olddescription ", 
                        new SqlParameter("@description", product.Description), new SqlParameter("@CustomerRetailPrice", product.CustomerRetailPrice),
                        new SqlParameter("@FundtrackerPrice", product.FundTrackerPrice), new SqlParameter("@itemWeight", product.ItemWeight), 
                        new SqlParameter("@ChargeSalesTax", product.ChargeSalesTax), new SqlParameter("@chargeShipping", product.ChargeShipping),
                        new SqlParameter("@ItemOverSize", product.ItemOverSize), new SqlParameter("@ItemActive", product.ItemActive),
                        new SqlParameter("@InventoryAmount", product.InventoryAmount), new SqlParameter("@inventory", product.Inventory), 
                        new SqlParameter("@ItemExtraTitle", product.ItemExtraTitle),
                        new SqlParameter("@ItemExtraFileName", product.ItemExtraFileName), new SqlParameter("@Detaildescription", product.DetailDescription),
                        new SqlParameter("@imageUrl", string.IsNullOrEmpty(product.ImageUrl) ? "":product.ImageUrl), 
                        new SqlParameter("@ShipToSchoolOnly", product.ShipToSchoolOnly), new SqlParameter("@oldDescription", Session["description"])
                        );

                    var magazinePriceData = JsonConvert.DeserializeObject<List<MagazinePriceMapping>>(product.MagazinePrice).ToList();
                    magazinePriceData.ForEach(x =>
                    {
                        db.Database.ExecuteSqlCommand("exec sp_UpdateMagazinePrice @itemNumber,@price,@issue,@description", new SqlParameter("@itemNumber", x.ItemNumber), new SqlParameter("@price", x.Price), new SqlParameter("@issue", x.Issue), new SqlParameter("@description", product.Description));
                    });
                    
                }
                product.MagazinePriceList = db.Database.SqlQuery<MagazinePriceMappingModel>("exec sp_getMagazinePriceList @description",new SqlParameter("@description",Session["description"])).ToList();
                if (fc!=null)
                {
                    if (fc["returnUrl"]!=null)
                    {
                        return Redirect(fc["returnUrl"].ToString());
                    }                    
                }
                return RedirectToAction("Index");
            }
            ViewBag.ItemNumber = product.ItemNumber;
            ViewBag.ProductType = new SelectList(Common.ProductTypes(), "ID", "Description", product.productType);
            return View(product);
        }

        //
        // GET: /Product/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/Delete/5

        [HttpPost,ActionName("Delete")]       
        public JsonResult DeleteConfirmed(int id,string description="" )
        {
            try
            {
                if (!string.IsNullOrEmpty(description))
                {
                    db.Database.ExecuteSqlCommand("exec sp_deletemagazine @description", new SqlParameter("@description", description));
                }
                else
                {
                    Product product = db.Products.Find(id);
                    db.Products.Remove(product);
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {

            }
                       
            return Json("done", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductByID(int ID)
        {
            Product product=  ShrdMaster.Instance.GetProductByID(ID);

            if(product==null)
            {
                product = new Product();
                product.ID = -1;
            }
            return Json(product, JsonRequestBehavior.AllowGet);

        }


        //public JsonResult CopyProduct(int productID, string itemnumber)
        //{
        //    return Json("asd", JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public async Task<JsonResult> SaveProduct(int productID, string itemnumber)
        {

            // int itm_number;
            Product product = ShrdMaster.Instance.GetProductByID(productID);
            if (!ShrdMaster.Instance.checkItemNumber(itemnumber))
            {
                if (product == null)
                {
                    return Json("-1", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    product.ItemNumber = itemnumber;
                    await ShrdMaster.Instance.SaveProduct(product);

                    return Json("1", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("-1", JsonRequestBehavior.AllowGet);
            }



        }


        public Product MapProduct(string row,DirectoryInfo[]dirs)
        {
            Product product = new Models.Product();
            var valArray = row.Split(',');
            double customerRetailPrice=0.0,fundTrackerPrice=0.0,itemWeight=0.0;
            bool chargeSalesTax = false, chargeShipping = false, itemOverSize = false,shipToSchoolOnly=false,existingItem=false;
            int inventoryAmount = 0;
            string itemNumber = "", imagePath = "";            
           
            //return product;
            if (ShrdMaster.Instance.checkItemNumber(valArray[0]))
            {
                existingItem = true; 
                product=db.Products.FirstOrDefault(x => x.ItemNumber == valArray[0]);
                itemNumber = product.ItemNumber;
            }
            else
            {
                itemNumber = valArray[0];
                if (itemNumber.Length < 4)
                {
                    itemNumber = MakeItemNumber(itemNumber);
                }
                product.ItemNumber = itemNumber;
            }
            product.productType = 1;
            product.Description = valArray[1];
            double.TryParse(valArray[2], out customerRetailPrice);
            product.CustomerRetailPrice = customerRetailPrice;
            double.TryParse(valArray[3], out fundTrackerPrice);
            product.FundTrackerPrice = fundTrackerPrice;
            double.TryParse(valArray[4], out itemWeight);
            product.ItemWeight = itemWeight;

            bool.TryParse(valArray[5], out chargeSalesTax);
            product.ChargeSalesTax = chargeSalesTax;

            bool.TryParse(valArray[6], out chargeShipping);
            product.ChargeShipping = chargeShipping;

            bool.TryParse(valArray[7], out itemOverSize);
            product.ItemOverSize = itemOverSize;

            int.TryParse(valArray[8], out inventoryAmount);
            product.InventoryAmount = inventoryAmount;
            if(inventoryAmount>0)
            {
                product.Inventory = true;
            }
            else
            {
                product.Inventory = false;
            }

            product.ItemExtraTitle = valArray[9];

            product.ItemExtraFileName= valArray[10];
            product.DetailDescription = valArray[11];

            bool.TryParse(valArray[12], out shipToSchoolOnly);
            product.ShipToSchoolOnly = shipToSchoolOnly;
            bool result = MovePicture(itemNumber,dirs, out imagePath);
            product.ImageUrl = result ? imagePath : "";
            product.Issue = 0;
            product.Price= 0;
            if (!existingItem)
            {
                
                return product;

            }
            else
            {
                return null;
            }
            
        }



        [HttpPost]
        public ActionResult UploadProducts()
        {

            string CSVFile="", ImageFolder="";
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get folder Name
                    HttpPostedFileBase file = Request.Files[0];
                    string fname="";

                    
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }


                   //checking the files within the zipped folder
                    
                    ZipFile zip = ZipFile.Read(file.InputStream);
                    //string filename = "", ext = "";

                    string uploadPath = Server.MapPath("~/Uploads");
                    //creating the directory
                    string folderName = "upload" + DateTime.Now.Day + "#" + DateTime.Now.Month + "#" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second;
                    string path=Path.Combine(uploadPath, folderName);
                    Directory.CreateDirectory(path);                                        
                    //Unzip CSV file and ZippedFolder  from folder
                    zip.ExtractAll(path, ExtractExistingFileAction.DoNotOverwrite);

                    string [] subfiles = Directory.GetFiles(path);
                    if(subfiles == null || subfiles.Length ==0)
                    {
                        fname = fname.Substring(0, fname.LastIndexOf('.'));
                        fname = Path.Combine(path, fname);
                    }
                    else
                    {
                        fname = path;
                        //string[] subdirs= Directory.GetDirectories(path);
                        //if(subdirs != null)
                        //{
                        //    fname = Path.Combine(path, subdirs[0]);
                        //}                        
                    }
                    
                    

                    ////Storage Folder Path
                    


                    DirectoryInfo dir = new DirectoryInfo(fname);
                    FileInfo[] files = dir.GetFiles();
                    DirectoryInfo[] dirs = dir.GetDirectories();

                    foreach(FileInfo f in files)
                    {
                        if(f.Extension.ToLower()==".csv")
                        {
                            CSVFile = Path.Combine(fname, f.FullName);
                            break;
                        }
                    }

                    List<Product> products = new List<Product>();
                    CsvFileReader CSV = new CsvFileReader(CSVFile);
                    string csvData=CSV.ReadToEnd();
                    foreach (string row in csvData.Split('\n'))
                    {

                        Product p1=new Models.Product ();
                        if (!string.IsNullOrEmpty(row) && !row.Contains("ItemNumber"))
                        {
                            p1 = MapProduct(row,dirs);
                            if (p1!=null)
                            {
                                
                                products.Add(p1);
                            }
                            
                            
                            
                            //SaveProduct(rowvalue, imagePath, itemnumber);
                        }
                    }

                   
                    // Returns message that successfully uploaded  
                    CSV.Close();
                    Directory.Delete(path, true);
                    
                     SaveToSql(products);
                    DirectoryInfo tempdirectory = new DirectoryInfo(Server.MapPath("~/TempImages/"));
                    foreach(var tempfile in tempdirectory.GetFiles())
                    {
                        tempfile.Delete();
                    }
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }  
            
            //return View();
        }

        public void  SaveToSql(List<Product>products)
        {            
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString))
            {
                con.Open();
                using (SqlBulkCopy copy = new SqlBulkCopy(con))
                {
                    copy.DestinationTableName = "Products";
                    var tbl = products.AsDataTable();
                    copy.WriteToServer(products.AsDataTable());
                }
            }
        }

        public string MakeItemNumber(string itemnumber)
        {
            int length = itemnumber.Length;
            while(length<4)
            {
                itemnumber = "0" + itemnumber;
                length = itemnumber.Length;
            }
            return itemnumber;
        }

      
        

        public bool MovePicture(string number, DirectoryInfo[] dirs,out string imagePath)
        {
            FileInfo []images=null;
            string imageFolder = "", imageName = "", ext = "", name = ""; 
            imagePath = "";
            string tempfolder=Server.MapPath("~/TempImages");
            string destination = Server.MapPath("~/SiteImages");
            
            int count = 0;
            int c = 0,index=0;
            foreach(DirectoryInfo d in dirs)
            {
                images=d.GetFiles();
                if(images!=null)
                {
                    foreach(FileInfo img in images)
                    {
                        index = img.Name.LastIndexOf('.');
                        imageName=img.Name.Substring(0,index);
                        ext = img.Name.Substring(index, (img.Name.Length-index));

                        if(number==imageName)
                        {
                            
                            //string imagename1;
                            tempfolder = GetPath(tempfolder,img.Name, out name);
                            img.MoveTo(tempfolder);
                            //image = ShrdMaster.Instance.ResizeImage(oldIamge, path);
                            //oldIamge.Dispose();
                            using (var srcImage = Image.FromFile(tempfolder))
                            {
                                GetPath(destination, img.Name, out name);
                                ResizeImage(srcImage,name);
                                
                                //newimage.Save(name);
                                
                                //var newWidth = (int)(150);
                                //var newHeight = (int)(150);
                                //using (var newImage = new Bitmap(newWidth, newHeight))
                                //using (var graphics = Graphics.FromImage(newImage))
                                //{
                                //    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                //    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                //    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                //    graphics.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight));
                                    
                                    
                                //    newImage.Save(destination);
                                //}
                            }
                            imagePath = "/SiteImages/" + img.Name;
                            count++;
                            //System.IO.File.Delete(path);
                            //image.Save(path);
                            //path = "/SiteImages/" + imagename1;


                            //long imglength = img.Length;
                            //if (imglength<150000)
                            //{


                            //    imagePath = "/SiteImages/" + name;
                            //    count = 1;
                            //}                          
                            break;
                        }
                    }
                    imageFolder = d.FullName;
                    break;
                }
            }

            if(count==1)
            {
                return true;
            }
            else
            {
                return false;
            }
         //   return false;

        }




        public void ResizeImage(Image image, string path)
        {
            var newWidth = (int)(150);
            var newHeight = (int)(150);
            using (var newImage = new Bitmap(newWidth, newHeight))
            {
                using (var graphics = Graphics.FromImage(newImage))
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight));
                    newImage.Save(path);
                    //return newImage;
                }
            }
        }
        public string  GetPath(string destination,string ImageName,out string Name)
        {

            string path = destination + "//" + ImageName; 
                //ext = "";
                //string[] firstname;
            //int c=1,index;
            Name = "";

            while(System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
               //  index = ImageName.LastIndexOf('.');
               // firstname = ImageName.Substring(0, index).Split('-');
               // ext = ImageName.Substring(index,(ImageName.Length - index));
               // ImageName = firstname[0]+"-"+c + ext;
               // Name = ImageName;
               // path = destination + "//" + ImageName;
               //// GetPath(destination, ImageName,out Name);               
               // c++;
            }
            Name = path;
            return destination + "//" + ImageName;
   


        }


        public void SetOption(int option)
        {
            HttpCookie optionCookie = new HttpCookie("OptionCookie");
            optionCookie.Value = option.ToString();
            Response.Cookies.Add(optionCookie);
        }

        public int GetOption()
        {
            int option;
            int.TryParse(Request.Cookies["OptionCookie"].Value, out option);

            return option;
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}