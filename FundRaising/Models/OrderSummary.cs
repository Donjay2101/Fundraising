using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundRaising.Models
{
    public class OrderSummary
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public double TotalAmount { get; set; }
        public double SalesTax { get; set; }
        public double ShippingAmount { get; set; }
        public double TotalPayable { get; set; }
        public int Quantity { get; set; }
    }
}