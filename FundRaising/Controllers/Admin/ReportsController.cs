using FundRaising.Reports;
//using Microsoft.Reporting.WebForms;
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
using Microsoft.Reporting.WebForms;

namespace FundRaising.Controllers.Admin
{
    public class ReportsController : Controller
    {
        //
        // GET: /Reports/
       ReportDataSet rds = new ReportDataSet();
        //ReportViewer rpt;
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
            if (!string.IsNullOrEmpty(SchoolID)&& !string.IsNullOrEmpty(CampaignID))
            {
                
                rptViewer.ProcessingMode = ProcessingMode.Local;
                rptViewer.SizeToReportContent = true;
                rptViewer.Width = Unit.Percentage(100);
                rptViewer.Height = Unit.Percentage(100);
                FillDataSet(rds.CampaignSalesReport.TableName, "sp_SalesReportByOrganization", new SqlParameter("@organizationID", SchoolID), new SqlParameter("@CampaignID", CampaignID));
                rptViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\OrganizationSalesReport.rdlc";
                rptViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", rds.Tables[rds.CampaignSalesReport.TableName]));
                //  ShrdMaster.Instance.GenerateReport("SalesByOrganization.rdlc", "sp_SalesReportByOrganization", "SalesByOganization", ref rptViewer, new SqlParameter("@organizationID", 1010), new SqlParameter("@CampaignID", 1));                
            }
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
            FillDataSet(rds.ActiveCampaignOfOrganizations.TableName, "sp_ReportActiveOrganizationCampaigns",null);
            rptViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\ActiveCampaignOfOrganizations.rdlc";
            rptViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", rds.Tables[rds.ActiveCampaignOfOrganizations.TableName]));
            ViewBag.ReportViewer = rptViewer;
            return View();
        }

        #endregion  Organizations_with_Active_Campaign



        #region  OrganizationSalesReport

        public ActionResult OrganizationSalesReport()
        {
            ViewBag.Distrinutor = new SelectList(db.Distributors.ToList(), "userID", "Username");
            return View();
        }

        public ActionResult OrganizationSalesReportAjax(int DistributorID)
        {

            ReportViewer rptViewer = new ReportViewer();
            if(DistributorID>0)
            {
                rptViewer.ProcessingMode = ProcessingMode.Local;
                rptViewer.SizeToReportContent = true;
                rptViewer.Width = Unit.Percentage(100);
                rptViewer.Height = Unit.Percentage(100);
                FillDataSet(rds.SalesReportByOrganization.TableName, "sp_ReportOrganizationSalesReport", new SqlParameter("@DistributorID", DistributorID));
                rptViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\SalesByOrganizations.rdlc";
                rptViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", rds.Tables[rds.SalesReportByOrganization.TableName]));
            }            
            ViewBag.ReportViewer = rptViewer;
            return PartialView("_SalesByOrganization");
        }

        #endregion  OrganizationSalesReport


        #region ItemSoldsReport

        public ActionResult ItemSoldReport()
        {
            return View();
        }

        public ActionResult ItemSoldReportAjax(DateTime? StartDate,DateTime? EndDate)
        {
            ReportViewer rptViewer = new ReportViewer();
            if (StartDate.HasValue && EndDate.HasValue)
            {
                rptViewer.ProcessingMode = ProcessingMode.Local;
                rptViewer.SizeToReportContent = true;
                rptViewer.Width = Unit.Percentage(100);
                rptViewer.Height = Unit.Percentage(100);
                FillDataSet(rds.ItemSoldReport.TableName, "sp_ReportITemSold", new SqlParameter("@StartDate", StartDate), new SqlParameter("@EndDate", EndDate));
                rptViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\ItemSoldReport.rdlc";
                rptViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", rds.Tables[rds.ItemSoldReport.TableName]));
            }
            ViewBag.ReportViewer = rptViewer;
            return PartialView("_SalesByOrganization");
        }

        #endregion ItemSoldsReport


        #region ProductReport

        public ActionResult ProductReport()
        {
            ReportViewer rptViewer = new ReportViewer();
            
                rptViewer.ProcessingMode = ProcessingMode.Local;
                rptViewer.SizeToReportContent = true;
                rptViewer.Width = Unit.Percentage(100);
                rptViewer.Height = Unit.Percentage(100);
                FillDataSet(rds.ProductsByCatalog.TableName, "sp_ReportProductWithCategoryandBrochureMapping",null);
                rptViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\ProductReport.rdlc";
                rptViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", rds.Tables[rds.ProductsByCatalog.TableName]));            
            ViewBag.ReportViewer = rptViewer;
            return View();
            
        }

        #endregion ProductReport


        #region SchoolOrders

        public ActionResult SchoolOrders()
        {
            return View();
        }
        
        public ActionResult SchoolOrdersAjax(DateTime? startDate,DateTime? endDate,string SchoolID,string STS)
        {
            bool? IsSTS = null;
            ReportViewer rptViewer = new ReportViewer();
            if (STS.ToUpper()=="STS")
            {
                IsSTS= true;
            }
            else if(STS.ToUpper()=="UPS")
            {
                IsSTS= false;
            }
            
            if(startDate.HasValue && endDate.HasValue && !string.IsNullOrEmpty(SchoolID) && !string.IsNullOrEmpty(STS))
            {
                rptViewer.ProcessingMode = ProcessingMode.Local;
                rptViewer.SizeToReportContent = true;
                rptViewer.Width = Unit.Percentage(100);
                rptViewer.Height = Unit.Percentage(100);
                FillDataSet(rds.SchoolOrders.TableName, "Sp_ReportOrdersBySchool", new SqlParameter("@IsShipToSchool", (object)IsSTS.Value ?? DBNull.Value),
                    new SqlParameter("@startDate",startDate),
                    new SqlParameter("@endDate",endDate),
                    new SqlParameter("@orgID",SchoolID));
                rptViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\SchoolOrders.rdlc";
                rptViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", rds.Tables[rds.SchoolOrders.TableName]));
                

            }

            ViewBag.ReportViewer = rptViewer;
            return PartialView("_SalesByOrganization");
        }
        #endregion Schoolorders


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
                    if(sqlparameters!=null)
                    {
                        foreach (var item in sqlparameters)
                        {
                            cmd.Parameters.Add(item);
                        }

                    }

                   
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(rds,tableName);
                }
            }
        }

    }
    }
