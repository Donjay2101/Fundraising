using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FundRaising.Models
{
    public class Voucher
    {
        [Key]
        public int ID { get; set; }


        [Required(ErrorMessage = "Code required")]
        public string Code { get; set; }

        [Display(Name = "Coupon Type")]
        public string CouponType { get; set; }
        
        [Required(ErrorMessage="value required")]
        public double Value { get; set; }
       
        [Display(Name="Coupon Usage")]
        public string CouponUsage { get; set; }

        [Display(Name = "Start Date")]
        [Required(ErrorMessage="Date required")]
        public DateTime StartDate { get; set; }


        [Display(Name = "End Date")]
        [Required(ErrorMessage = "Date required")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Is Delete")]      
        public bool IsDelete { get; set; }


        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}