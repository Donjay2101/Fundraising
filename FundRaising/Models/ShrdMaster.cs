using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ionic.Zip;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FundRaising.Models
{
    public class ShrdMaster
    {
        static ShrdMaster _Instance;
        FundRaisingDBContext db=new FundRaisingDBContext ();


        //string[] States = new string[] { "AL","AK","AZ","AR","CA","CO","CT","DE","FL","GA","HI","ID","IL","IN","IA","KS","KY","LA","ME","MD","MA","MI","MN","MS","MO","MT","NE","NV","NH","NJ","NM","NY","NC","ND","OH","OK","OR","PA","RI","SC","SD","TN","TX","UT","VT","VA","WA","WV","WI","WY"};
        Dictionary<string,string> States=new Dictionary<string,string> ();
        public static ShrdMaster Instance
        {
            get
            {
                if(_Instance==null)
                {
                    _Instance = new ShrdMaster();
                }

                return _Instance;
            }
        }

        public Dictionary<String,String> GStates
        {
            get
            {
                if(States.Count<1)
                {
                    States.Add("AL", "Alabama");
                    States.Add("AK", "Alaska");
                    States.Add("AZ", "Arizona");
                    States.Add("AR", "Arkansas");
                    States.Add("CA", "California");
                    States.Add("CO", "Colorado");
                    States.Add("CT", "Connecticut");
                    States.Add("DE", "Delaware");
                    States.Add("FL", "Florida");
                    States.Add("GA", "Georgia");
                    States.Add("HI", "Hawaii");
                    States.Add("ID", "Idaho");
                    States.Add("IL", "Illinois");
                    States.Add("IN", "Indiana");
                    States.Add("IA", "Iowa");
                    States.Add("KS", "Kansas");
                    States.Add("KY", "Kentucky");
                    States.Add("LA", "Louisiana");
                    States.Add("ME", "Maine");
                    States.Add("MD", "Maryland");
                    States.Add("MA", "Massachusetts");
                    States.Add("MI", "Michigan");
                    States.Add("MN", "Minnesota");
                    States.Add("MS", "Mississippi");
                    States.Add("MO", "Missouri");
                    States.Add("MT", "Montana");
                    States.Add("NE", "Nebraska");
                    States.Add("NV", "Nevada");
                    States.Add("NH", "New Hampshire");
                    States.Add("NJ", "New Jersey");
                    States.Add("NM", "New Mexico");
                    States.Add("NY", "New York");
                    States.Add("NC", "North Carolina");
                    States.Add("ND", "North Dakota");
                    States.Add("OH", "Ohio");
                    States.Add("OK", "Oklahoma");
                    States.Add("OR", "Oregon");
                    States.Add("PA", "Pennsylvania");
                    States.Add("RI", "Rhode Island");
                    States.Add("SC", "South Carolina");
                    States.Add("SD", "South Dakota");
                    States.Add("TN", "Tennessee");
                    States.Add("TX", "Texas");
                    States.Add("UT", "Utah");
                    States.Add("VT", "Vermont");
                    States.Add("VA", "Virginia");
                    States.Add("WA", "Washington");
                    States.Add("WV", "West Virginia");
                    States.Add("WI", "Wisconsin");
                    States.Add("WY", "Wyoming");

                }


                return States;
            }
        }



        #region Brochure
        public async  Task CopyBrochure(List<CategoryMapping> map,int copyTo=-1)
        {

            foreach (CategoryMapping m in map)
            {
                m.BrochureID = copyTo;
                db.CategoryToBrochureMap.Add(m);
                await Task.Run(() => { db.SaveChanges(); });
            }
            //return 1;
        }


        public List<Category> GetByBrochureID(string ID)
        {
            
            int bID;
            int.TryParse(ID,out bID);
            var plist = db.Categories.ToList();

            //mark all unchecked
            plist.ForEach(x => x.check = null);


            //mapped values
            var list = plist
                        .Join(
                                db.CategoryToBrochureMap.ToList(),
                                c => c.ID,
                                map => map.CategoryId,
                                (p, map) => new { Category = p,CategoryMapping=map})
                        .Where(
                                x => x.CategoryMapping.BrochureID==bID)
                        .Select(x => x.Category).ToList() ;



            if (list.ToList().Count > 0)
            {
                //mark check which are mapped
                list.ToList().ForEach(x => x.check = "checked");

                //Products which doesnot fall under the condition(products which are not mapped) 
                var restCategories = db.Categories.ToList().Except(list).AsEnumerable();

                //join mapped products and unmapped produts
                list = list.Union(restCategories).ToList();
            }
            else
            {
                //all categories
                list = plist;
            }

            return list;
        }

        public bool CheckState(string ShippingCountry, string SalesTaxCountry)
        {
            string states = GStates[SalesTaxCountry] ;

            if (states.ToUpper() == ShippingCountry.ToUpper() || ShippingCountry.ToUpper() == "TEXAS" || ShippingCountry.ToUpper() == "TX")
            {
                return true;
            }
            return false;
        }

        public List<Product> getProductsByBrochureID_and_CategoryID(string BrochureID, string CategoryID)
        {
            int bID, cID;
            int.TryParse(BrochureID, out bID);
            int.TryParse(CategoryID, out cID);
            
            //taking out all the products
            var pList = db.Products.ToList();

            //mark all unchecked
            pList.ForEach(x=>x.check=null);

            //mapped products
            var productlist = pList
                        .Join(
                                db.mappings.ToList(),
                                p => p.ID,
                                map => map.ProductID,
                                (p, map) => new { Product = p, MapCategory = map })
                        .Where(
                                x => x.MapCategory.BrochureID == bID && x.MapCategory.CategoryID == cID)
                        .Select(x => x.Product).AsEnumerable();


          
            if(productlist.ToList().Count>0)
            {
                //mark check which are mapped
                productlist.ToList().ForEach(x=>x.check="checked");
              
                //Products which doesnot fall under the condition(products which are not mapped) 
                var restProducts =db.Products.ToList().Except(productlist).AsEnumerable();

                //join mapped products and unmapped produts
                productlist = productlist.Union(restProducts).ToList(); 
            }
            else
            {
                productlist = pList;
            }
           
            return productlist.ToList();


        }


        public async Task Mapbrochure(string BrochureID, string CategoryID,string Products)
        {
            int bID, cID,pID;
            int.TryParse(BrochureID, out bID);
            int.TryParse(CategoryID, out cID);
            List<Product> plist = getProductsByBrochureID_and_CategoryID(BrochureID, CategoryID);
            if(!string.IsNullOrEmpty(Products))
            {
              
                await Task.Run(() =>
                {

                    //seprate product ids
                    var products = Products.Split(',');

                    //take out products which are in products array
                    List<Product> list = plist.Where(c => products.Contains(c.ID.ToString())).ToList();

                    //take out products which are not  in products array
                    List<Product> templist = plist.Where(c => !products.Contains(c.ID.ToString())).ToList();


                    //Products to be deleted
                    List<Product> deleteList = templist.Where(x => x.check == "checked").ToList();


                    //mapped products
                    List<Product> mappedList = plist.Where(x => x.check == "checked").ToList();


                    //products to update in database
                    List<Product> updatelist = list.Except(mappedList).ToList();

                    //add Records in mapping
                    if (updatelist.Count > 0)
                    {
                        MapCategory mp = new MapCategory();
                        foreach (Product p in updatelist)
                        {
                            mp.BrochureID = bID;
                            mp.CategoryID = cID;
                            mp.ProductID = p.ID;
                            db.mappings.Add(mp);
                            db.SaveChanges();
                        }


                        //updatelist.ForEach(m => db.SaveChanges());
                    }

                    //delete Records from mapping
                    if (deleteList.Count > 0)
                    {
                        MapCategory mp = null;
                        foreach (Product p in deleteList)
                        {
                            mp = db.mappings.Single(x => x.BrochureID == bID && x.CategoryID == cID && x.ProductID == p.ID);
                            db.mappings.Remove(mp);
                            db.SaveChanges();
                        }
                    }




                });
            }
            else
            {
                   await Task.Run(() =>
                {
                
                 var products = plist.Where(x => x.check == "checked").ToList();
                 products.ForEach(x => { db.Products.Remove(x);  });

                 db.SaveChanges();
                });
               
            }
           
        }



        public async Task MapbrochureForCategory(string FromBrochureID, string ToBrochureID,string Categories,string Option,string CopytoCategory)
        {
            int bID, TBID,cID,toCID ;
            int.TryParse(FromBrochureID, out bID);
            int.TryParse(ToBrochureID, out TBID);
            int.TryParse(CopytoCategory, out toCID);
           // List<Product> plist = getProductsByBrochureID_and_CategoryID(BrochureID, CategoryID);
                 await Task.Run(() =>
                {
                   // List<Category> categoryList = GetByBrochureID(FromBrochureID).Where(x => x.check == "checked").ToList();
                    if(Option=="2")
                    {
                        if (!string.IsNullOrEmpty(Categories))
                        {

                            //seprate product ids
                            var cats = Categories.Split(',');
                            MapCategory mp = null;
                            List<MapCategory> mplist = null;
                            foreach (string c in cats)
                            {
                                cID = int.Parse(c);
                                mplist = db.mappings.Where(x => x.BrochureID == bID && x.CategoryID == cID).ToList();
                                mplist.ForEach(x => { mp = new MapCategory(); mp.BrochureID = TBID; mp.CategoryID = toCID; mp.ProductID = x.ProductID; db.mappings.Add(mp); db.SaveChanges(); });
                            }

                        }
                    }
                    else if(Option=="1")
                    {
                        if (!string.IsNullOrEmpty(Categories))
                        {

                            //seprate product ids
                            var cats = Categories.Split(',');
                            MapCategory mp = null;
                            List<MapCategory> mplist = null;
                            foreach (string c in cats)
                            {
                                cID = int.Parse(c);
                                mplist = db.mappings.Where(x => x.BrochureID == TBID && x.CategoryID == toCID).ToList();
                                mplist.ForEach(x => { db.mappings.Remove(x); db.SaveChanges(); });

                                mplist = db.mappings.Where(x => x.BrochureID == bID && x.CategoryID == cID).ToList();
                                mplist.ForEach(x => { mp = new MapCategory(); mp.BrochureID = TBID; mp.CategoryID = toCID; mp.ProductID = x.ProductID; db.mappings.Add(mp); db.SaveChanges(); });
                            }

                        }
                    }

          
                });
        }



        public bool checkBrochureID(string ID)
        {
            Brochure brochure= db.Brochures.Where(c => c.BrochureID== ID).SingleOrDefault();
            if (brochure == null)
            {
                return false;
            }
            return true;
        }


        public async Task AddCategoriesToBrochure(int BrochureID,string Categories)
        {
            //split Categories
            await Task.Run(()=>
            {
                string[] CategoryList = Categories.Split(',');


                //already Added Categories            
                var added_categories = db.CategoryToBrochureMap.Where(x => x.BrochureID == BrochureID).ToList();
                int categoryID;
                if (added_categories.Count() > 0)
                {
                    foreach (CategoryMapping cat in added_categories)
                    {
                        if (!CategoryList.Contains(cat.CategoryId.ToString()))
                        {
                            db.CategoryToBrochureMap.Remove(cat);
                            db.SaveChanges();
                        }
                    }
                }


                //add New categories
                CategoryMapping mp = new CategoryMapping();
                foreach (string str in CategoryList)
                {
                    categoryID = int.Parse(str);
                    var category = db.CategoryToBrochureMap.Where(x => x.BrochureID == BrochureID && x.CategoryId == categoryID);
                    if (category.Count() <= 0)
                    {
                        mp.BrochureID = BrochureID;
                        mp.CategoryId = categoryID;
                        db.CategoryToBrochureMap.Add(mp);
                        db.SaveChanges();
                    }
                }
            
            });
           
            
            



            //CategoryMapping mp = new CategoryMapping();
            //string[] CategoryList = Categories.Split(',');
            //foreach (string category in CategoryList)
            //{
            //    mp.BrochureID = BrochureID;
            //    mp.CategoryId = int.Parse(category);
            //    db.CategoryToBrochureMap.Add(mp);
            //    db.SaveChanges();
            //}
        }

        #endregion


        #region Product
        /// <summary>
        /// Methods for Product 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>

        public Product GetProductByID(int ID)
        {

           Product product= db.Products.Find(ID);
            if(product==null)
            {
               return  product = null;
            }

            return product;

        }


        public async Task SaveProduct(Product p)
        {
            await Task.Run(() =>
            {
                db.Products.Add(p);
                db.SaveChanges();
            });
        }


        public bool checkItemNumber(string itemnumber)
        {
            if(itemnumber=="0")
            {
                return false;
            }
            var student = db.Products.Where(x => x.ItemNumber == itemnumber && x.ItemActive == true);
            int count = student.Count();

            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }


        #endregion




        #region Organization



        // Organization 

        public async Task<Organization> organizationGetByID(int ID)
        {
            Organization org = null;
            await Task.Run(() =>
            {

                org = db.Organizations.Where(x=>x.SchoolID==ID).SingleOrDefault();
            });

            return org;
        }

        public async Task<Campaign> GetActiveCampaignByOrganizationId(int ID)
        {
            Campaign camp = null;
         
            await Task.Run(() =>
            {

                var CampaignList = db.Campaigns.Where(c => c.OrganizatonID == ID && c.CampaignEndDate > DateTime.Now);
                camp = CampaignList.FirstOrDefault();
            });
            return camp;
        }

        public bool IsString(object value)
        {
            return value is string;
        }

        public async Task<bool> CheckShoolID(int ID)
        {
            bool result=false;
          await  Task.Run(() =>
            {
                var data = db.Organizations.Where(x => x.SchoolID == ID).ToList();

                if (data.Count > 0)
                {
                    result= true;
                }
                else
                {
                    result=false;
                }
            });

          return result;
            
        }
        #endregion


        #region Student
        
        public string AutoGenerateNumber(int OrgID)
        {

            //first name of School
            //string []initial = Name.Split(' ');


            //generate Random number between 1 to 100000
            Random r = new Random();
            int num = r.Next(100000,999999);

            //concatenate both
            string number = "A"+ num.ToString();


            if (!CheckStudentID(number,OrgID))
            {
                AutoGenerateNumber(OrgID);
            }
            //string studentID = "";
            // var student= db.Students.SingleOrDefault(n => n.StudentID == number);
            //if(student!=null)
            //{
            //    studentID=student.StudentID;
            //}
            //if(!string.IsNullOrEmpty(studentID))
            //{
                
            //}
            //Return the generated number
            return number;        


        }

        public bool CheckStudentID(string ID,int SchoolID)
        {
           // string schoolID=SchoolID.ToString();
            var student = db.Students.FirstOrDefault(n => n.StudentID == ID && n.SchoolID == SchoolID);
            if(student==null)
            {
                return true;
            }
            return false;
        }
        #endregion



        #region MailService

        public string buildEmailBody(string templateName,Dictionary<string,string> options)
        {
            

            System.IO.StreamReader sr = new System.IO.StreamReader(HttpContext.Current.Request.PhysicalApplicationPath + @"EmailTemplates\" + templateName);

            string messageBody = sr.ReadToEnd();
            if(options!=null)
            {
               foreach(KeyValuePair<string,string> opt in options)
               {
                   messageBody=messageBody.Replace(opt.Key, opt.Value);
               }
            }
            

            sr.Close();            
            return messageBody;
        }
        #endregion



        #region Category
        
         public bool CheckCategoryID(string ID)
        {
           Category category=db.Categories.Where(c => c.CategoryID == ID).SingleOrDefault();
             if(category==null)
             {
                 return false;
             }
             return true;

        }

        public async Task<List<Product>>  GetProductsByCategoryID(int ID)
         {
             List<Product> list=null;
             await Task.Run(() =>
             {
                 var plist = db.Products.ToList();
                 plist.ForEach(x => x.check = null);

                 list= plist.Join(db.productToCategoryMap.ToList(), pr => pr.ID, map => map.ProductID, (pr, map) => new { Product = pr, ProductMapping = map }).Where(x => x.ProductMapping.CategoryID == ID).Select(x => x.Product).ToList();

                 if (list.Count > 0)
                 {
                     //mark check which are mapped
                     list.ToList().ForEach(x => x.check = "checked");

                     //Products which doesnot fall under the condition(products which are not mapped) 
                     var restCategories = db.Products.ToList().Except(list).AsEnumerable().OrderBy(x=>x.ItemNumber);

                     //join mapped products and unmapped produts
                     list = list.Union(restCategories).ToList();
                 }
                 else
                 {
                     //all categories
                     list = plist.OrderBy(x=>x.ItemNumber).ToList();
                 }
                
             });

             return list;
         }


        public async Task CopyCategory(int CopyFrom,int CopyTo)
        {



            await Task.Run(() =>
            {
                var category = db.productToCategoryMap.Where(x => x.CategoryID == CopyFrom).ToList();
                foreach (ProductMapping p in category)
                {
                    p.CategoryID = CopyTo;
                    db.productToCategoryMap.Add(p);
                    db.SaveChanges();
                }

            });                                    
        }


        public async Task CopyToNewCategory(int CopyFrom, string CategoryID,string CategoryName)
        {
             await  Task.Run(async () => 
                {
                    Category cat = new Category();
                    cat.CategoryID = CategoryID;
                    cat.CategoryName = CategoryName;
                    db.Categories.Add(cat);
                    db.SaveChanges();
                     await CopyCategory(CopyFrom, cat.ID);
                });
        }


        public async Task<int> AddProductsToCategory(int CategoryID,string products)
        {
            try
            {
                  //split Categories
            await Task.Run(()=>
            {
                string[] productList = products.Split(',');


                //already Added Categories            
                var added_products = db.productToCategoryMap.Where(x => x.CategoryID==CategoryID ).ToList();
                int productID;
                if (added_products.Count() > 0)
                {
                    foreach (ProductMapping cat in added_products)
                    {
                        if (!productList.Contains(cat.ProductID.ToString()))
                        {
                            db.productToCategoryMap.Remove(cat);
                            db.SaveChanges();
                        }
                    }
                }


                //add New categories
                ProductMapping mp = new ProductMapping();
                foreach (string str in productList)
                {
                    productID = int.Parse(str);
                    var category = db.productToCategoryMap.Where(x => x.CategoryID == CategoryID && x.ProductID == productID);
                    if (category.Count() <= 0)
                    {
                        mp.CategoryID = CategoryID;
                        mp.ProductID = productID;
                        db.productToCategoryMap.Add(mp);
                        db.SaveChanges();
                    }
                }
            
            });
            return 0;
            }
            catch
            {
                return -1;
            }
            
        }

        #endregion

        #region Images
         string Image_name = "";
         public async Task<string> SaveImage(string path,string ImageName,int counter=0)
         //public string SaveImage(string path,string ImageName,int counter=0)
         {

         


             await Task.Run(async()=>
                 {
                       string[] files = Directory.GetFiles(path).Select(x => Path.GetFileName(x)).ToArray();


             if (files.Contains(ImageName))
             {
                 string name = ImageName.Substring(0, ImageName.LastIndexOf('.'));
                 string extension = ImageName.Substring(ImageName.LastIndexOf('.'), (ImageName.Length) - ImageName.LastIndexOf('.'));
                 string[] names = null;
                 if (name.IndexOf('_') > 0)
                 {
                     names = name.Split('_');
                     int.TryParse(names[1], out counter);
                     ImageName = names[0] + "_" + (++counter);
                     ImageName += extension;
                 }
                 else
                 {
                     ImageName = name + "_" + counter++;
                     ImageName += extension;
                 }
                 Image_name = ImageName;
                 // await SaveImage(path, ImageName, counter++);
                await SaveImage(path, ImageName, counter++);
             }
             else
             {
                 Image_name = ImageName;
             }
               });


           

             return Image_name;
         }



        public Image ResizeImage(Image image, string path)
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
                    //newImage.Save(path);
                    return newImage;
                }
            }
        }
        #endregion


        public string ReadCookie(string name, HttpContextBase context)
         {
             string value=context.Request.Cookies[name].ToString();

             return value;
         }

        public void CreateCookie(string name,string obj,HttpContextBase context)
        {
            HttpCookie cookie = new HttpCookie(name);
            cookie.Value = obj.ToString();
            context.Response.Cookies.Add(cookie);
        }


        public string  GenerateCouponCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] number = new char[3];
            string code;
            
            Random r = new Random();
            int temp=r.Next(100,999);
            for (int k = 0; k <3;k++)
            {
                number[k] = chars[r.Next(chars.Length)];
            }

            string strnumber = new string(number);
            code =  strnumber+ temp.ToString();

            if(CheckCouponCode(code))
            {
                GenerateCouponCode();
            }

            return code;
        }

        public bool CheckCouponCode(string Code)
        {
            var coupon = db.Coupons.Where(x=>x.Code==Code);
            if(coupon.Count()>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool CheckProductQty(int  ID,int Quantity)
        {

            var product=db.Products.Find(ID);
            if (product != null)
            {
                if (product.Inventory)
                {
                    if (product.InventoryAmount < Quantity)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }

            }
            else
            {
                return false;
            }
        }

        public string WriteToFile(string data,string FileName="")
        {
            try
            {
                DateTime today = DateTime.Today;
                today = today.AddDays(-1);
                string FilePath = HttpContext.Current.Server.MapPath("/Orders");
                
                if(string.IsNullOrEmpty(FileName))
                {
                    FileName = "ft16exp_" + today.Year.ToString() + today.Month.ToString() + today.Day.ToString() + ".txt";
                }
                
                string file = Path.Combine(FilePath, FileName);

                if (File.Exists(file))
                {
                    return "";
                }

                //folderPath = Path.Combine(folderPath, name+".txt");
                FileStream fs = new FileStream(file, FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                if(!string.IsNullOrEmpty(data))
                {                   
                    sw.WriteLine(data);
                }
                                            
                fs.Flush();
                sw.Close();                
                fs.Close();
                return "success";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            
        }

        //public CreateConnection()
        //{

        //}
        public void DownloadFile()
        {
            string orderDetailsStr = null;
            DateTime today=DateTime.Today;            
            today=today.AddDays(-1);
            var orders = db.Orders.Where(x => EntityFunctions.TruncateTime(x.CreatedDate) >= today && EntityFunctions.TruncateTime(x.CreatedDate)<= today).ToList();
          //  var orders = db.Orders.GroupBy((x => EntityFunctions.TruncateTime(x.CreatedDate))).ToList();
          //  string FileName = "";
            if(orders.Count>0)
            {  
                    foreach (Order order in orders)
                    {

                    
                        var Student = db.Students.Find(order.StudentID);
                        var schoool = db.Organizations.Find(Student.SchoolID);
                        var orderDetails = db.OrderDetails.Where(x => x.OrderID == order.ID).ToList();
                        orderDetailsStr = "H" + schoool.ID + " " + Student.FirstName+" "+ Student.LastName+ " " + order.FullName + " #" + order.ID + " " + order.Address1 + " ";
                        orderDetailsStr += order.Address2 + " " + order.City + " " + order.State + " " + order.PostalCode + "-" + order.PhoneNumber + " " + order.EmailAddress;
                        orderDetailsStr += Student.TeacherName + " " + Student.Grade + " " + Student.StudentID + " " + Student.EmailAddress + " ";
                        if (order.ShiptoSchool)
                        {
                            orderDetailsStr += "Y" + Environment.NewLine;
                        }
                        else
                        {
                            orderDetailsStr += "N" + Environment.NewLine;
                        }
                        foreach (OrderDetail orderd in orderDetails)
                        {
                            orderDetailsStr += "D" + orderd.itemNumber + " " + orderd.Quantity + " " + orderd.unitPrice + Environment.NewLine;
                        }

                        string result = ShrdMaster.Instance.WriteToFile(orderDetailsStr); 
                    }
                    
                
            }     
            else
            {
                string result = ShrdMaster.Instance.WriteToFile(orderDetailsStr); 
            }
           // string result = ShrdMaster.Instance.WriteToFile(orderDetailsStr); 
            
            
            
        }



        public void DownloadAllFile()
        {
            string orderDetailsStr = null;
            DateTime today = DateTime.Today;
            today = today.AddDays(-1);
            //var orders = db.Orders.Where(x => EntityFunctions.TruncateTime(x.CreatedDate) >= today && EntityFunctions.TruncateTime(x.CreatedDate)<= today).ToList();
            var orders = db.Orders.GroupBy((x => EntityFunctions.TruncateTime(x.CreatedDate))).ToList();
            string FileName = "";
            if (orders.Count > 0)
            {
                foreach (var group in orders)
                {

                    foreach (Order order in group.ToList())
                    {

                        FileName = "ft16exp_" + order.CreatedDate.Year.ToString() + order.CreatedDate.Month.ToString() + order.CreatedDate.Day.ToString() + ".txt";
                        var Student = db.Students.Find(order.StudentID);
                        var schoool = db.Organizations.Find(Student.SchoolID);
                        var orderDetails = db.OrderDetails.Where(x => x.OrderID == order.ID).ToList();
                        orderDetailsStr = "H" + schoool.ID + " " + Student.FirstName+" "+Student.LastName + " " + order.FullName + " #" + order.ID + " " + order.Address1 + " ";
                        orderDetailsStr += order.Address2 + " " + order.City + " " + order.State + " " + order.PostalCode + "-" + order.PhoneNumber + " " + order.EmailAddress;
                        orderDetailsStr += Student.TeacherName + " " + Student.Grade + " " + Student.StudentID + " " + Student.EmailAddress + " ";
                        if (order.ShiptoSchool)
                        {
                            orderDetailsStr += "Y" + Environment.NewLine;
                        }
                        else
                        {
                            orderDetailsStr += "N" + Environment.NewLine;
                        }
                        foreach (OrderDetail orderd in orderDetails)
                        {
                            orderDetailsStr += "D" + orderd.itemNumber + " " + orderd.Quantity + " " + orderd.unitPrice + Environment.NewLine;
                        }

                        string result = ShrdMaster.Instance.WriteToFile(orderDetailsStr, FileName);
                    }

                }
            }
            // string result = ShrdMaster.Instance.WriteToFile(orderDetailsStr); 



        }

        public void MakeZip()
        {
            ZipFile zip = new ZipFile();
            string FilePath = HttpContext.Current.Server.MapPath("/Orders");
            string Folername = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Day + "-" + DateTime.Now.Month.ToString();
            string path = Path.Combine(FilePath, Folername);
            zip.AddDirectory(path);
            zip.Save(FilePath+"//"+Folername+".zip");
        }



        public Double FreeShippingAmount()
        {
            double amount=0;
            var data=db.ShippingCharges.ToList();
            if(data!=null)
            {
                amount = data[0].FreeAmount;
            }

            return amount;
        }


        public void RemoverOrderDetails(int ID)
        {
            db.Database.ExecuteSqlCommand("delete orderdetails where orderID="+ID);
            db.SaveChanges();
        }
    }
}