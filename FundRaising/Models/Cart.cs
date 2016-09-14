using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundRaising.Models
{
    public class Cart
    {

        public int ID { get; set; }
        public string itemNumber { get; set; }
        public int productId { get; set; }
        public string Description { get; set; }
        public string CartID { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DateTime DateCreated { get; set; }
        public bool chargeShipping { get; set; }
        public bool chargeSalesTax { get; set; }
        public bool ShipToSchool { get; set; }
        public bool IsGift { get; set; }
    }

}