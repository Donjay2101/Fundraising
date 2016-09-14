using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FundRaising.App_Start;
using FundRaising.Filters;
using FundRaising.Models;
using FundRaising.ViewModels;
using Microsoft.AspNet.Identity;
using WebMatrix.WebData;

namespace FundRaising.Controllers.Admin
{
    [Authorize(Users = "Admin")]
    [InitializeSimpleMembership]
    public class DistributorController : Controller
    {
        private FundRaisingDBContext db = new FundRaisingDBContext();

        //
        // GET: /Distributor/


        #region Students
        public ActionResult Students()
        {

            var students = db.Students.Join(db.Organizations.Where(x=>x.IsActive==true).AsEnumerable(), stu => stu.SchoolID, org => org.SchoolID, (stu, org) => new { Student = stu, Organization = org })
                    .Select(both => new StudentViewModel
                    {
                        ID = both.Student.ID,
                        StudentID = both.Student.StudentID,
                        FirstName= both.Student.FirstName,
                        LastName= both.Student.LastName,
                        EmailAddress = both.Student.EmailAddress,
                        SchoolName = both.Organization.Name,
                        SchoolID=both.Organization.SchoolID,
                        IsActive=both.Student.Active
                    });

            return View(students.ToList());
        }

        public ActionResult StudentDetails(int id = -1)
        {
            
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            return View(student);
        }

        public async Task<JsonResult> ReSendMail(int ID)
        {
            var student = db.Students.Find(ID);
            EmailService email = new EmailService();
            IdentityMessage details = new IdentityMessage();
            details.Destination = student.EmailAddress;
            details.Subject = "Welcome Mail! Fundraisingshop.com";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("<%ID%>", student.ID.ToString());
            param.Add("<%UserID%>", student.StudentID);
            param.Add("<%password%>", student.Password);
            details.Body = ShrdMaster.Instance.buildEmailBody("WelcomeMessage.txt", param);
            await email.SendAsync(details);

            return Json("Sent", JsonRequestBehavior.AllowGet);

        }


        public ActionResult Processing(int ID,int Option)
        {

            var student = db.Students.Find(ID);
            if(student==null)
            {
                return HttpNotFound();
            }
            if(Option==0)
            {
                student.Active = false;
            }
            else
            {
                student.Active = true;
            }            
            db.SaveChanges();
            return Json("done",JsonRequestBehavior.AllowGet);
        }


        public ActionResult StudentOrders(string  ID="")
        {

            var orders=db.Database.SqlQuery<StudentOrderViewModel>("SP_GetOrdersByStudentID @studentID", new SqlParameter("@studentID", ID));

            //var orders = db.Orders.Join(db.OrderDetails, o => o.ID, od => od.OrderID,
            //    (o, od) => new { orders = o, OrderDetail = od }).Where(x => x.orders.StudentID == ID)                
            //    .Select(x => new StudentOrderViewModel 
            //    { createdDate = x.orders.CreatedDate, OrderID = x.orders.ID, FirstName = x.orders.FirstName,LastName=x.orders.LastName,
            //        Items = x.OrderDetail.Quantity, ProductTotal = (x.OrderDetail.Quantity * x.OrderDetail.unitPrice) });
           
            return View(orders.ToList());                                   
        }


        public ActionResult OrderDetails(int ID)
        {
            var details = db.Orders.ToList().Join(db.OrderDetails, o => o.ID, od => od.OrderID, (o, od) => new { Order = o, OrderDetail = od })
                .Where(x => x.Order.ID == ID).Select(x => new OrderViewModel
                {
                    ID = x.Order.ID,
                    FirstName = x.Order.FirstName,
                    LastName=x.Order.LastName,
                    ProductID = x.OrderDetail.ProductID,
                    Quantity=x.OrderDetail.Quantity,
                    UnitPrice=x.OrderDetail.unitPrice,                    
                }).ToList();

            var next = details.Join(db.Products, d => d.ProductID, p => p.ID, (d, p) => new { OrderViewModel = d, Product = p }).Select(x => new OrderViewModel { ID = x.OrderViewModel.ID, CreatedDate = x.OrderViewModel.CreatedDate, FirstName = x.OrderViewModel.FirstName, LastName=x.OrderViewModel.LastName,ProductName = x.Product.Description, Quantity = x.OrderViewModel.Quantity, UnitPrice = x.OrderViewModel.UnitPrice });

           return View(next.ToList());
        }
       

        #endregion



        public ActionResult Index()
        {
            return View(db.Distributors.ToList());
        }

        //
        // GET: /Distributor/Details/5

        public ActionResult Details(int id = 0)
        {
            RegisterModel registermodel = db.Distributors.Find(id);
            if (registermodel == null)
            {
                return HttpNotFound();
            }
            return View(registermodel);
        }

        //
        // GET: /Distributor/Create

        public ActionResult Create()
        {


         

            //Country List
            ViewBag.Country = new SelectList(Common.GetCountries(), "ID", "Text");

            //State List
            ViewBag.State = new SelectList(Common.GetStates(), "ID", "Text");

            //Catalog List
            ViewBag.DefaultProductCatalog = new SelectList(db.Brochures, "ID", "BrochureName");

            //Ship to Catalog List
           // ViewBag.DefaultSToSCatalog = new SelectList(db.Brochures, "ID", "BrochureName");

            //Goal Type
            ViewBag.GoalType = new SelectList(Common.GoalType(), "ID", "Text");

            //Pricing Level
            ViewBag.PricingLevel = new SelectList(Common.getPricingLevel(), "ID", "Text");

            
            ViewBag.FreeShipment = new SelectList(Common.getShipAmountList(), "ID", "Text");          
            
            ViewBag.ParticipantOption = new SelectList(Common.getParticipantOption(), "ID", "Text");
            
            ViewBag.FAQ = new SelectList(Common.getFAQList(), "ID", "Text");

            return View();
        }

        //
        // POST: /Distributor/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
       public async  Task<ActionResult> Create(RegisterModel registermodel, HttpPostedFileBase FileUpload)
       //public ActionResult Create(RegisterModel registermodel, HttpPostedFileBase FileUpload)           
        {
            if (ModelState.IsValid)
            {


                // Attempt to register the user
                try
                {
                    string fileName = "";
                    string path = Server.MapPath("~/SiteImages");
                    if (FileUpload != null)
                    {
                        fileName = Path.GetFileName(FileUpload.FileName);         

                        //code to chnage name of the image if same exists in folder.         
                        string ImageName =  await ShrdMaster.Instance.SaveImage(path, fileName);
                        //string ImageName = ShrdMaster.Instance.SaveImage(path, fileName);                       
                        
                        path = Path.Combine(path, ImageName);
                        FileUpload.SaveAs(path);
                        path = "/SiteImages/" + ImageName;
                        registermodel.ImagePath = path;
                    }

                   


                    WebSecurity.CreateUserAndAccount(registermodel.UserName, registermodel.Password, new
                    {
                        Password = registermodel.Password,
                        Address1 = registermodel.Address1,
                        Address2 = registermodel.Address2,
                        City = registermodel.City,
                        State = registermodel.State,
                        Country ="United States",
                        EmailAddress = registermodel.EmailAddress,
                        PhoneNumber = registermodel.PhoneNumber,
                        FaxNumber = registermodel.FaxNumber,
                        Enabled = registermodel.Enabled,
                        PricingLevel = registermodel.PricingLevel,
                        ImagePath = registermodel.ImagePath,
                        DefaultProductCatalog = registermodel.DefaultProductCatalog,
                        ShipToSchoolFee = 0,
                        DefaultSToSCatalog = "",
                        ParticipantOption = registermodel.ParticipantOption,
                        FreeShipment = registermodel.FreeShipment,
                        GoalType = registermodel.GoalType,
                        ShipToSchool = registermodel.ShipToSchool,
                        CollectTeacher = registermodel.CollectTeacher,
                        ParentCertification = registermodel.ParentCertification,
                        StudentAddress = registermodel.StudentAddress,
                        ParentRequired = registermodel.ParentRequired,
                        CellPhone = registermodel.CellPhone,
                        registermodel.CellPhoneRequired,
                        OrganzationGoal = registermodel.OrganzationGoal,
                        EventID = "",
                        EventTypeID = "",
                        ParticipateCost = registermodel.ParticipateCost,
                        CompanyName = registermodel.CompanyName,
                        AutoAssignParticipantID = registermodel.AutoAssignParticipantID
                    });
                    //Roles.AddUserToRole(model.UserName, "Admin");
                   // WebSecurity.Login(registermodel.UserName, registermodel.Password);

                    return RedirectToAction("Index");
                }
                catch (MembershipCreateUserException e)
                {
                    ViewBag.error = e.Message;
                    //ModelState.AddModelError("Username",e.Message);
                }
            }

            //Country List
            ViewBag.Country = new SelectList(Common.GetCountries(), "ID", "Text");

            //State List
            ViewBag.State = new SelectList(Common.GetStates(), "ID", "Text");

            //Catalog List
            ViewBag.DefaultProductCatalog = new SelectList(db.Brochures, "ID", "BrochureName");

            //Ship to Catalog List
         //   ViewBag.DefaultSToSCatalog = new SelectList(db.Brochures, "ID", "BrochureName");

            //Goal Type
            ViewBag.GoalType = new SelectList(Common.GoalType(), "ID", "Text");

            //Pricing Level
            ViewBag.PricingLevel = new SelectList(Common.getPricingLevel(), "ID", "Text");


            ViewBag.FreeShipment = new SelectList(Common.getShipAmountList(), "ID", "Text");

            ViewBag.ParticipantOption = new SelectList(Common.getParticipantOption(), "ID", "Text");

            ViewBag.FAQ = new SelectList(Common.getFAQList(), "ID", "Text");

            // If we got this far, something failed, redisplay form
            return View(registermodel);
        }

        //
        // GET: /Distributor/Edit/5

        public ActionResult Edit(int id = 0)
        {
           
           RegisterModel registermodel = db.Distributors.Find(id);
            
            if (registermodel == null)
            {
                return HttpNotFound();
            }

            //Data for DropDowns


            registermodel.ConfirmPassword = registermodel.Password;
            

            //Country List
            ViewBag.Country = new SelectList(Common.GetCountries(), "ID", "Text",registermodel.Country);

            //State List
            ViewBag.State = new SelectList(Common.GetStates(), "ID", "Text", registermodel.State);

            //Catalog List
            ViewBag.DefaultProductCatalog = new SelectList(db.Brochures, "ID", "BrochureName", registermodel.DefaultProductCatalog);

            //Ship to Catalog List
            ViewBag.DefaultSToSCatalog = new SelectList(db.Brochures, "ID", "BrochureName", registermodel.DefaultProductCatalog);

            //Goal Type
            ViewBag.GoalType = new SelectList(Common.GoalType(), "ID", "Text", registermodel.GoalType);

            //Pricing Level
            ViewBag.PricingLevel = new SelectList(Common.getPricingLevel(), "ID", "Text",registermodel.PricingLevel);


            ViewBag.FreeShipment = new SelectList(Common.getShipAmountList(), "ID", "Text", registermodel.FreeShipment);



            ViewBag.ParticipantOption = new SelectList(Common.getParticipantOption(), "ID", "Text", registermodel.ParticipantOption);

            ViewBag.FAQ = new SelectList(Common.getFAQList(), "ID", "Text", registermodel.FAQ);


            return View(registermodel);
        }

        //
        // POST: /Distributor/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RegisterModel registermodel,HttpPostedFileBase FileUpload)
        //public  ActionResult Edit(RegisterModel registermodel,HttpPostedFileBase FileUpload)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (FileUpload != null)
                    {
                        string fileName = "";
                        fileName = FileUpload.FileName;
                        string path = Server.MapPath("~/SiteImages");
                        string ImageName = await ShrdMaster.Instance.SaveImage(path, fileName);
                        //string ImageName = ShrdMaster.Instance.SaveImage(path, fileName);
                        //fileName = FileUpload.FileName;
                        path = Path.Combine(path, ImageName);
                        FileUpload.SaveAs(path);
                        path = "/SiteImages/" + ImageName;
                        registermodel.ImagePath = path;
                    }

                    db.Entry(registermodel).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    ViewBag.error = ex.Message;
                }
                
                return RedirectToAction("Index");
            }


            //Country List
            ViewBag.Country = new SelectList(Common.GetCountries(), "ID", "Text", registermodel.Country);

            //State List
            ViewBag.State = new SelectList(Common.GetStates(), "ID", "Text", registermodel.State);

            //Catalog List
            ViewBag.DefaultProductCatalog = new SelectList(db.Brochures, "ID", "BrochureName", registermodel.DefaultProductCatalog);

            //Ship to Catalog List
         //   ViewBag.DefaultSToSCatalog = new SelectList(db.Brochures, "ID", "BrochureName", registermodel.DefaultProductCatalog);

            //Goal Type
            ViewBag.GoalType = new SelectList(Common.GoalType(), "ID", "Text", registermodel.GoalType);

            //Pricing Level
            ViewBag.PricingLevel = new SelectList(Common.getPricingLevel(), "ID", "Text", registermodel.PricingLevel);


            ViewBag.FreeShipment = new SelectList(Common.getShipAmountList(), "ID", "Text", registermodel.FreeShipment);



            ViewBag.ParticipantOption = new SelectList(Common.getParticipantOption(), "ID", "Text", registermodel.ParticipantOption);


            ViewBag.FAQ = new SelectList(Common.getFAQList(), "ID", "Text", registermodel.FAQ);

            return View(registermodel);
        }

        //
        // GET: /Distributor/Delete/5

        public ActionResult Delete(int id = 0)
        {
            RegisterModel registermodel = db.Distributors.Find(id);
            if (registermodel == null)
            {
                return HttpNotFound();
            }
            return View(registermodel);
        }

        //
        // POST: /Distributor/Delete/5

        [HttpPost, ActionName("Delete")]
   
        public JsonResult DeleteConfirmed(int id)
        {
            RegisterModel registermodel = db.Distributors.Find(id);
            db.Distributors.Remove(registermodel);
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