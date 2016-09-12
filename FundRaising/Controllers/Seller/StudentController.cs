using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FundRaising.App_Start;
using FundRaising.Models;
using Microsoft.AspNet.Identity;
using FundRaising.Filters;
using FundRaising.ViewModels;
using System.Drawing;
using System.Drawing.Drawing2D;



namespace FundRaising.Controllers.Seller
{
    //[Authorize(Users = "Admin")]
    [Authorize(Roles = "Student,Admin")]
   //[Authorize(Users= "Admin")]
    [InitializeSimpleMembership]
    public class StudentController : Controller
    {
        private FundRaisingDBContext db = new FundRaisingDBContext();

        //
        // GET: /Student/
       //public ActionResult Index()
       // {
       //     int Id=-1;
       //     if(Session["studentID"]!=null)
       //     {
       //         int.TryParse(Session["studentID"].ToString(),out Id);
       //     }
       //     else
       //     {
       //         return RedirectToAction("Login", "Account");
       //     }

       //     return RedirectToAction("Index", new { ID = Id });
       // }

       
        

        public ActionResult Index(string ID="-1")
        {

            if(ID =="-1")
            {
                return RedirectToAction("Login", "Account"); 
            }
            int orgID = -1;
            string stuID = ID.ToString();
            Student student = db.Students.Where(x => x.StudentID == stuID).FirstOrDefault();
            ViewBag.StudentName = student.FirstName +" "+student.LastName;
            ViewBag.StudentID = student.StudentID;
            ViewBag.SchoolID = student.SchoolID;
            ViewBag.ID = student.ID;
            orgID = student.SchoolID;
            Organization org = null;
            Session["studentIDs"] = student.StudentID;

            HttpCookie StudentCookie = new HttpCookie("StudentID");
            StudentCookie.Value = student.StudentID.ToString();
            Response.Cookies.Add(StudentCookie);
            HttpCookie StudentCookie1 = new HttpCookie("CustomStudentID");
            StudentCookie1.Value = student.StudentID.ToString();
            Response.Cookies.Add(StudentCookie1);
           // Response.Cookies.Add(StudentCookie1);
            if (student.SchoolID > 0)
            {
                org = db.Organizations.Where(x=>x.SchoolID==orgID).FirstOrDefault();
                if(org!=null)
                {
                    HttpCookie imgcookie = new HttpCookie("ImageCookie");
                    RegisterModel reg = db.Distributors.Find(org.Distributor);
                    imgcookie.Value = reg.ImagePath;
                    Response.Cookies.Add(imgcookie);
                   
                }
               
            }

            if (org != null)
            {
                ViewBag.SchoolName = org.Name;
                Session["SchoolName"] = org.Name;
                Session["GoalType"] = org.GoalType;
                if (student.DefaultGoal == null)
                {
                    Session["Goal"] = org.DefaultGoal;
                }
                else
                {
                    Session["Goal"] = student.DefaultGoal;
                }
            }
            
           
            return View();
        }

        //
        // GET: /Student/Details/5

        public ActionResult Details(string id = "-1")
        {
           // string stuID=id.ToString();
            Student student = db.Students.Where(x => x.StudentID == id).FirstOrDefault();
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = student.ID;
            return View(student);
        }

        //
        // GET: /Student/Create
        [AllowAnonymous]
        public ActionResult Create(string ID="-1")
        {
           
            Organization org = null;
            if (Session["studentIDs"] != null)
            {
                ViewBag.StudentID = Session["studentIDs"].ToString();
            }
            //else
            //{
            //    return RedirectToAction("Register", "Student");
            //}

            if (Session["Organization"] != null)
            {
                org = Session["Organization"] as Organization;
            }
            if (org != null)
            {
                var distributor = db.Distributors.Find(org.Distributor);
                if (org.CollectTeacherGrade)
                {
                    ViewBag.teacher = "yes";
                }
                else
                {
                    ViewBag.teacher = "no";
                }
                if (distributor.ParentRequired)
                {
                    ViewBag.ParentRequired = "yes";
                }
                else
                {
                    ViewBag.ParentRequired = "no";
                }

                if (org.CellPhoneRequired)
                {
                    ViewBag.cellRequired = "yes";
                }
                else
                {
                    ViewBag.cellRequired = "no";
                }
                ViewBag.Goal = org.DefaultGoal;
            }

            Student student = null;

            if (ID != "-1")
            {
                
                student = db.Students.Where(x => x.StudentID == ID).FirstOrDefault();
                ViewBag.ID = student.ID;
                ViewBag.ShowButton = 1;
                return View(student);
            }

            ViewBag.Grade = new SelectList(Common.Grades(), "ID", "Description"); 
            
           // ViewBag.ID = student.ID;
            return View();
        }

        //
        // POST: /Student/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [InitializeSimpleMembership]
        [AllowAnonymous]
        public async Task<ActionResult> Create(Student student)
        {
           
            try
            {
                ViewBag.Grade = new SelectList(Common.Grades(), "ID", "Description"); 
                Organization org = Session["Organization"] as Organization;
                if(org!=null)
                {
                    student.SchoolID = org.SchoolID;
                    student.DefaultGoal = org.DefaultGoal.ToString();
                    student.Message="";
                    student.image = "";
                    student.Active = true;
                }
                
               
               

                //RegisterModel distributor = db.Distributors.Find(org.Distributor);
                //student.image=org.
               
                db.Students.Add(student);
                
                db.SaveChanges();
                Session["StudentIDs"] = student.StudentID;
                
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

                UserRole userrole = new Models.UserRole();
                userrole.UserId = student.ID;
                userrole.RoleId = 2;
                db.UserRoles.Add(userrole);
                db.SaveChanges();
                //Roles.AddUserToRole("Admin", "Student");
                ViewBag.ID = student.ID;
                return RedirectToAction("personalization", new { studentID = student.StudentID, option = 1 });
                //return RedirectToAction("Success", new { ID=student.StudentID });
               
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }  


           // return View(student);
        }


       
        //
        // GET: /Student/Edit/5
       
        public ActionResult Edit(string  id ="-1",int option=0)
        {
            Organization org = null;
            
            int orgID;            
            Student student = db.Students.Where(x => x.StudentID == id).FirstOrDefault();
            orgID=student.SchoolID;
            //org = db.Organizations.Find(orgID);
            org = db.Organizations.Where(x => x.SchoolID == orgID).SingleOrDefault();
            
            if (org != null)
            {
                var distributor = db.Distributors.Find(org.Distributor);
                if (org.CollectTeacherGrade)
                {
                    ViewBag.teacher = "yes";
                }
                else
                {
                    ViewBag.teacher = "no";
                }
                if (distributor.ParentRequired)
                {
                    ViewBag.ParentRequired = "yes";
                }
                else
                {
                    ViewBag.ParentRequired = "no";
                }
                if (org.CellPhoneRequired)
                {
                    ViewBag.cellRequired = "yes";
                }
                else
                {
                    ViewBag.cellRequired = "no";
                }
            }
          
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentName = student.FirstName+" "+student.LastName;
            ViewBag.StudentID = student.StudentID;
            ViewBag.SchoolID = student.SchoolID;
            ViewBag.ID = student.ID;
            ViewBag.SchoolName= org.Name;
            ViewBag.Grade = new SelectList(Common.Grades(), "ID", "Description",student.Grade);
            if(option==1)
            {
                ViewBag.Layout ="~/Views/Shared/_Layout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Student/_AuthLayout.cshtml";
            }
            HttpCookie Coption = new HttpCookie("Option");
            Coption.Value = option.ToString();
            Response.Cookies.Add(Coption);
            ViewBag.ID = student.ID;
            return View(student);
        }

        //
        // POST: /Student/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]        
        public ActionResult Edit(Student student)
        {
            int option=0;
            if (ModelState.IsValid)
            {
                student.Active = true;
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                if(Request.Cookies["Option"]!=null)
                {
                    int.TryParse(Request.Cookies["Option"].Value.ToString(), out option);
                    
                }
                if(option==1)
                {
                    return RedirectToActionPermanent("StudentDetails", "Distributor", new { id = student.ID });
                }
                return RedirectToAction("Index", new { ID=student.StudentID});
            }
            ViewBag.ID = student.ID;
            return View(student);
        }

        //
        // GET: /Student/Delete/5
       [AllowAnonymous]
        public ActionResult Delete(int id = -1)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        //
        // POST: /Student/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

       [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Register(FormCollection form)
        {
            int OrganizationID=-1;
            Organization org=null;
            Campaign camp = null;
            if(form["SchoolID"]!=null)
            {
                int.TryParse(form["SchoolID"].ToString(), out OrganizationID);
                org=await ShrdMaster.Instance.organizationGetByID(OrganizationID);
                if(org!=null)
                {
                    camp = await ShrdMaster.Instance.GetActiveCampaignByOrganizationId(OrganizationID);
                    if(camp!=null)
                    {
                        ViewBag.Result = org.Name;
                        Session["Organization"] = org;
                        HttpCookie imgcookie = new HttpCookie("ImageCookie");
                        RegisterModel reg=db.Distributors.Find(org.Distributor);
                        imgcookie.Value=reg.ImagePath;
                        Response.Cookies.Add(imgcookie);
                        Session["Campaign"] = camp;
                        return View("registerverify");
                    }
                }
            }
            
            ViewBag.Result = 0;
           // ViewBag.ID = student.ID;
            return View();            
        }


       [AllowAnonymous]
        public ActionResult Register2()
        {
            if (Session["Organization"] == null)
            {
                return RedirectToAction("Register", "Student");
            }

            Organization org = Session["Organization"] as Organization;


            // Code to generate autonumber with school name initial    
            if(org.AutoAssignParticipantID)
            {
                string number =ShrdMaster.Instance.AutoGenerateNumber(org.ID);
                Session["StudentIDs"] = number;
                ViewBag.StudentID = number;
                //Session["studentID"] = ViewBag.studentID;
                return RedirectToAction("Create", "Student");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register2(FormCollection fc)
        {
            Organization org = null;
            if (Session["Organization"] != null)
            {
                org = Session["Organization"] as Organization;
            }
            else
            {
                return RedirectToAction("Register", "Student");
            }
           // string studentID="";
            if(fc["studentID"]!=null)
            {
                
                ViewBag.studentID = fc["studentID"].ToString();
              
                if (!ShrdMaster.Instance.CheckStudentID(ViewBag.studentID,org.SchoolID))
                {
                    ViewBag.Message = "Student Id already registered for the school.";
                    return View();
                }
                Session["StudentIDs"] = fc["studentID"].ToString(); ;
               // Session["StudentIDs"] = ViewBag.studentID;
               
            }                        
                //// Code to generate autonumber with school name initial    
                //if (org.AutoAssignParticipantID)
                //{
                 
                   
                //}
                return RedirectToAction("Create", "Student");
                                                         
        }




       [AllowAnonymous]
        public ActionResult Success(string ID)
        {
            ViewBag.studentID = ID;
            return View("_success");
        }




       [AllowAnonymous]
        public ActionResult  GoalSetUp(string studentID,int option)
        {
            GoalSetupData(studentID);
            if(option==1)
            {
                ViewBag.Layout = "~/Views/Student/_Layout.cshtml";
                ViewBag.Option =1;
            }
            else if(option==0)
            {
                ViewBag.Layout = "~/Views/Student/_AuthLayout.cshtml";
                ViewBag.Option = 0;
            }
          
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GoalSetUp(FormCollection fc)
        {
            string studentID="";
            int goal = -1;
            string _goal="";
            if(fc!=null)
            {
               studentID= fc["sID"].ToString();
               _goal = fc["Goal"];
               _goal = _goal.Replace(".00", "");
               int.TryParse(_goal, out goal);
            }
            Student student = db.Students.Where(x=>x.StudentID==studentID).FirstOrDefault();
            student.DefaultGoal = goal.ToString() ;
            db.SaveChanges();

            if(fc["option"].ToString()=="1")
            {
                return RedirectToAction("personalization", new { studentID=student.StudentID,option=1});
            }
            else
            {
                return RedirectToAction("index", new { ID = student.StudentID });
            }
          
        }



       [AllowAnonymous]
        private void GoalSetupData(string studentID)
        {

            Student student = db.Students.Where(x => x.StudentID == studentID).FirstOrDefault();

            Session["studentIDs"] = studentID;
            if (student.SchoolID>0)
            {
                int ID = -1;
                ID=student.SchoolID;
                //int.TryParse(, out ID);

                Organization org = db.Organizations.Where(x=>x.SchoolID==student.SchoolID).SingleOrDefault();

                if(org!=null)
                {
                    if (org.GoalType == "0")
                    {
                        ViewBag.GoalType = "Item";
                    }
                    else
                    {
                        ViewBag.GoalType = "Money";
                    }
                    ViewBag.SchoolName = org.Name;
                }
               
                if(!string.IsNullOrEmpty(student.DefaultGoal)&&student.DefaultGoal!="0")
                {
                    ViewBag.Goal = student.DefaultGoal.Replace(".00","");
                }
                else
                {
                    ViewBag.Goal = org.DefaultGoal.ToString();
                }
                ViewBag.StudentName = student.FirstName+" "+student.LastName;
                ViewBag.StudentID = student.StudentID;
                ViewBag.SchoolID = student.SchoolID;
                ViewBag.ID = student.ID;
                
            }   
        }

        private void PersonalizationData(string  studentID="-1")
        {
            string  studentid="";
            Student student = null;
            if(studentID!="-1")
            {
                student = db.Students.Where(x => x.StudentID == studentID).FirstOrDefault();
            }
            else
            {
                if (Session["studentIDs"] != null)
                {
                   // int.TryParse(Session["studentIDs"].ToString(), out studentid);
                    studentid = Session["studentIDs"].ToString();
                    student = db.Students.FirstOrDefault(x => x.StudentID == studentid);
                }
            }                   
            if (student.SchoolID>0)
            {
                int ID = -1;
                ID=student.SchoolID;
                //int.TryParse(, out ID);

                //Organization org = db.Organizations.Find(ID);
                Organization org = db.Organizations.Where(x=>x.SchoolID==ID).SingleOrDefault();
                string message="";
                if(org!=null)
                {
                    if (org.ParticipantOption == "NameEmailandPersonalGreetingandPhoto" || org.ParticipantOption == "NameEmailAndPersonalGreeting")
                    {
                        //if student has customized message
                        if (string.IsNullOrEmpty(student.Message))
                        {
                            Dictionary<string, string> param = new Dictionary<string, string>();
                            param.Add("<%orgname%>", org.Name);
                            message = ShrdMaster.Instance.buildEmailBody("DefaultMailForInvitation.txt", param);
                            message = message.Replace("<%orgname%>", org.Name);
                        }
                        else
                        {
                            message = student.Message;
                        }

                        if (org.ParticipantOption == "NameEmailandPersonalGreetingandPhoto")
                        {
                            // If the stduent has image path then this will execute
                            if (!string.IsNullOrEmpty(student.image))
                            {
                                ViewBag.Image = student.image;
                            }
                            else
                            {
                                ViewBag.Image = "";
                            }
                        }
                    }
                
                }
                

                ViewBag.Message = message;
                ViewBag.DefaultValue = org.DefaultGoal;
                ViewBag.StudentName = student.FirstName+" "+student.LastName;
                ViewBag.StudentID = student.StudentID;
                ViewBag.SchoolID = student.SchoolID;
                ViewBag.ID = student.ID;
                ViewBag.SchoolName = org.Name;
                //Session["SchoolName"] = org.Name;
                //Session["GoalType"] = org.GoalType;
                //if(student.DefaultGoal==null)
                //{
                //    Session["Goal"] = org.DefaultGoal;
                //}
                //else
                //{
                //    Session["Goal"] = student.DefaultGoal;
                //}
                
            }
        }
       [AllowAnonymous]
       
        public ActionResult Personalization(string studentID="-1",int option=-1)
        {
            PersonalizationData(studentID);
            if (option == 1)
            {
                ViewBag.Layout = "~/Views/Student/_Layout.cshtml";
                ViewBag.Option = 1;
            }
            else if (option == 0)
            {
                ViewBag.Layout = "~/Views/Student/_AuthLayout.cshtml";
                ViewBag.Option = 0;
            }
             return View();

        }

      

        [HttpPost]
        [AllowAnonymous]
       [ValidateInput(false)]
        public async  Task<ActionResult> Personalization(HttpPostedFileBase FileUpload,FormCollection fc)
        // public ActionResult Personalization(HttpPostedFileBase FileUpload,FormCollection fc)
        {
            int studentid,orgID=-1;
            string message="";
             Student student=null;
        
                if(fc["ID"].ToString()!=null)
                {
                    int.TryParse(fc["ID"].ToString(),out studentid);
                    student = db.Students.Find(studentid);
                }
                PersonalizationData(student.StudentID);

              
                if(student!=null)
                {
                    orgID = student.SchoolID;
                   
                    //int.TryParse(student.SchoolID, out orgID);
                    string mailImagePath="";
                    //Organization org = db.Organizations.Find(orgID);

                    Organization org = db.Organizations.Where(x=>x.SchoolID==orgID).SingleOrDefault();
                    EmailService email = new EmailService();
                    IdentityMessage details = new IdentityMessage();
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    if(fc["message"]!=null)
                    {
                        message = fc["message"].ToString();
                        string fileName = "";
                        string oldpath=Server.MapPath("~/SiteImages");
                        string filepath = Server.MapPath("~/SiteImages");
                        string dbpath = "";

                        if (FileUpload != null)
                        {
                            fileName = Path.GetFileName(FileUpload.FileName);
                            //code to generate different name if the filename exists
                            string ImageName = await ShrdMaster.Instance.SaveImage(filepath, fileName);
                            // string ImageName =ShrdMaster.Instance.SaveImage(path, fileName);
                            //fileName = FileUpload.FileName;
                            filepath = Path.Combine(filepath, ImageName);
                            FileUpload.SaveAs(filepath);
                            string imagename1;
                            using (var srcImage = Image.FromFile(filepath))
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
                                    imagename1 = await ShrdMaster.Instance.SaveImage(oldpath, fileName);
                                    oldpath = Path.Combine(oldpath, imagename1);
                                    newImage.Save(oldpath);                                    
                                }
                            }
                            try
                            {
                                System.IO.File.Delete(filepath);
                            }
                            catch
                            {

                            }
                            mailImagePath = oldpath;
                            dbpath = "/SiteImages/" + imagename1;
                            //if (FileUpload.ContentLength >= 150000)
                            //{
                            //    ViewBag.error = "Image length is too large";
                            //    string opt = fc["option"].ToString();
                            //    if (opt == "1")
                            //    {
                            //        ViewBag.Layout = "~/Views/Student/_Layout.cshtml";
                            //        ViewBag.Option = 1;
                            //        return View();
                            //    }
                            //    else if (opt == "0")
                            //    {
                            //        ViewBag.Layout = "~/Views/Student/_AuthLayout.cshtml";
                            //        ViewBag.Option = 0;
                            //        return View();
                            //    }
                                
                            //}

                           
                            //product.ImageUrl = path;
                            
                           
                              
                        }
                        else
                        { 
                            if(student!=null)
                            {
                                dbpath = student.image;
                            }
                            else
                            {
                                dbpath = null; 
                            }

                       }
                      
                       

                       
                     
                        //param.Add("<%src%>",mailImagePath);
                                               
                                          
                        student.Message = message;
                        student.image = dbpath;
                        db.SaveChanges();
                                                                                                                      
                    }
                    //Email service
                    param.Add("<%msg%>", message);
                    param.Add("<%Name%>", student.FirstName + " " + student.LastName);
                    param.Add("<%link%>", "fundraising.infodatixhosting.com/Customer/index/" + student.StudentID);
                    if (!string.IsNullOrEmpty(mailImagePath))
                    {
                        details.Subject = "Help " + student.FirstName + " raise money for " + org.Name + "<" + mailImagePath;
                    }
                    else
                    {
                        details.Subject = "Help " + student.FirstName + " raise money for " + org.Name;
                    }
                    details.Destination = student.EmailAddress;

                    details.Body = ShrdMaster.Instance.buildEmailBody("InviteEmailTemplate.txt", param);
                    await email.SendAsync(details);
                    return RedirectToAction("Index", new { ID = student.StudentID });      
                }  
                else
                {
                    return RedirectToAction("Login","Account");
                }
                
           
            return RedirectToAction("Index", new { ID = student.StudentID });  

        }

       [AllowAnonymous]
        public ActionResult SocialMedia()
        {
            if(Session["studentIDs"]!=null)
            {
                ViewBag.studentID = Session["studentIDs"].ToString();

                
            }
            return View();
            
        }


       public ActionResult Orders(int ID)
       {
           string studentID = "";
           if (Session["StudentIDs"] != null)
           {
               studentID = Session["StudentIDs"].ToString();
               //int.TryParse(, out studentID);
           }
           else
           {
               return RedirectToAction("Login", "Account");
           }
           var student = db.Students.Where(x => x.StudentID == studentID).FirstOrDefault();

           var details = db.Orders.ToList().Join(db.OrderDetails, o => o.ID, od => od.OrderID, (o, od) => new { Order = o, OrderDetail = od })
               .Where(x => x.Order.ID == ID).Select(x => new OrderViewModel
               {
                   ID = x.Order.ID,
                   FirstName = x.Order.FirstName,
                   LastName  =x.Order.LastName,
                   ProductID = x.OrderDetail.ProductID,
                   Quantity = x.OrderDetail.Quantity,
                   UnitPrice = x.OrderDetail.unitPrice,
               }).ToList();

           var next = details.Join(db.Products, d => d.ProductID, p => p.ID, (d, p) => new { OrderViewModel = d, Product = p }).Select(x => new OrderViewModel { ID = x.OrderViewModel.ID, CreatedDate = x.OrderViewModel.CreatedDate, FirstName = x.OrderViewModel.FirstName,LastName=x.OrderViewModel.LastName, ProductName = x.Product.Description, Quantity = x.OrderViewModel.Quantity, UnitPrice = x.OrderViewModel.UnitPrice });
           ViewBag.StudentName = student.FirstName +" "+student.LastName;
           ViewBag.StudentID = student.StudentID;
           ViewBag.SchoolID = student.SchoolID;
          ViewBag.ID = student.ID;
           if(Session["SchoolName"]!=null)
           {
               ViewBag.SchoolName = Session["SchoolName"];
           }
           if(Session["GoalType"]!=null)
           {
               ViewBag.GoalType = Session["GoalType"];
           }

           if(Session["Goal"]!=null)
           {
               ViewBag.Goal = Session["Goal"];
           }
        //   orgID = student.SchoolID;
           return View(next.ToList());
       }

       public ActionResult OrderDetails(int ID = -1)
       {


           string studentID = "";
           int orgId = -1;
           if(Session["StudentIDs"]!=null)
           {
               studentID=Session["StudentIDs"].ToString();
               //int.TryParse(Session["StudentIDs"].ToString(), out studentID);
           }
           else
           {
               return RedirectToAction("Login", "Account");
           }

           var student = db.Students.Where(x => x.StudentID == studentID).FirstOrDefault(); ;
           if(student!=null)
           {
               int go=0;
               int.TryParse(student.DefaultGoal,out go);
               
                   ViewBag.Goal = go;
               
               
               //var org = db.Organizations.Find(student.SchoolID);
                   ViewBag.StudentName = student.FirstName + " " + student.LastName;
               ViewBag.StudentID = student.StudentID;
               ViewBag.SchoolID = student.SchoolID;
               ViewBag.ID = student.ID;
               if (Session["SchoolName"] != null)
               {
                   ViewBag.SchoolName = Session["SchoolName"];
               }
               if (Session["GoalType"] != null)
               {
                   ViewBag.GoalType = Session["GoalType"];
               }

               if (Session["Goal"] != null)
               {
                   ViewBag.Goal = Session["Goal"];
               }
             // orgID = student.SchoolID;
           }

           var orders = db.Orders.Join(db.OrderDetails, o => o.ID, od => od.OrderID,
               (o, od) => new { orders = o, OrderDetail = od }).Where(x => x.orders.StudentID == student.StudentID)
               .Select(x => new StudentOrderViewModel
               {
                   createdDate = x.orders.CreatedDate,
                   OrderID = x.orders.ID,
                   FirstName  = x.orders.FirstName,
                   LastName=x.orders.LastName,
                   Items = x.OrderDetail.Quantity,
                   ProductTotal = (x.OrderDetail.Quantity * x.OrderDetail.unitPrice)
               });
           int count = 0;
           if(orders.Count()>0)
           {
               count = orders.Select(x => x.Items).Sum();
           }
          
           ViewBag.Count = count;
           return View(orders.ToList());
       }


       [AllowAnonymous]
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}