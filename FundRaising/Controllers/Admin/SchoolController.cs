using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FundRaising.Models;
using FundRaising.ViewModels;
using System.Net.Mail;
using FundRaising.App_Start;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace FundRaising.Controllers.Admin
{
    public class SchoolController : Controller
    {
        private FundRaisingDBContext db = new FundRaisingDBContext();

        // GET: School
        public ActionResult Index(int id)
        {
            
            return View(db.Organizations.ToList());
        }

        private void GetGoalData(int id)
        {

        

        }


        public ActionResult Dashboard(int id)
        {
            
            Organization org = db.Organizations.Where(x => x.ID == id).SingleOrDefault();
            Session["Organization"] = org;
            Session["OrgID"] = org.ID;
            HttpCookie StudentCookie = new HttpCookie("OrgID");
            StudentCookie.Value = org.ID.ToString();
            Response.Cookies.Add(StudentCookie);
            SetSchoolInformation();
            return View();
        }


        public void SetSchoolInformation()
        {
            //string id;
            Organization org;
            if(Session["Organization"] !=null)
            {
                org= Session["Organization"] as Organization;
                if (org != null)
                {
                    var data = db.Campaigns.Where(x => x.OrganizatonID == org.SchoolID && x.CampaignEndDate >= DateTime.Now).FirstOrDefault();
                    //TempData["CampaignName"] = data.CampaignName;
                    ViewBag.CampaignName = data.CampaignName;
                    ViewBag.CampaignStartDate = data.CampaignStartDate.ToShortDateString();
                    ViewBag.CampaignEndDate = data.CampaignEndDate.ToShortDateString();
                    ViewBag.SchoolID = data.OrganizatonID;
                }

                                
                
            }
            
        }

        public ActionResult Reports()
        {

            //   Organization org = db.Organizations.Where(x => x.ID == schoolID).SingleOrDefault();

            //   var data = db.Campaigns.Where(x => x.OrganizatonID == org.SchoolID).SingleOrDefault();
            ////   TempData["CampaignName"] = data.CampaignName;
            //   ViewBag.CampaignName = data.CampaignName;
            //   ViewBag.CampaignStartDate = data.CampaignStartDate;
            //   ViewBag.CampaignEndDate = data.CampaignEndDate;
            //   ViewBag.SchoolID = data.OrganizatonID;
            SetSchoolInformation();
            return View();
        }

        public ActionResult sortedbyteacher()
        {
            SetSchoolInformation();
            return View();
        }

        public ActionResult sortedbyparticipant()
        {
            SetSchoolInformation();
            return View();
        }

        public ActionResult Participants(FormCollection fc)
        {
            SetSchoolInformation();

            //var std = db.Students.Where(x => (x.FirstName + x.LastName).Contains(fc["name"])).ToList().Select(x => new Student
            //{
            //    ID = x.ID,
            //    StudentID = x.StudentID,
            //    FullName = x.FirstName + " " + x.LastName,
            //    EmailAddress = x.EmailAddress,
            //    TeacherName = x.TeacherName,
            //}).ToList();


            var std = db.Students.ToList().Select(x => new Student
            {
                ID = x.ID,
                StudentID = x.StudentID,
                FullName = x.FirstName + " " + x.LastName,
                EmailAddress = x.EmailAddress,
                TeacherName = x.TeacherName,
            }).ToList();



            return View(std);

        }
        

        public ActionResult Email()
        {
            //var sd = db.Students.ToList();
            var s = db.Students.ToList().Count;

            ViewBag.stdcount = s;

            SetSchoolInformation();
            return View();
        }

       
        [HttpPost]
        public async Task<ActionResult> Email( FormCollection fc)
        {
            var sd = db.Students.ToList();
            var s = sd.Count();
            string cc;
            EmailService service = new App_Start.EmailService();
            var std = db.Students.ToList().Select(x => new Student
            {
                ID = x.ID,
                EmailAddress = x.EmailAddress
            }).ToList();

            foreach (var student in std)
            {
                //MailAddress cc = new MailAddress(fc["cc"]);
                IdentityMessage message = new IdentityMessage();
                cc = fc["cc"].ToString();
                message.Destination = student.EmailAddress;
                if (!string.IsNullOrEmpty(cc))
                {
                    message.Destination += ";" + cc;
                }
                
                message.Body = fc["message"];
                message.Subject = fc["subject"];
                await service.SendAsync(message);
            }                   
            ViewBag.stdcount = s;
            SetSchoolInformation();
            return View();
        }
        public ActionResult Faq()
        {

            SetSchoolInformation();
            return View();
        }

        public ActionResult Help()
        {
            SetSchoolInformation();
            return View();
        }

        public ActionResult OrderDetails(string id)
        {

            var std = db.Database.SqlQuery<StudentViewModel>("sp_GetTotalSaleByStudentID  @studentID", new SqlParameter("@studentID", id)).FirstOrDefault();

            if (std != null)
            {

                if (std.Active == true)
                {
                    string s = "Active";
                    std.Status = s;

                }

                else
                {
                    string s = "DeActive";
                    std.Status =s;


                }


                Student student = new Student();
                    student.SchoolID = std.SchoolID;
                    student.SchoolName = std.SchoolName;
                    student.username = std.username;
                    student.CampaignName = std.CampaignName;
                    student.StudentID = std.StudentID;
                    student.image = std.image;
                    student.TeacherName = std.TeacherName;
                    student.Grade = std.Grade;
                    student.EmailAddress = std.EmailAddress;
                    student.DateRegistered = std.DateRegistered;
                    student.totalsales = std.totalsales;
                    student.CampaignDates = std.CampaignDates;
                    student.totalOrders = std.totalOrders;
                    student.Active = std.IsActive;
                    student.FullName = std.FullName;
                   student.Status = std.Status;

               

            }

            else
            {

                return RedirectToAction("Participants");
            }


            SetSchoolInformation();

            //.Select(x => new Student
            //{

            //}).ToList();

            return View(std);
        }

        public ActionResult StudentDetails(string id)
        {
            var student= db.Students.Where(x => x.StudentID == id).FirstOrDefault();            
            SetSchoolInformation();
            return View(student);
        }

        [HttpPost]
        public async Task<ActionResult> StudentDetails(int id, FormCollection fc)
        {
            string cc;
            var student = db.Students.Where(x => x.ID == id).FirstOrDefault();
            EmailService service = new App_Start.EmailService();
            IdentityMessage message = new IdentityMessage();
            cc = fc["cc"].ToString();
            message.Destination = student.EmailAddress;
            if (!string.IsNullOrEmpty(cc))
            {
                message.Destination += ";" + cc;
            }

            message.Body = fc["message"];
            message.Subject = fc["subject"];
            await service.SendAsync(message);
            SetSchoolInformation();
            return View(student);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
