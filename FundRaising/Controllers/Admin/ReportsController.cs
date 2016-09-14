using FundRaising.Reports;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FundRaising.Models;
using System.IO;

namespace FundRaising.Controllers.Admin
{
    public class ReportsController : Controller
    {
        //
        // GET: /Reports/
       ReportDataSet rds = new ReportDataSet();
        ReportViewer rpt;
        FundRaisingDBContext db = new Models.FundRaisingDBContext();
        public ActionResult Index()
        {
            return RedirectToAction("SalesByOrganization");
        }

      

        #region SalesByOrganization
        public ActionResult SearchSchool(string prefix)
        {
            var data = ShrdMaster.Instance.GetOrganizations(prefix);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Getcampaigns(string ID)
        {
            var data = db.Campaigns.Where(x => x.OrganizatonID == ID).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetCampaignById(int id)
        {
            var data = db.Campaigns.FirstOrDefault(x => x.ID == id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalesByOrganization()
        {
            //ReportViewer rptViewer = new ReportViewer();
            //ViewBag.ReportViewer = rptViewer;
            return View();
        }



        public ActionResult AjaxSalesByOrganization(string SchoolID,string CampaignID )
        {
            ReportViewer rptViewer = new ReportViewer();
            rptViewer.ProcessingMode = ProcessingMode.Local;
            rptViewer.SizeToReportContent = true;
            rptViewer.Width = Unit.Percentage(100);
            rptViewer.Height = Unit.Percentage(100);
            FillDataSet(rds.SalesByOganization.TableName, "sp_SalesReportByOrganization", new SqlParameter("@organizationID", 1010), new SqlParameter("@CampaignID", 1));
            rptViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\SalesByOrganization.rdlc";
            rptViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", rds.Tables[0]));
          //  ShrdMaster.Instance.GenerateReport("SalesByOrganization.rdlc", "sp_SalesReportByOrganization", "SalesByOganization", ref rptViewer, new SqlParameter("@organizationID", 1010), new SqlParameter("@CampaignID", 1));
            ViewBag.ReportViewer = rptViewer;
            return PartialView("_SalesByOrganization");

        }

        #endregion  SalesByOgranization



        #region Organizations_with_Active_Campaign


        public ActionResult OrganizationWithActivecampaign()
        {
            ReportViewer rptViewer = new ReportViewer();
            rptViewer.ProcessingMode = ProcessingMode.Local;
            rptViewer.SizeToReportContent = true;
            rptViewer.Width = Unit.Percentage(100);
            rptViewer.Height = Unit.Percentage(100);
            FillDataSet(rds.OrganizationActiveCampaign.TableName, "sp_ReportActiveOrganizationCampaigns",null);
            rptViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\SalesByOrganization.rdlc";
            rptViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", rds.Tables[0]));
            return View();
        }

        #endregion  Organizations_with_Active_Campaign


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
                    foreach (var item in sqlparameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(rds,rds.SalesByOganization.TableName);
                }
            }
        }

    }
    }
