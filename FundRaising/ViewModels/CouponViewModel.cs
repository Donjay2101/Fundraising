using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundRaising.ViewModels
{
    public class CouponViewModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string CouponType { get; set; }
        public double value { get; set; }
        public string CouponUsage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }
}