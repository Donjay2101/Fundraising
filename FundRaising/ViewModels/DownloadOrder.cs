using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundRaising.ViewModels
{
    public class DownloadOrder
    {
        public string SchoolID { get; set; }
        public string StudentName { get; set; }
        public string CustomerName { get; set; }
        public string orderNumber { get; set; }
        public string ShipToAddress { get; set; }
        public string City { get;set; }
        public string State { get;set; }
        public string zip { get; set; }
        public string CustomerEmail{get;set;}
        public string Teacher { get; set; }
        public string Grade { get; set; }
        public string StudentID { get; set; }
        public string StudentEmail { get; set; }
        public string ShipToSchool { get; set; }
        public string Quantity { get; set; }
        public string RetailPrice { get; set; }
    }
}