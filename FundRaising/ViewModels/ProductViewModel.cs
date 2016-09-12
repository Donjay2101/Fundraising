using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FundRaising.ViewModels
{
    public class ProductViewModel
    {

        public int ID { get; set; }
        public string ItemNumber { get; set; }
        public string Name { get; set; }
        public double CustomerRetailPrice { get; set; }
        public double FundTrackerRetailPrice { get; set; }

        public string sProductType { get; set; }
        public int InventoryAmount { get; set; }

        [Display(Name = "Inventory Amount")]
        public string sInventoryAmount { get; set; }

        public double price { get; set; }
        public int Issue { get; set; }
    }
}