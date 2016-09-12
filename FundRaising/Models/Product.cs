using FundRaising.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FundRaising.Models
{
    [Table("Products")]
    public class Product
    {

       
        #region Properties

        public int ID { get; set; }

        [ScaffoldColumn(true)]
        [NotMapped]
        public string check { get; set; }

        [Display(Name="Item Number")]
        [Required(ErrorMessage="*")]
        public string ItemNumber { get; set; }

        [Display(Name = "Product Type")]
        [Required(ErrorMessage = "*")]
        public int productType { get; set; }

        [Display(Name = "Item Description")]
        [Required(ErrorMessage = "*")]
        public string Description { get; set; }

        [Display(Name = "Customer Retail Price")]
        //[Required(ErrorMessage = "*")]
        public double CustomerRetailPrice { get; set; }
        //[Display(Name = "Customer retail Price A")]
        ////[Required(ErrorMessage = "*")]
        //public double CustomerRetailPriceA { get; set; }

        //[Display(Name = "Customer retail Price B")]
        ////[Required(ErrorMessage = "*")]
        //public double CustomerRetailPriceB { get; set; }

        [Display(Name = "Fund Tracker Price")]
        //[Required(ErrorMessage = "*")]
        public double FundTrackerPrice { get; set; }


        //[Display(Name = "FundTracker Price A")]
        ////[Required(ErrorMessage = "*")]
        //public double FundTrackerPriceA { get; set; }

        //[Display(Name = "FundTracker Price B")]
        ////[Required(ErrorMessage = "*")]
        //public double FundTrackerPriceB { get; set; }

        [Display(Name = "Item Weight")]
        [Required(ErrorMessage = "*")]
        public double ItemWeight { get; set; }

        [Display(Name = "Charge Sales Tax?")]
        [Required(ErrorMessage = "*")]
        public bool ChargeSalesTax { get; set; }

        [Display(Name = "Charge Shipping?")]
        [Required(ErrorMessage = "*")]
        public bool ChargeShipping { get; set; }

        [Display(Name = "Item Over Size?")]
        [Required(ErrorMessage = "*")]
        public bool ItemOverSize { get; set; }

        [Display(Name = "Item Active?")]
        [Required(ErrorMessage = "*")]
        public bool ItemActive { get; set; }


        //[Display(Name = "Pics Inverntory?")]
        ////[Required(ErrorMessage = "*")]
        //public bool PicsInventory { get; set; }

        [Display(Name = "Inventory Amount")]
        public int InventoryAmount { get; set; }


        [Display(Name = "Item Extra Title")]
        //[Required(ErrorMessage = "*")]
        public string ItemExtraTitle { get; set; }


        [Display(Name = "Item Extra File Name")]
        
        public string ItemExtraFileName { get; set; }

        [Display(Name = "Detail Description")]
              
        public string DetailDescription { get; set; }

       
        public string ImageUrl { get; set; }

        public bool ShipToSchoolOnly { get; set; }

        public Nullable<int> Issue { get; set; }

        public Nullable<double> Price { get;set;}
       
       
        public bool Inventory { get; set; }

        //public double ShippingAmount { get; set; }
        [NotMapped]
        public virtual ICollection<Category> Categories { get; set; }    
        
        [NotMapped]
        public string MagazinePrice { get; set; }    

        [NotMapped]
        public List<MagazinePriceMappingModel> MagazinePriceList { get; set; }
        #endregion
    }
}