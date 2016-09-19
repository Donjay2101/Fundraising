using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FundRaising.Models
{
    public class MagazinePrice
    {
        [Required]
        public string ItemNumber { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Issue { get; set; }
    }
}