using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundRaising.ViewModels
{
    public class MagazinePriceMappingModel
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string description { get; set; }
        public double Price { get; set; }
        public int Issue { get; set; }
        public string ItemNumber { get; set; }
    }
}