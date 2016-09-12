using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;
using System.Xml;
using FundRaising.App_Start;
using FundRaising.Models;
using FundRaising.ViewModels;
using iTransact;
using iTransact.Requests;
using iTransact.Responses;
using Microsoft.AspNet.Identity;
using SelectPdf;

namespace FundRaising.Controllers.Client
{
    public class CustomerController : Controller
    {

        FundRaisingDBContext db = new FundRaisingDBContext();
       // ShoppingCart cart = new ShoppingCart();
        //
        // GET: /Customer/
       

        public ActionResult Index(string ID="0")                        
       
     {
            try
            {
                if(ID=="0" && Session["studentID"]==null)
                {
                    return RedirectToAction("select","Customer");
                   // return;
                    //RedirectToActionPermanent("Select","Customer");
                }
                else
                {
                    if(ID=="0")
                    {
                        if (Session["StudentID"] != null)
                        {
                            ID = Session["StudentID"].ToString();
                        }
                    }
                    
                    //string sID=ID.ToString();
                    Student student = db.Students.Where(x=>x.StudentID==ID).FirstOrDefault();
                    if (student != null)
                    {
                        if (student.StudentID == "0000007")
                        {
                            HttpCookie imgcookie = new HttpCookie("ImageCookie");
                            imgcookie.Value = "/images/coupon.jpg";
                            Response.Cookies.Add(imgcookie);
                            Session["studentID"] = ID;
                            Session["student"] = student;
                            Session["BrochureID"] = "007";
                        }
                        else
                        {
                            int OrgID = student.SchoolID;
                            Organization org = db.Organizations.Where(x=>x.SchoolID==OrgID).FirstOrDefault();
                            var camp = db.Campaigns.Where(x => x.OrganizatonID == org.SchoolID).FirstOrDefault();
                            //HttpCookie imgcookie = new HttpCookie("ImageCookie");
                            RegisterModel reg = db.Distributors.Find(org.Distributor);
                            if (reg.ImagePath != null)
                            {
                                ShrdMaster.Instance.CreateCookie("ImageCookie", reg.ImagePath, this.HttpContext);
                            }

                            //imgcookie.Value = reg.ImagePath;
                            //Response.Cookies.Add(imgcookie);
                            if (camp == null || camp.CampaignEndDate < DateTime.Now)
                            {
                                ViewBag.School = org.Name;
                                ViewBag.Date = camp.CampaignEndDate.ToShortDateString();
                                return View("Finish");
                            }
                            else
                            {
                                Session["studentID"] = ID;
                                //Session["CartId"] = ID;
                                Session["student"] = student;
                            }



                            Session["Organization"] = org;
                            Session["SchoolID"] = org.SchoolID;
                            Session["BrochureID"] = org.Catalog;
                            ViewBag.BrochureID = Session["BrochureID"].ToString();
                            ViewBag.Message = org.WelcomeMessage;
                        }

                        string Id = ID.ToString();
                        ShrdMaster.Instance.CreateCookie("StudentID", Id, this.HttpContext);
                        //HttpCookie studentIDCookie = new HttpCookie("StudentID");
                        //studentIDCookie.Value = ID.ToString();
                        //studentIDCookie.Expires.AddDays(10);
                        //Response.Cookies.Add(studentIDCookie);


                        if (Session["BrochureID"] != null)
                        {
                            ViewBag.BrochureID = Session["BrochureID"].ToString();
                        }
                        else
                        {
                            return RedirectPermanent("/");
                        }

                        //   Brochure brochure=null;
                        //if(org.ShipToSchool)
                        //{           
                        //    ViewBag.BrochureID = org.ShipToSchoolCatalog;
                        //    Session["BrochureID"] = ViewBag.BrochureID;
                        //}


                        ViewBag.studentID = ID;
                        return View(student);
                    }
                    else
                    {
                        string Message = "Student Id not found.";

                        return RedirectToAction("ErrorView", "Customer", new { message = Message });
                    }
                }
                
            }
            catch(Exception ex)
            {
                //ViewBag.Error = ex.Message;
                return RedirectToAction("ErrorView", "Customer", new { message = ex.Message });
            }
           
           
        }


        public ActionResult Select()
        {
            return View();
        }
        

        public ActionResult CheckOut(int option=0)
        {
            try
            {
                ShoppingCart shop = new ShoppingCart();
                Organization org = null;
                Order order = null;
                string studentID;
                if (Session["BrochureID"] != null)
                {
                    ViewBag.BrochureID = Session["BrochureID"].ToString();
                }
                else
                {
                    return RedirectPermanent("/");
                }

                if (Session["studentID"] != null)
                {
                    ViewBag.studentID = Session["studentID"].ToString();
                    //  studentID=Session["studentID"].ToString();;
                }
                else
                {
                    studentID = ShrdMaster.Instance.ReadCookie("StudentID", this.HttpContext);
                    ViewBag.studentID = studentID;
                    Session["studentID"] = studentID;
                }


                var cart = ShoppingCart.GetCart(this.HttpContext);
                if (shop.CheckShipToSchool(cart))
                {
                    ViewBag.ShipToSchool =true;
                }
                else
                {
                    ViewBag.ShipToSchool = false;
                }

                if (Session["Order"] != null)
                {
                    order = Session["Order"] as Order;

                }
                else
                {
                    order = new Order();
                }
                                                                                

                if (shop.CheckShipToSchool(cart))
                {
                   
                    
                    org = Session["Organization"] as Organization;
                    order.SAddress1= org.Address1;
                    order.SAddress2 = org.Address2;
                    order.SCity= org.City;
                    order.SState = org.State;
                    order.SCountry = org.Country;
                    order.SPostalCode = org.Postal;
                    //return View(order);
                }

               
                if(order!=null)
                {
                    ViewBag.SState = new SelectList(Common.GetStates(), "ID", "Text",order.SState);
                    ViewBag.SCountry = new SelectList(Common.GetCountries(), "ID", "Text", order.SCountry);
                    return View(order);
                }
                else
                {
                    ViewBag.SState = new SelectList(Common.GetStates(), "ID", "Text");
                    ViewBag.SCountry = new SelectList(Common.GetCountries(), "ID", "Text");
                    return View();
                }
                
            }
            catch(Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return RedirectToAction("ErrorView", "Customer", new { message=ex.Message});
            }
           
        }


        [HttpPost]        
        public ActionResult CheckOut(Order order)
        {
            try
            {
                int studentID = -1;
                string sID;
                bool shipToSchool = false;
                //int stuID;
                ShoppingCart shop = new ShoppingCart();
                var cart = ShoppingCart.GetCart(this.HttpContext);
                if (shop.CheckShipToSchool(cart))
                {
                    ViewBag.ShipToSchool = true;
                    var org = Session["Organization"] as Organization;
                    if(org!=null)
                    {
                        order.SAddress1 = org.Address1;
                        order.SAddress2 = org.Address2;
                        order.SCity = org.City;
                        order.SState = org.State;
                        order.SCountry = org.Country;
                        order.SPostalCode = org.Postal;
                    }
                    else
                    {
                        return RedirectPermanent("/");
                    }
                   
                    shipToSchool = true;
                }
                if (ModelState.IsValid)
                {
                    if (Session["studentID"] != null)
                    {
                        //int.TryParse(Session["studentID"].ToString(), out studentID);

                        sID = Session["studentID"].ToString();
                        ViewBag.studentID = sID;
                    }
                    else
                    {
                        return RedirectPermanent("/");
                    }

                     if (Session["BrochureID"] != null)
                    {
                        ViewBag.BrochureID = Session["BrochureID"].ToString();
                    }
                    else
                    {
                        return RedirectPermanent("/");
                    }

                
                   // int.TryParse(sID, out studentID);

                    //var student = db.Students.Find(studentID);
                    var student = db.Students.Where(x => x.StudentID == sID).SingleOrDefault();
                    int orgID = student.SchoolID;

                    //var org = db.Organizations.Find(orgID);
                    var org = db.Organizations.Where(x => x.SchoolID == orgID).SingleOrDefault();
                   
                    if (string.IsNullOrEmpty(order.SCountry) || order.SCountry.ToUpper()=="SELECT--")
                    {


                        if (shipToSchool)
                        {


                            order.SAddress1 = org.Address1;
                            order.SCountry = org.Country;
                            order.SAddress2 = org.Address2;
                            order.SCity = org.City;
                            order.SState = org.State;
                            order.SPostalCode = org.Postal;
                            order.ShiptoSchool = true;
                        }
                        else
                        {
                            order.SCompanyName = order.CompanyName;
                            order.SCountry = order.Country;
                            order.SAddress1 = order.Address1;
                            order.SAddress2 = order.Address2;
                            order.SCity = order.City;                            
                            order.SState = order.State;
                            order.SPostalCode = order.PostalCode;
                        }


                    }
                    //if (order.SCountry.ToUpper() != "UNITED STATES") || order.SCountry.ToUpper() != "US" || order.SCountry.ToUpper() != "USA")
                    //{
                    //   // ViewBag.BrochureID = Session["BrochureID"].ToString();
                    //    ViewBag.Error = "Shipping Address Should with in United States only";
                    //    return View(order);
                    //}

                    //if (order.SCountry.ToUpper() != "US" || order.SCountry.ToUpper() != "USA")
                    //{
                    //   // ViewBag.BrochureID = Session["BrochureID"].ToString();
                    //    ViewBag.Error = "Shipping Address Should with in United States only";
                    //    return View(order);
                    //}

                    //if (order.SCountry.ToUpper() != "UNITED STATES") || order.SCountry.ToUpper() != "US" || order.SCountry.ToUpper() != "USA")
                    //{
                    //   // ViewBag.BrochureID = Session["BrochureID"].ToString();
                    //    ViewBag.Error = "Shipping Address Should with in United States only";
                    //    return View(order);
                    //}
                    order.StudentID = sID;
                    //  Organization org=null;
                    //if(Session["Organization"]!=null)
                    //{
                    //    org = Session["Organization"] as Organization;
                    //}
                    if (org != null)
                    {
                        order.SchoolID = org.ID;
                    }
                    //  order.Status = "Order Received";
                    order.CreatedDate = DateTime.Now;
                    Session["Order"] = order;
                    //   db.Orders.Add(order);
                    // db.SaveChanges();                
                    return RedirectToAction("complete", new { ID = order.ID });
                    //return('');
                }
                ViewBag.SState = new SelectList(Common.GetStates(), "ID", "Text",order.SState);
                ViewBag.SCountry = new SelectList(Common.GetCountries(), "ID", "Text", order.SCountry);
                if (Session["BrochureID"] != null)
                {
                    ViewBag.BrochureID = Session["BrochureID"].ToString();
                }
                else
                {
                    HttpCookie cook = new HttpCookie("StudentID");
                    return RedirectToAction("Index", new { ID = cook.Value });
                }

               

                return View(order);
            }
            catch(Exception ex)
            {
                //ViewBag.Error = ex.Message;
                return RedirectToAction("ErrorView", "Customer", new { message = ex.Message });
                //return View("ErrorView");
            }
            
        }


        public ActionResult Complete(int ID)
        {

            try
            {
                if (Session["BrochureID"] != null)
                {
                    ViewBag.BrochureID = Session["BrochureID"].ToString();
                }
                else
                {
                    return RedirectPermanent("/");
                }

                if (Session["studentID"] != null)
                {
                    ViewBag.studentID = Session["studentID"].ToString();
                }
               
                //Organization org=null;
                //if (Session["Organization"] != null)
                //{
                //    org = Session["Organization"] as Organization;
                //}


                if (Session["Order"] == null)
                {
                    return RedirectToActionPermanent("Index", new { ID = ViewBag.studentID });
                }

                // Validate customer owns this order
                var order = Session["Order"] as Order;

                if (order != null)
                {
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    ViewBag.OrderList = cart.GetCartItems();
                    double total = cart.GetTotal();
                    double shippingamt = 0,freeShippingAmount=0;
                    ShippingCharge shipping = null;
                    bool ISShipToSchool = cart.IsShipToSchool();
                    if (!ISShipToSchool)
                    {

                        shipping = db.ShippingCharges.Where(x => total >= x.LowerLimit && total <= x.UpperLimit).SingleOrDefault();
                        if(shipping!=null)
                        {
                            shippingamt = shipping.Charge;
                            freeShippingAmount = shipping.FreeAmount;
                        }

                      

                        //foreach (ShippingCharge charge in shipping)
                        //{
                        //    if (total >= charge.LowerLimit && total <= charge.UpperLimit)
                        //    {
                        //        shippingamt = charge.Charge;
                        //        freeShippingAmount = charge.FreeAmount;
                        //        break;
                        //    }
                        //}
                    }
                    else
                    {
                        var org = Session["Organization"] as Organization;
                        if (org != null)
                        {
                            ViewBag.SchoolName = org.Name;
                        }
                    }
                    

                    //if (Session["Organization"] != null)
                    //{
                    //    var org = Session["Organization"] as Organization;
                    //    if (org.FreeShippingAmount)
                    //    {
                    //        if (total >= freeShippingAmount)
                    //        {
                    //            shippingamt = 0;
                    //        }
                    //    }
                    //}
                    ViewBag.ShippingAmount = shippingamt;

                    SalesTaxCharge SalesTaxCharge = db.SalesTaxCharges.Where(x => x.Active == true).SingleOrDefault();
                    //ViewBag.SalesTaxCharge = SalesTaxCharge.TaxAmount.ToString();
                    //ViewBag.State = SalesTaxCharge.State.ToString();
                    //string shippingState = order.SState.ToUpper();
                    if (ShrdMaster.Instance.CheckState(order.SState,SalesTaxCharge.State))
                    {
                        //SalesTaxCharge = db.SalesTaxCharges.Where(x => x.Active == true).SingleOrDefault();
                        ViewBag.SalesTaxCharge = SalesTaxCharge.TaxAmount.ToString();
                        ViewBag.State = SalesTaxCharge.State.ToString();
                        ViewBag.IsTaxChargeable = "true";

                    }
                    else
                    {
                        ViewBag.SalesTaxCharge = SalesTaxCharge.TaxAmount.ToString();
                        ViewBag.State = SalesTaxCharge.State.ToString();
                        ViewBag.IsTaxChargeable = "false";
                        // ViewBag.SalesTaxCharge = "N/A";
                    }
                   
                  
                    
                    HttpCookie orderCookie = new HttpCookie("orderCookie");
                    orderCookie.Value = order.ID.ToString();
                    Response.Cookies.Add(orderCookie);
                    order.FullName = order.FirstName + " " + order.LastName;
                    return View(order);

                }
                else
                {
                    return RedirectToAction("ErrorView", "Customer", new { message = "some error has occured." });
                }
                //return View()
            }
            catch(Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return RedirectToAction("ErrorView", "Customer", new { message = ex.Message });
            }
           
        }



        public int Transaction()
        {
            Random r = new Random();

            return r.Next();
        }

        [HttpPost]
        public async Task<ActionResult> Complete(FormCollection fc)
        {
            try
            {
                Order order = null;

                if (Session["BrochureID"] != null)
                {
                    ViewBag.BrochureID = Session["BrochureID"].ToString();
                }
                else
                {
                    return RedirectPermanent("/");
                }

                if (Session["studentID"] != null)
                {
                    ViewBag.studentID = Session["studentID"].ToString();
                }
                int orderID = -1;
                if (Request.Cookies["orderCookie"] != null)
                {
                    int.TryParse(Request.Cookies["orderCookie"].Value.ToString(), out orderID);
                }
                else
                {
                    return View("Index", "Customer", new { ID = ViewBag.studentID });
                }

                if (Session["Order"] == null)
                {
                    return RedirectToActionPermanent("Index", new { ID = ViewBag.studentID });
                }
               // var cart = ShoppingCart.GetCart(this.HttpContext);
                order = Session["Order"] as Order;
                //db.Orders.Add(order);
                //db.SaveChanges();


                if (order != null)
                {
                    var cart = ShoppingCart.GetCart(this.HttpContext);

                    var total = cart.GetTotal();
                    if (fc["CardType"] != null)
                    {
                        order.CardType = fc["CardType"].ToString();
                    }
                    if (fc["CardNumber"] != null)
                    {
                        order.CardNumber = fc["CardNumber"].ToString();
                    }
                    if (fc["CardName"] != null)
                    {
                        order.CardName = fc["CardName"].ToString();
                    }
                    if (fc["ExpirationDate"] != null)
                    {
                        order.ExpirationDate = fc["ExpirationDate"].ToString();
                    }
                    if (fc["ExpirationYear"] != null)
                    {
                        order.ExpirationYear = fc["ExpirationYear"].ToString();
                    }
                    if (fc["CVVNumber"] != null)
                    {
                        order.CVVNumber = fc["CVVNumber"].ToString();
                    }

                    OrderItem orderitem;
                    List<OrderItem> orderarr = new List<OrderItem>();
                    bool shiptoschool = false;  //OrderItem[] orderarr = new OrderItem[cart.GetCount()];
                    //int count = 0;
                    OrderItem shipping = new OrderItem();
                    var CartItems=cart.GetCartItems();

                    foreach (Cart c in CartItems)
                    {
                        orderitem = new OrderItem();
                        //if (c.chargeShipping)
                        //{

                        //}
                        if (c.ShipToSchool)
                        {
                            shiptoschool = true;
                        }
                        orderitem.Cost = Convert.ToDecimal(c.Price);
                        orderitem.Description = c.Description;
                        orderitem.Quantity = c.Quantity;
                        if (!ShrdMaster.Instance.CheckProductQty(c.productId, c.Quantity))
                        {
                            string Message = "Product with name " + c.Description + " is out of stock.please wait for availability.";
                            return RedirectToAction("ErrorView", "Customer", new { message = Message });
                        }
                        orderarr.Add(orderitem);
                        // orderarr[count] = orderitem;
                        //  count++;
                    }
                    var cartTotal = cart.GetTotal();

                    decimal shippingcharge;
                    double freeShippingAmount = 0;
                    if (!shiptoschool)
                    {
                        //var charges = db.ShippingCharges.Where(x => x.LowerLimit >= cartTotal && x.UpperLimit <= cartTotal).SingleOrDefault();
                        var charges = db.ShippingCharges.Where(x => cartTotal >= x.LowerLimit && cartTotal <= x.UpperLimit).SingleOrDefault();
                        if (charges != null)
                        {
                            freeShippingAmount = charges.FreeAmount;
                            if (Session["Organization"] != null)
                            {
                                var org = Session["Organization"] as Organization;
                                if (org.FreeShippingAmount)
                                {
                                    if (total >= freeShippingAmount)
                                    {
                                        shippingcharge = 0;
                                    }
                                }
                                else
                                {
                                    shipping.Description = "Shipping";
                                    decimal.TryParse(charges.Charge.ToString(), out shippingcharge);
                                    shipping.Cost += shippingcharge;
                                    shipping.Quantity = 1;
                                    orderarr.Add(shipping);
                                }
                            }

                        }
                    }

                    var SalesTax=db.SalesTaxCharges.Where(x=>x.Active==true).SingleOrDefault();
                    if(SalesTax!=null)
                    {
                            if(ShrdMaster.Instance.CheckState(order.SState,SalesTax.State))
                            {
                                shipping = new OrderItem();
                                decimal amount;
                                decimal.TryParse(SalesTax.TaxAmount.ToString(),out amount);
                                shipping.Cost += amount;
                                shipping.Description = "SalesTax";
                                shipping.Quantity = 1;
                                orderarr.Add(shipping);
                            }
                    }
                   





                    //if (shipping != null)
                    //{
                    //    if (shipping.Cost > 0)
                    //    {
                    //        orderarr.Add(shipping);
                    //    }

                    //}



                    var chargeRequest = new ChargeRequest()
                    {
                        iTransactApiKey = ConfigurationManager.AppSettings["iTransactApiKey"],
                        iTransactGateway = ConfigurationManager.AppSettings["iTransactTargetGateway"],
                        iTransactUserName = ConfigurationManager.AppSettings["iTransactUsername"],
                        BillingAddress = order.Address1,
                        BillingCity = order.City,
                        BillingState = order.State,
                        BillingCountry = "US",
                        BillingFirstName = order.FirstName,
                        BillingLastName = order.LastName,
                        BillingPhone = order.PhoneNumber,
                        BillingZip = order.PostalCode,
                        Email = order.EmailAddress,

                        // Optional
                        ShippingAddress = order.SAddress1,
                        ShippingCity = order.SCity,
                        ShippingState = order.SState,
                        ShippingCountry = "US",
                        ShippingFirstName = order.FirstName,
                        ShippingLastName = order.LastName,
                        ShippingPhone = order.PhoneNumber,
                        ShippingZip = order.SPostalCode,
                        CreditCardNumber = order.CardNumber,
                        Cvv = order.CVVNumber,
                        ExpirationMonth = DateTime.Now.AddMonths(1).Month > 10 ? DateTime.Now.AddMonths(1).Month.ToString() : "0" + DateTime.Now.AddMonths(1).Month,
                        ExpirationYear = DateTime.Now.Year.ToString(),
                        //ExpirationMonth = Convert.ToInt32(order.ExpirationDate) > 10 ? order.ExpirationDate : "0" + order.ExpirationDate,
                        //ExpirationYear = order.ExpirationYear,

                        OrderItems = orderarr.ToArray(),
                        //CustomerId = order.ID.ToString()
                    };

                    var response = PaymentProcessor.ChargeCreditCard(chargeRequest);

                    if (response.Status == TransactionStatus.Ok)
                    {

                        Logger.Instance.Log("Entered in ok part......1");
                        db.Orders.Add(order);
                        db.SaveChanges();
                        Logger.Instance.Log("Entered in ok part......1.1");
                        cart.createOrder(order);
                        Logger.Instance.Log("Entered in ok part......1.2");
                        if (Request.Cookies["Coupon"] != null)
                        {
                            string code = "";
                            code = Request.Cookies["Coupon"].Value;
                            var coupon = db.Coupons.Where(cp => cp.Code == code).SingleOrDefault();

                            if (coupon.CouponUsage == 2)
                            {
                                coupon.Active = false;
                                db.Entry(coupon).State = EntityState.Modified;
                                db.SaveChanges();
                            }

                            //Adding values to coupon User tbale
                            CouponUser cpnUser = new CouponUser();
                            cpnUser.FirstName = order.FirstName;
                            cpnUser.LastName = order.LastName;
                            cpnUser.EmailID = order.EmailAddress;
                            cpnUser.Couponcode = coupon.Code;
                            db.CouponUsers.Add(cpnUser);
                            db.SaveChanges();
                        }




                        Logger.Instance.Log("Entered in ok part......2");
                        /// convert to PDF
                        HtmlToPdf Convertor = new HtmlToPdf();
                        Logger.Instance.Log("Entered in ok part......2.1");
                        //// create a new pdf document converting an url
                      //  string url = "http://localhost:51369/Customer/InvoicePrint?orderID=" + order.ID + "&PDF=1&cartID=" + cart.GetCartId(this.HttpContext);
                      string url = "http://fundraising.infodatixhosting.com/Customer/InvoicePrint?orderID=" + order.ID + "&PDF=1&cartID=" + cart.GetCartId(this.HttpContext);

                        Logger.Instance.Log("Entered in ok part......2.3");

                        Logger.Instance.Log("Entered in ok part......3");
                        string htmlCode;
                        using (WebClient client = new WebClient())
                        {

                            // Download as a string.
                            Logger.Instance.Log("Entered in ok part......3.1");
                            htmlCode = client.DownloadString(url);
                            Logger.Instance.Log("Entered in ok part......4");
                        }

                        Logger.Instance.Log("Entered in ok part......5");


                        // PdfPageSize pdfpagesize = PdfPageSize.B5;
                        //  Convertor.Options.PdfPageSize = pdfpagesize;
                        Convertor.Options.MarginTop = 50;
                        Convertor.Options.MarginBottom = 50;
                        // Convertor.Options.WebPageHeight = 1000;
                        PdfDocument doc = Convertor.ConvertHtmlString(htmlCode);
                        string path = Server.MapPath("/PDF//");
                        path += order.ID + "_Receipt.Pdf";
                        //HtmlToPdf opt = new HtmlToPdf();
                        //HtmlToPdfOptions options = new HtmlToPdfOptions();
                        //options.MarginTop = 2;

                        // PdfMargins margin=new PdfMargins (1.5f);
                        //doc.Margins = margin;
                        doc.Save(path);
                        Logger.Instance.Log("Entered in ok part......1");
                        // close pdf document
                        doc.Close();
                        //sending mail 
                        var student = db.Students.Where(s => s.StudentID == order.StudentID).SingleOrDefault();
                        var org = db.Organizations.Find(order.SchoolID);
                        EmailService email = new EmailService(path);
                        IdentityMessage details = new IdentityMessage();
                        details.Destination = order.EmailAddress;
                        details.Subject = "Receipt! Fundraisingshop.com";
                        Dictionary<string, string> param = new Dictionary<string, string>();
                        if (org != null)
                        {
                            param.Add("<%Student%>", student.FirstName + " " + student.LastName);
                            param.Add("<%School%>", org.Name);
                        }
                        else
                        {
                            param.Add("<%Student%>", " ");
                            param.Add("<%School%>", " ");
                        }

                        param.Add("<%customer%>", order.FullName);
                        details.Body = ShrdMaster.Instance.buildEmailBody("InvoiceEmailTemplate.txt", param);
                        string attachment = path;
                        await email.SendAsync(details);
                        Logger.Instance.Log("Entered in ok part......6");
                        cart.EmptyCart();
                        return View("Complete1");
                        //System.Console.WriteLine("Transaction OK");
                        //System.Console.WriteLine("Transaction Id:  " + response.TransactionId);
                    }
                    else
                    {
                        db.Orders.Remove(order);
                        db.SaveChanges();
                        ShrdMaster.Instance.RemoverOrderDetails(order.ID);
                        string Message = "<span><h3>Transaction declined.</h3></span> <br/> <b>" + response.ErrorMessage + "</b>";
                        //"<span><h3>Transaction declined.</h3></span> <br/> <b>In pattern AccountInfo: AccountNumber must be between 13 and 16 digits </b>"
                        return RedirectToAction("ErrorView", "Customer", new { option = 1, message = Message });

                        //var cartITems = cart.GetCartItems();
                        //foreach (Cart c in cartITems)
                        //{
                        //    var product = db.Products.Find(c.productId);
                        //    product.InventoryAmount += c.Quantity;
                        //    db.Entry(product).State = EntityState.Modified;
                        //    db.SaveChanges();
                        //}


                        //System.Console.WriteLine("ERROR!!!!");
                        //System.Console.WriteLine("AVS Error:  " + response.Avs);
                        //System.Console.WriteLine("Error Message:  " + response.ErrorMessage);
                        //System.Console.WriteLine("Xml:  " + response.RawXml);
                    }





                    //if (response.IsSuccessStatusCode)
                    //{




                    //}
                    //else
                    //{

                    //}



                    ////doc.SelectSingleNode("//AccountInfo//CardAccount//TrackData").InnerText = "TRACK DATA";
                    ////doc.SelectSingleNode("//AccountInfo//CardAccount//Ksn").InnerText = "12345";
                    ////doc.SelectSingleNode("//AccountInfo//CardAccount//Pin").InnerText = "1234";




                    ////XmlTextReader xmlFile = new XmlTextReader(xmlUrl);
                    ////XmlTextWriter write = new XmlTextWriter (xmlUrl,null);
                    ////write.WriteElementString("Email", order.EmailAddress);
                    ////write.WriteElementString("CustId", XmlConvert.ToString(order.ID));
                    ////write.WriteElementString("Address1", order.Address1);
                    ////write.WriteElementString("Address2", order.Address2);
                    ////string file;
                    ////while(xmlFile.Read())
                    ////{
                    ////    file+=xmlFile.reade
                    ////}

                    //if (Transaction() % 2 == 0)
                    //{
                    //    //getting cart information to update in orders



                    //}
                    //else
                    //{


                    //}
                }

                return RedirectToAction("ErrorView", "Customer", new { message = "some error has occured." });
                //return RedirectToAction("InvoicePrint", "Customer", new { orderID=orderID,PDF=1});
                //return View("Index", "Customer", new { ID=order.StudentID});
            }
            catch(Exception ex)
            {
              //  ViewBag.Error = ex.Message;
                return RedirectToAction("ErrorView", "Customer", new { message = ex.Message });
            }
            
        }

        [AllowAnonymous]
        public ActionResult InvoicePrint (int orderID,int PDF=0)
        {
            Order order = db.Orders.Find(orderID);            
            Organization org = null;
            OrderSummary ordersummary = null;

            if(order!=null)
            {
                var orderdetails = db.OrderDetails.Where(x => x.OrderID == orderID).ToList();

                order.OrderList = orderdetails.Join(
                        db.Products.ToList(), od => od.ProductID, p => p.ID,
                        (od, p) => new { OrderDetail = od, Product = p })
                        .Select(x => new Cart
                        {
                            ID = x.OrderDetail.ID,
                            productId=x.Product.ID,
                            Description = x.Product.Description,
                            itemNumber = x.Product.ItemNumber,
                            Price = x.OrderDetail.unitPrice,
                            Quantity = x.OrderDetail.Quantity,
                            chargeShipping = x.OrderDetail.ChargeShipping,
                            chargeSalesTax = x.OrderDetail.ChargeSalesTax
                        }).ToList();
                
                if (order.SchoolID != 0)
                {
                    var student = db.Students.Where(x => x.StudentID == order.StudentID).SingleOrDefault();
                    org = db.Organizations.Find(order.SchoolID);
                    var distributor = db.Distributors.Find(org.Distributor);
                    ViewBag.Distributor = distributor.UserName;
                    ViewBag.Organization = org;
                    ViewBag.School = org.Name;
                    ViewBag.Student = student.FirstName + " " + student.LastName;

                }


                //ViewBag.FreeShippingAmount = ShrdMaster.Instance.FreeShippingAmount();

                ordersummary = db.OrderSummaries.Where(x => x.OrderID == orderID).SingleOrDefault();

                if(ordersummary!=null)
                {
                    var SalesTaxCharge = db.SalesTaxCharges.Where(x => x.Active == true).SingleOrDefault();
                    ViewBag.SalesTaxCharge = SalesTaxCharge.TaxAmount.ToString();
                    ViewBag.State = SalesTaxCharge.State.ToString();                    
                   
                }
                order.OSummary = ordersummary;


                if(PDF==1)
                {
                    return View("InvoiceEditPDF", order);
                
                }
                return View(order);
                

            }
            if(PDF==1)
            {
                return RedirectToAction("ErrorView", "Customer", new { message = "some error has occured." });
            }


            ViewBag.Error = "something went wrong";
            return RedirectToAction("Error");
        }
        public ActionResult SelectBrochure()
        {

            int studentID = -1;
            if (Session["studentID"] != null)
            {
                int.TryParse(Session["studentID"].ToString(), out studentID);
                Student student = db.Students.Find(studentID);
                //int OrgID = int.Parse();
                Organization org = db.Organizations.Find(student.SchoolID);
                //   Brochure brochure=null;

                ViewBag.catalog1 = db.Brochures.Find(org.Catalog);
                ViewBag.Catalog2 = db.Brochures.Find(org.ShipToSchoolCatalog); 
            }
            else
            {
                return RedirectPermanent("/");
            }
            

           
            //if (org.ShipToSchool)
            //{
            //    ViewBag.BrochureID = org.ShipToSchoolCatalog;
            //    Session["BrochureID"] = ViewBag.BrochureID;
            //}
            //Session["studentID"] = ID;
            //ViewBag.studentID = ID; 

            return PartialView("_selectBrochure");

        }



        //[HttpPost]
        public ActionResult SelectBrochureP(int BrochureID)
        {
           

            //if (org.ShipToSchool)
            //{
            //    ViewBag.BrochureID = org.ShipToSchoolCatalog;
            //    Session["BrochureID"] = ViewBag.BrochureID;
            //}
           // Session["studentID"] = ID;
            //ViewBag.studentID = ID;
           int studentID=-1;
           Session["BrochureID"] = BrochureID;
            if(Session["studentID"]!=null)
            {
                 int.TryParse(Session["studentID"].ToString(),out studentID); 
            }
            else
            {
                return RedirectPermanent("/");
            }
            return RedirectToAction("Index", "Customer", new { Id=studentID});

        }


        public ActionResult Coupon()
        {           
         //   return View("Index");
            return RedirectToActionPermanent("Index", new { id = 50000 });
        }


        public ActionResult ApplyCoupon(string code,string name,string EmailAddress)
        {
            
                string[] arrname = name.Split('-');

                string firstname, lastname;
                firstname = Regex.Replace(arrname[0], @"\s+", string.Empty);
                lastname = Regex.Replace(arrname[1], @"\s+", string.Empty);
                Coupon coupon = db.Coupons.Where(x => x.Code == code && x.EndDate >= DateTime.Now).SingleOrDefault();
                if (coupon != null)
                {
                    if (coupon.Active == false)
                    {
                        return Json("-1", JsonRequestBehavior.AllowGet);
                    }
                    else if (coupon.CouponUsage == 1)
                    {
                        var list = db.CouponUsers.Where(x => x.Couponcode == code && x.EmailID == EmailAddress && x.FirstName == firstname && x.LastName == lastname);
                        if (list.Count() > 0)
                        {
                            return Json("-1", JsonRequestBehavior.AllowGet);
                        }
                    }

                    HttpCookie couponCodeCookie = new HttpCookie("Coupon");
                    couponCodeCookie.Value = coupon.Code;
                    Response.Cookies.Add(couponCodeCookie);
                    return Json(coupon.value, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                     
           
            
           
        }

        [ValidateInput(false)]
        public ActionResult ErrorView(int option=0,string message="")
        {
         if(option==1)
         {
             ViewBag.Transaction = "true";           
         }
         ViewBag.Message = message;
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult OrderInformation()
        {
            
            return View();
        }
    }
}
