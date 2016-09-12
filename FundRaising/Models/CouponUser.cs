using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundRaising.Models
{
    public class CouponUser
    {
        public int ID { get; set; }

        public string FirstName { get;set; }
        public string LastName { get; set; }
        //public string Name { get; set; }
        public string EmailID { get; set; }
        public string Couponcode { get; set; }
    }
}