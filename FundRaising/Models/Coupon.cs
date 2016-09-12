using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FundRaising.Models
{
    public class Coupon
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string CouponType { get; set; }
        [Required]       
        public double value { get; set; }
        public int CouponUsage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public bool Active{ get; set; }
    }
}