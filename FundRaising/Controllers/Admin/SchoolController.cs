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
using System.Configuration;
using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;
using FundRaising.Reports;

namespace FundRaising.Controllers.Admin
{
    public class SchoolController : Controller
    {
        private FundRaisingDBContext db = new FundRaisingDBContext();


        ReportDataSet rds = new ReportDataSet();
        Organization org = null;
        public ActionResult Dashboard(int id)
        {
            var std = db.Students.Count();
            ViewBag.totalstd = std;
            Organization org = db.Organizations.Where(x => x.ID == id).SingleOrDefault();
            Session["Organization"] = org;

            if(Session["Oragnization"]!= null)
                {

                org = Session["Organization"] as Organization;

                if (org != null)
                {
                    var data = db.Campaigns.Where(x => x.OrganizatonID == org.SchoolID && x.CampaignEndDate >= DateTime.Now).FirstOrDefault();
                    ViewBag.CampaignEndDate = data.CampaignEndDate.ToShortDateString();
                }
            }


            Session["OrgID"] = org.ID;
            HttpCookie StudentCookie = new HttpCookie("OrgID");
            StudentCookie.Value = org.ID.ToString();
            Response.Cookies.Add(StudentCookie);
            SetSchoolInformation();
            return View();
        }

        public ActionResult EmailtoAdmin()
        {
            
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> EmailtoAdmin(string id, FormCollection fc)
        {
          //  string cc;

            var data = db.Distributors.Where(x => x.UserName == "Admin").FirstOrDefault();
            string str = data.EmailAddress;
            //ViewBag.adminemail = str;

            var student = db.Students.Where(x => x.StudentID == id).FirstOrDefault();
            EmailService service = new App_Start.EmailService();
            IdentityMessage message = new IdentityMessage();
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("<%Name%>", fc["yourname"]);
            param.Add("<%Phone%>", fc["phone"]);
            param.Add("<%Email%>", fc["youremail"]);
            param.Add("<%Message%>", fc["message"]);
           string body= ShrdMaster.Instance.buildEmailBody("ContactUsTemplate.txt", param);                       
            message.Destination = str;
            message.Body = body;
            Organization org=null;
            if(Session["Organization"]!=null)
            {
                org = Session["Organization"] as Organization;
                if(org==null)
                {
                    return RedirectToAction("Login","Account");
                }
            }

            message.Subject = "Enquiry www.fundraising.com: From "+org.Name;

            await service.SendAsync(message);
            SetSchoolInformation();

            return View(student);
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
        public bool SetOrganization()
        {
            if (Session["Organization"] != null)
            {
                org = Session["Organization"] as Organization;
                return true;
            }
            return false;
        }
        public ActionResult ReportByParticpant()
        {
            if (SetOrganization())
            {
                ReportViewer rptViewer = new ReportViewer();
                rptViewer.ProcessingMode = ProcessingMode.Local;
                rptViewer.SizeToReportContent = true;
                rptViewer.Width = Unit.Percentage(100);
                rptViewer.Height = Unit.Percentage(100);
                FillDataSet(rds.AdminReportByParticipant.TableName, "sp_ReportByParticipant", new SqlParameter("@schoolID",org.SchoolID));
                rptViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\SchoolReportByParticipant.rdlc";
                rptViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", rds.Tables[rds.AdminReportByParticipant.TableName]));
                ViewBag.ReportViewer = rptViewer;
                SetSchoolInformation();            
            }
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
        public async Task<ActionResult> Email(FormCollection fc)
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
                
                if (!string.IsNullOrEmpty(cc))
                {
                    message.Destination += ";" + cc;
                }

                message.Destination = student.EmailAddress;
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
        public async Task<ActionResult> StudentDetails(string id, FormCollection fc)
        {
            string cc;
            var student = db.Students.Where(x => x.StudentID == id).FirstOrDefault();
            EmailService service = new App_Start.EmailService();
            IdentityMessage message = new IdentityMessage();
            cc = fc["cc"].ToString();
            if (!string.IsNullOrEmpty(cc))
            {
                message.Destination += ";" + cc;
            }
            message.Destination = student.EmailAddress;
            message.Body = fc["message"];
            message.Subject = fc["subject"];
            await service.SendAsync(message);
            SetSchoolInformation();
            return View(student);
        }

        public void FillDataSet(string tableName, string procName, params SqlParameter[] sqlparameters)
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = procName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (sqlparameters != null)
                    {
                        foreach (var item in sqlparameters)
                        {
                            cmd.Parameters.Add(item);
                        }

                    }


                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(rds, tableName);
                }
            }
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
