using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FundRaising.Models
{
    public class OrderDetail
    {
        public int ID { get; set; }
        public int OrderID { get; set; }        
        public int ProductID { get; set; }
        public double unitPrice { get; set; }
        public string itemNumber { get; set; }
        public int Quantity { get; set; }
        public bool ChargeShipping { get; set; }
        public bool ChargeSalesTax { get; set; }
        public bool IsGift { get; set; }
        [NotMapped]
        public List<Product> Products {get;set;}
    }
}