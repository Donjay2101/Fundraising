using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundRaising.Models
{
    public class ShippingCharge
    {
        public int ID { get; set; }
        public double LowerLimit { get; set; }
        public double UpperLimit { get; set; }
        public double Charge { get; set; }
        public double FreeAmount { get; set; }
    }
}