using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundRaising.Models
{
    public class GiftCard
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int  ItemID { get; set; }
        public int CartITemID { get; set; }
        public int OrderID { get; set; }
        public int BaseOrderId { get; set; }
    }
}