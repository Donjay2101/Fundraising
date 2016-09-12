using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FundRaising.ViewModels
{
    public class ProductShopViewModel
    {
        public int ID { get; set; }
        public string Description { get; set; }        
        public double CustomerRetailPrice { get; set; }
        public string ImageUrl { get; set; }
        [NotMapped]
        public bool ShipToSchoolOnly { get; set; }
    }
}